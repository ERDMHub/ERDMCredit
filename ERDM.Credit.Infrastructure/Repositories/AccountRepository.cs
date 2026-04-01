using ERDM.Core;
using ERDM.Credit.Domain.Entities;
using ERDM.Credit.Domain.Interfaces;
using ERDMCore.Infrastructure.MongoDB.Settings;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System.Linq.Expressions;

namespace ERDM.Credit.Infrastructure.Repositories
{
    public class AccountRepository : MongoRepository<Account>, IAccountRepository
    {
        public AccountRepository(IMongoDatabase database, IOptions<MongoDbSettings> settings, ILogger<AccountRepository> logger)
            : base(database, settings, logger)
        {
            CreateIndexes();
        }

        private void CreateIndexes()
        {
            var indexModels = new List<CreateIndexModel<Account>>
            {
                // AccountId unique index (primary key)
                new CreateIndexModel<Account>(
                    Builders<Account>.IndexKeys.Ascending(x => x.AccountId),
                    new CreateIndexOptions { Unique = true, Name = "idx_account_id" }),

                // AccountNumber unique index with sparse option to ignore null/empty
                new CreateIndexModel<Account>(
                    Builders<Account>.IndexKeys.Ascending(x => x.AccountNumber),
                    new CreateIndexOptions {
                        Unique = true,
                        Name = "idx_account_number",
                        Sparse = true  // This allows multiple documents with null/empty AccountNumber
                    }),

                // CustomerId index for customer lookups
                new CreateIndexModel<Account>(
                    Builders<Account>.IndexKeys.Ascending(x => x.CustomerId),
                    new CreateIndexOptions { Name = "idx_customer_id" }),

                // ApplicationId index
                new CreateIndexModel<Account>(
                    Builders<Account>.IndexKeys.Ascending(x => x.ApplicationId),
                    new CreateIndexOptions { Name = "idx_application_id" }),

                // Compound index for status and created date
                new CreateIndexModel<Account>(
                    Builders<Account>.IndexKeys.Combine(
                        Builders<Account>.IndexKeys.Ascending(x => x.Status),
                        Builders<Account>.IndexKeys.Descending(x => x.CreatedAt)),
                    new CreateIndexOptions { Name = "idx_status_created_at" }),

                // Compound index for account type and status
                new CreateIndexModel<Account>(
                    Builders<Account>.IndexKeys.Combine(
                        Builders<Account>.IndexKeys.Ascending(x => x.AccountType),
                        Builders<Account>.IndexKeys.Ascending(x => x.Status)),
                    new CreateIndexOptions { Name = "idx_type_status" }),

                // Index for payment due date queries
                new CreateIndexModel<Account>(
                    Builders<Account>.IndexKeys.Ascending(x => x.NextPaymentDueDate),
                    new CreateIndexOptions { Name = "idx_next_payment_due_date" }),

                // Index for overdue days
                new CreateIndexModel<Account>(
                    Builders<Account>.IndexKeys.Ascending(x => x.DaysOverdue),
                    new CreateIndexOptions { Name = "idx_days_overdue" }),

                // Index for branch code
                new CreateIndexModel<Account>(
                    Builders<Account>.IndexKeys.Ascending(x => x.BranchCode),
                    new CreateIndexOptions { Name = "idx_branch_code" }),

                // Text index for search
                new CreateIndexModel<Account>(
                    Builders<Account>.IndexKeys.Text(x => x.Notes),
                    new CreateIndexOptions { Name = "idx_notes_text" })
            };

            foreach (var index in indexModels)
            {
                try
                {
                    _collection.Indexes.CreateOne(index);
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, "Error creating index {IndexName}. It might already exist.", index.Options.Name);
                }
            }
        }

        public override async Task<Account> AddAsync(Account entity, CancellationToken cancellationToken = default)
        {
            // Generate AccountId if not set
            if (string.IsNullOrEmpty(entity.AccountId))
                entity.AccountId = Guid.NewGuid().ToString();

            // Generate AccountNumber if not set
            if (string.IsNullOrEmpty(entity.AccountNumber))
                entity.AccountNumber = await GenerateUniqueAccountNumber();

            // Set ID to be the same as AccountId for consistency with base repository
            if (string.IsNullOrEmpty(entity.Id))
                entity.Id = entity.AccountId;

            // Set default values if not set
            if (entity.CreatedAt == default)
                entity.CreatedAt = DateTime.UtcNow;

            if (string.IsNullOrEmpty(entity.CreatedBy))
                entity.CreatedBy = "system";

            if (entity.Version == 0)
                entity.Version = 1;

            // Initialize collections if null
            entity.StatusHistory ??= new List<AccountStatusHistory>();
            entity.PaymentHistory ??= new List<PaymentHistory>();
            entity.Metadata ??= new AccountMetadata();
            entity.Tags ??= new List<string>();

            // Set initial payment-related fields
            if (entity.TotalPayments == 0 && entity.TermMonths > 0)
                entity.TotalPayments = entity.TermMonths;

            if (entity.PaymentsRemaining == 0 && entity.TotalPayments > 0)
                entity.PaymentsRemaining = entity.TotalPayments;

            // Calculate EMI if not set and principal amount exists
            if (entity.EmiAmount == 0 && entity.PrincipalAmount > 0 && entity.InterestRate > 0 && entity.TermMonths > 0)
            {
                entity.EmiAmount = CalculateEMI(entity.PrincipalAmount, entity.InterestRate, entity.TermMonths);
            }

            // Set opening date if not set
            if (entity.OpeningDate == default)
                entity.OpeningDate = entity.CreatedAt.Value;

            // Set maturity date if not set and term months exist
            if (!entity.MaturityDate.HasValue && entity.TermMonths > 0)
                entity.MaturityDate = entity.OpeningDate.AddMonths(entity.TermMonths);

            // Set next payment due date if not set and account is active/disbursed
            if (!entity.NextPaymentDueDate.HasValue &&
                (entity.Status == AccountStatus.Active || entity.Status == AccountStatus.Disbursed) &&
                entity.TermMonths > 0)
            {
                entity.NextPaymentDueDate = entity.OpeningDate.AddMonths(1);
            }

            // Ensure Currency is set
            if (string.IsNullOrEmpty(entity.Currency))
                entity.Currency = "USD";

            // Calculate available credit if not set
            if (entity.AvailableCredit == 0 && entity.ApprovedAmount > 0)
                entity.AvailableCredit = entity.ApprovedAmount - entity.PrincipalAmount;

            // Set disbursed amount if not set
            if (entity.DisbursedAmount == 0 && entity.Status == AccountStatus.Disbursed)
                entity.DisbursedAmount = entity.PrincipalAmount;

            // Add initial status history if not exists
            if (!entity.StatusHistory.Any())
            {
                entity.StatusHistory.Add(new AccountStatusHistory
                {
                    Status = entity.Status,
                    ChangedAt = entity.CreatedAt.Value,
                    ChangedBy = entity.CreatedBy,
                    Reason = "Account created"
                });
            }

            _logger.LogInformation("Adding new account: AccountId={AccountId}, AccountNumber={AccountNumber}, CustomerId={CustomerId}, ProductType={ProductType}, Amount={Amount}",
                entity.AccountId, entity.AccountNumber, entity.CustomerId, entity.ProductType, entity.PrincipalAmount);

            return await base.AddAsync(entity, cancellationToken);
        }

        #region Basic Queries

        public async Task<Account> GetByAccountIdAsync(string accountId)
        {
            var filter = Builders<Account>.Filter.Eq(x => x.AccountId, accountId);
            return await _collection.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<Account> GetByAccountNumberAsync(string accountNumber)
        {
            var filter = Builders<Account>.Filter.Eq(x => x.AccountNumber, accountNumber);
            return await _collection.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Account>> GetByCustomerIdAsync(string customerId)
        {
            var filter = Builders<Account>.Filter.Eq(x => x.CustomerId, customerId);
            var sort = Builders<Account>.Sort.Descending(x => x.CreatedAt);
            return await _collection.Find(filter).Sort(sort).ToListAsync();
        }

        public async Task<IEnumerable<Account>> GetByApplicationIdAsync(string applicationId)
        {
            var filter = Builders<Account>.Filter.Eq(x => x.ApplicationId, applicationId);
            return await _collection.Find(filter).ToListAsync();
        }

        #endregion

        #region Status-based Queries

        public async Task<IEnumerable<Account>> GetByStatusAsync(AccountStatus status)
        {
            var filter = Builders<Account>.Filter.Eq(x => x.Status, status);
            var sort = Builders<Account>.Sort.Descending(x => x.CreatedAt);
            return await _collection.Find(filter).Sort(sort).ToListAsync();
        }

        public async Task<IEnumerable<Account>> GetActiveAccountsAsync()
        {
            var filter = Builders<Account>.Filter.In(x => x.Status, new[]
            {
                AccountStatus.Active,
                AccountStatus.Disbursed,
                AccountStatus.Approved
            });
            var sort = Builders<Account>.Sort.Descending(x => x.CreatedAt);
            return await _collection.Find(filter).Sort(sort).ToListAsync();
        }

        public async Task<IEnumerable<Account>> GetDelinquentAccountsAsync()
        {
            var filter = Builders<Account>.Filter.Eq(x => x.IsDelinquent, true);
            var sort = Builders<Account>.Sort.Descending(x => x.DaysOverdue);
            return await _collection.Find(filter).Sort(sort).ToListAsync();
        }

        public async Task<IEnumerable<Account>> GetAccountsDueForPaymentAsync(DateTime cutoffDate)
        {
            var filter = Builders<Account>.Filter.And(
                Builders<Account>.Filter.In(x => x.Status, new[] { AccountStatus.Active, AccountStatus.Disbursed }),
                Builders<Account>.Filter.Lte(x => x.NextPaymentDueDate, cutoffDate),
                Builders<Account>.Filter.Gt(x => x.OutstandingBalance, 0)
            );
            var sort = Builders<Account>.Sort.Ascending(x => x.NextPaymentDueDate);
            return await _collection.Find(filter).Sort(sort).ToListAsync();
        }

        public async Task<IEnumerable<Account>> GetOverdueAccountsAsync()
        {
            var filter = Builders<Account>.Filter.And(
                Builders<Account>.Filter.In(x => x.Status, new[] { AccountStatus.Active, AccountStatus.Disbursed, AccountStatus.Delinquent }),
                Builders<Account>.Filter.Lt(x => x.NextPaymentDueDate, DateTime.UtcNow),
                Builders<Account>.Filter.Gt(x => x.OutstandingBalance, 0)
            );
            var sort = Builders<Account>.Sort.Ascending(x => x.NextPaymentDueDate);
            return await _collection.Find(filter).Sort(sort).ToListAsync();
        }

        public async Task<IEnumerable<Account>> GetMaturedAccountsAsync()
        {
            var filter = Builders<Account>.Filter.And(
                Builders<Account>.Filter.In(x => x.Status, new[] { AccountStatus.Active, AccountStatus.Disbursed }),
                Builders<Account>.Filter.Lte(x => x.MaturityDate, DateTime.UtcNow),
                Builders<Account>.Filter.Eq(x => x.OutstandingBalance, 0)
            );
            return await _collection.Find(filter).ToListAsync();
        }

        #endregion

        #region Type-based Queries

        public async Task<IEnumerable<Account>> GetByAccountTypeAsync(AccountType accountType)
        {
            var filter = Builders<Account>.Filter.Eq(x => x.AccountType, accountType);
            var sort = Builders<Account>.Sort.Descending(x => x.CreatedAt);
            return await _collection.Find(filter).Sort(sort).ToListAsync();
        }

        public async Task<IEnumerable<Account>> GetByProductTypeAsync(string productType)
        {
            var filter = Builders<Account>.Filter.Eq(x => x.ProductType, productType);
            var sort = Builders<Account>.Sort.Descending(x => x.CreatedAt);
            return await _collection.Find(filter).Sort(sort).ToListAsync();
        }

        #endregion

        #region Financial Queries

        public async Task<decimal> GetTotalOutstandingBalanceByCustomerAsync(string customerId)
        {
            var filter = Builders<Account>.Filter.And(
                Builders<Account>.Filter.Eq(x => x.CustomerId, customerId),
                Builders<Account>.Filter.In(x => x.Status, new[] { AccountStatus.Active, AccountStatus.Disbursed, AccountStatus.Delinquent })
            );

            var result = await _collection.Aggregate()
                .Match(filter)
                .Group(x => x.CustomerId, g => new { Total = g.Sum(x => x.OutstandingBalance) })
                .FirstOrDefaultAsync();

            return result?.Total ?? 0;
        }

        public async Task<decimal> GetTotalDisbursedAmountByCustomerAsync(string customerId)
        {
            var filter = Builders<Account>.Filter.And(
                Builders<Account>.Filter.Eq(x => x.CustomerId, customerId),
                Builders<Account>.Filter.In(x => x.Status, new[] { AccountStatus.Active, AccountStatus.Disbursed, AccountStatus.Delinquent, AccountStatus.Closed })
            );

            var result = await _collection.Aggregate()
                .Match(filter)
                .Group(x => x.CustomerId, g => new { Total = g.Sum(x => x.DisbursedAmount) })
                .FirstOrDefaultAsync();

            return result?.Total ?? 0;
        }

        public async Task<decimal> GetAvailableCreditByCustomerAsync(string customerId)
        {
            var filter = Builders<Account>.Filter.And(
                Builders<Account>.Filter.Eq(x => x.CustomerId, customerId),
                Builders<Account>.Filter.In(x => x.Status, new[] { AccountStatus.Active, AccountStatus.Disbursed })
            );

            var result = await _collection.Aggregate()
                .Match(filter)
                .Group(x => x.CustomerId, g => new { Total = g.Sum(x => x.AvailableCredit) })
                .FirstOrDefaultAsync();

            return result?.Total ?? 0;
        }

        public async Task<Dictionary<AccountStatus, decimal>> GetBalanceSummaryByStatusAsync()
        {
            var result = await _collection.Aggregate()
                .Group(x => x.Status, g => new { Status = g.Key, TotalBalance = g.Sum(x => x.OutstandingBalance) })
                .ToListAsync();

            return result.ToDictionary(x => x.Status, x => x.TotalBalance);
        }

        public async Task<Dictionary<AccountType, decimal>> GetBalanceSummaryByTypeAsync()
        {
            var result = await _collection.Aggregate()
                .Group(x => x.AccountType, g => new { Type = g.Key, TotalBalance = g.Sum(x => x.OutstandingBalance) })
                .ToListAsync();

            return result.ToDictionary(x => x.Type, x => x.TotalBalance);
        }

        #endregion

        #region Statistics and Analytics

        public async Task<Dictionary<AccountStatus, int>> GetAccountStatisticsByStatusAsync()
        {
            var result = await _collection.Aggregate()
                .Group(x => x.Status, g => new { Status = g.Key, Count = g.Count() })
                .ToListAsync();

            return result.ToDictionary(x => x.Status, x => x.Count);
        }

        public async Task<Dictionary<AccountType, int>> GetAccountStatisticsByTypeAsync()
        {
            var result = await _collection.Aggregate()
                .Group(x => x.AccountType, g => new { Type = g.Key, Count = g.Count() })
                .ToListAsync();

            return result.ToDictionary(x => x.Type, x => x.Count);
        }

        #endregion

        #region Payment-related

        public async Task<Account> GetAccountWithPaymentsAsync(string accountId)
        {
            var filter = Builders<Account>.Filter.Eq(x => x.AccountId, accountId);
            return await _collection.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<PaymentHistory>> GetPaymentHistoryAsync(string accountId, DateTime? fromDate = null, DateTime? toDate = null)
        {
            var account = await GetByAccountIdAsync(accountId);
            if (account == null)
                return new List<PaymentHistory>();

            var payments = account.PaymentHistory.AsQueryable();

            if (fromDate.HasValue)
                payments = payments.Where(p => p.PaymentDate >= fromDate.Value);

            if (toDate.HasValue)
                payments = payments.Where(p => p.PaymentDate <= toDate.Value);

            return payments.OrderByDescending(p => p.PaymentDate).ToList();
        }

        public async Task<PaymentHistory?> GetLastPaymentAsync(string accountId)
        {
            var account = await GetByAccountIdAsync(accountId);
            return account?.PaymentHistory.OrderByDescending(p => p.PaymentDate).FirstOrDefault();
        }

        public async Task<decimal> GetTotalPaymentsByAccountAsync(string accountId)
        {
            var account = await GetByAccountIdAsync(accountId);
            return account?.PaymentHistory.Sum(p => p.AmountPaid) ?? 0;
        }

        public async Task<decimal> GetTotalInterestEarnedByAccountAsync(string accountId)
        {
            var account = await GetByAccountIdAsync(accountId);
            return account?.PaymentHistory.Sum(p => p.InterestPaid) ?? 0;
        }

        #endregion

        #region Paginated Results

        public async Task<PaginatedResult<Account>> GetPaginatedByCustomerAsync(
            string customerId,
            int pageNumber,
            int pageSize,
            string sortBy = "CreatedAt",
            bool sortDescending = true)
        {
            Expression<Func<Account, bool>> predicate = x => x.CustomerId == customerId;

            Expression<Func<Account, object>> sortExpression = sortBy?.ToLower() switch
            {
                "accountnumber" => x => x.AccountNumber,
                "status" => x => x.Status,
                "outstandingbalance" => x => x.OutstandingBalance,
                "nextpaymentduedate" => x => x.NextPaymentDueDate,
                "createdat" => x => x.CreatedAt,
                _ => x => x.CreatedAt
            };

            return await GetPaginatedAsync(pageNumber, pageSize, predicate, sortExpression, sortDescending);
        }

        public async Task<PaginatedResult<Account>> GetPaginatedByStatusAsync(
            AccountStatus status,
            int pageNumber,
            int pageSize,
            string sortBy = "CreatedAt",
            bool sortDescending = true)
        {
            Expression<Func<Account, bool>> predicate = x => x.Status == status;

            Expression<Func<Account, object>> sortExpression = sortBy?.ToLower() switch
            {
                "accountnumber" => x => x.AccountNumber,
                "outstandingbalance" => x => x.OutstandingBalance,
                "nextpaymentduedate" => x => x.NextPaymentDueDate,
                "createdat" => x => x.CreatedAt,
                _ => x => x.CreatedAt
            };

            return await GetPaginatedAsync(pageNumber, pageSize, predicate, sortExpression, sortDescending);
        }

        public async Task<PaginatedResult<Account>> GetPaginatedByDateRangeAsync(
            DateTime fromDate,
            DateTime toDate,
            int pageNumber,
            int pageSize,
            string sortBy = "CreatedAt",
            bool sortDescending = true)
        {
            Expression<Func<Account, bool>> predicate = x => x.CreatedAt >= fromDate && x.CreatedAt <= toDate;

            Expression<Func<Account, object>> sortExpression = sortBy?.ToLower() switch
            {
                "accountnumber" => x => x.AccountNumber,
                "status" => x => x.Status,
                "outstandingbalance" => x => x.OutstandingBalance,
                "createdat" => x => x.CreatedAt,
                _ => x => x.CreatedAt
            };

            return await GetPaginatedAsync(pageNumber, pageSize, predicate, sortExpression, sortDescending);
        }

        public async Task<PaginatedResult<Account>> GetPaginatedWithFiltersAsync(
            AccountFilter filter,
            int pageNumber,
            int pageSize,
            string sortBy = "CreatedAt",
            bool sortDescending = true)
        {
            var filterBuilder = Builders<Account>.Filter;
            var filters = new List<FilterDefinition<Account>>();

            if (!string.IsNullOrEmpty(filter.CustomerId))
                filters.Add(filterBuilder.Eq(x => x.CustomerId, filter.CustomerId));

            if (filter.Status.HasValue)
                filters.Add(filterBuilder.Eq(x => x.Status, filter.Status.Value));

            if (filter.AccountType.HasValue)
                filters.Add(filterBuilder.Eq(x => x.AccountType, filter.AccountType.Value));

            if (!string.IsNullOrEmpty(filter.ProductType))
                filters.Add(filterBuilder.Eq(x => x.ProductType, filter.ProductType));

            if (filter.FromDate.HasValue)
                filters.Add(filterBuilder.Gte(x => x.CreatedAt, filter.FromDate.Value));

            if (filter.ToDate.HasValue)
                filters.Add(filterBuilder.Lte(x => x.CreatedAt, filter.ToDate.Value));

            if (filter.MinBalance.HasValue)
                filters.Add(filterBuilder.Gte(x => x.OutstandingBalance, filter.MinBalance.Value));

            if (filter.MaxBalance.HasValue)
                filters.Add(filterBuilder.Lte(x => x.OutstandingBalance, filter.MaxBalance.Value));

            if (!string.IsNullOrEmpty(filter.BranchCode))
                filters.Add(filterBuilder.Eq(x => x.BranchCode, filter.BranchCode));

            if (!string.IsNullOrEmpty(filter.AssignedOfficer))
                filters.Add(filterBuilder.Eq(x => x.AssignedOfficer, filter.AssignedOfficer));

            if (filter.IsDelinquent.HasValue)
                filters.Add(filterBuilder.Eq(x => x.IsDelinquent, filter.IsDelinquent.Value));

            if (filter.MinOverdueDays.HasValue)
                filters.Add(filterBuilder.Gte(x => x.DaysOverdue, filter.MinOverdueDays.Value));

            if (!string.IsNullOrEmpty(filter.SearchTerm))
                filters.Add(filterBuilder.Text(filter.SearchTerm));

            var finalFilter = filters.Any()
                ? filterBuilder.And(filters)
                : filterBuilder.Empty;

            Expression<Func<Account, object>> sortExpression = sortBy?.ToLower() switch
            {
                "accountnumber" => x => x.AccountNumber,
                "status" => x => x.Status,
                "outstandingbalance" => x => x.OutstandingBalance,
                "nextpaymentduedate" => x => x.NextPaymentDueDate,
                "createdat" => x => x.CreatedAt,
                _ => x => x.CreatedAt
            };

            var sortDefinition = sortDescending
                ? Builders<Account>.Sort.Descending(sortExpression)
                : Builders<Account>.Sort.Ascending(sortExpression);

            var totalCount = await _collection.CountDocumentsAsync(finalFilter);
            var items = await _collection
                .Find(finalFilter)
                .Sort(sortDefinition)
                .Skip((pageNumber - 1) * pageSize)
                .Limit(pageSize)
                .ToListAsync();

            return new PaginatedResult<Account>
            {
                Data = items,
                TotalCount = (int)totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }

        #endregion

        #region Update Operations

        public async Task<bool> UpdateAccountStatusAsync(string accountId, AccountStatus newStatus, string reason, string updatedBy)
        {
            var filter = Builders<Account>.Filter.Eq(x => x.AccountId, accountId);
            var update = Builders<Account>.Update
                .Set(x => x.Status, newStatus)
                .Set(x => x.UpdatedAt, DateTime.UtcNow)
                .Set(x => x.UpdatedBy, updatedBy)
                .Push(x => x.StatusHistory, new AccountStatusHistory
                {
                    Status = newStatus,
                    ChangedAt = DateTime.UtcNow,
                    ChangedBy = updatedBy,
                    Reason = reason
                });

            var result = await _collection.UpdateOneAsync(filter, update);
            return result.ModifiedCount > 0;
        }

        public async Task<bool> UpdateOutstandingBalanceAsync(string accountId, decimal newBalance)
        {
            var filter = Builders<Account>.Filter.Eq(x => x.AccountId, accountId);
            var update = Builders<Account>.Update
                .Set(x => x.OutstandingBalance, newBalance)
                .Set(x => x.UpdatedAt, DateTime.UtcNow);

            var result = await _collection.UpdateOneAsync(filter, update);
            return result.ModifiedCount > 0;
        }

        public async Task<bool> UpdateAvailableCreditAsync(string accountId, decimal newAvailableCredit)
        {
            var filter = Builders<Account>.Filter.Eq(x => x.AccountId, accountId);
            var update = Builders<Account>.Update
                .Set(x => x.AvailableCredit, newAvailableCredit)
                .Set(x => x.UpdatedAt, DateTime.UtcNow);

            var result = await _collection.UpdateOneAsync(filter, update);
            return result.ModifiedCount > 0;
        }

        public async Task<bool> AddPaymentHistoryAsync(string accountId, PaymentHistory payment)
        {
            var filter = Builders<Account>.Filter.Eq(x => x.AccountId, accountId);
            var update = Builders<Account>.Update
                .Push(x => x.PaymentHistory, payment)
                .Set(x => x.LastPaymentDate, payment.PaymentDate)
                .Set(x => x.LastPaymentAmount, payment.AmountPaid)
                .Inc(x => x.PaymentsMade, 1)
                .Set(x => x.UpdatedAt, DateTime.UtcNow);

            if (payment.Status == PaymentStatus.Paid && payment.AmountPaid >= payment.AmountDue)
            {
                update = update.Set(x => x.NextPaymentDueDate, CalculateNextDueDate(payment.DueDate));
            }

            var result = await _collection.UpdateOneAsync(filter, update);
            return result.ModifiedCount > 0;
        }

        public async Task<bool> RecordLatePaymentAsync(string accountId, int lateDays, decimal lateFee)
        {
            var filter = Builders<Account>.Filter.Eq(x => x.AccountId, accountId);
            var update = Builders<Account>.Update
                .Set(x => x.IsDelinquent, true)
                .Set(x => x.DaysOverdue, lateDays)
                .Set(x => x.UpdatedAt, DateTime.UtcNow);

            if (lateFee > 0)
            {
                update = update.Inc(x => x.OutstandingBalance, lateFee);
            }

            var result = await _collection.UpdateOneAsync(filter, update);
            return result.ModifiedCount > 0;
        }

        #endregion

        #region Bulk Operations

        public async Task<bool> BulkUpdateStatusAsync(List<string> accountIds, AccountStatus newStatus, string reason, string updatedBy)
        {
            var filter = Builders<Account>.Filter.In(x => x.AccountId, accountIds);
            var update = Builders<Account>.Update
                .Set(x => x.Status, newStatus)
                .Set(x => x.UpdatedAt, DateTime.UtcNow)
                .Set(x => x.UpdatedBy, updatedBy);

            var result = await _collection.UpdateManyAsync(filter, update);
            return result.ModifiedCount > 0;
        }

        public async Task<int> BulkMarkOverdueAccountsAsync(DateTime asOfDate)
        {
            try
            {
                var filter = Builders<Account>.Filter.And(
                    Builders<Account>.Filter.In(x => x.Status, new[] { AccountStatus.Active, AccountStatus.Disbursed }),
                    Builders<Account>.Filter.Lt(x => x.NextPaymentDueDate, asOfDate),
                    Builders<Account>.Filter.Gt(x => x.OutstandingBalance, 0),
                    Builders<Account>.Filter.Exists(x => x.AccountId, true),
                    Builders<Account>.Filter.Not(Builders<Account>.Filter.Eq(x => x.AccountNumber, null)),
                    Builders<Account>.Filter.Not(Builders<Account>.Filter.Eq(x => x.AccountNumber, ""))
                );

                var accounts = await _collection.Find(filter).ToListAsync();

                if (!accounts.Any())
                    return 0;

                var bulkOps = new List<WriteModel<Account>>();

                foreach (var account in accounts)
                {
                    var daysOverdue = account.NextPaymentDueDate.HasValue
                        ? (int)Math.Max(0, (asOfDate - account.NextPaymentDueDate.Value).TotalDays)
                        : 0;

                    var updateFilter = Builders<Account>.Filter.Eq(x => x.AccountId, account.AccountId);
                    var update = Builders<Account>.Update
                        .Set(x => x.IsDelinquent, true)
                        .Set(x => x.Status, AccountStatus.Delinquent)
                        .Set(x => x.UpdatedAt, DateTime.UtcNow)
                        .Set(x => x.LastCollectionAttempt, asOfDate)
                        .Set(x => x.DaysOverdue, daysOverdue)
                        .Set(x => x.UpdatedBy, "system");

                    bulkOps.Add(new UpdateOneModel<Account>(updateFilter, update));
                }

                var result = await _collection.BulkWriteAsync(bulkOps, new BulkWriteOptions
                {
                    IsOrdered = false
                });

                _logger.LogInformation("Marked {ModifiedCount} accounts as overdue as of {AsOfDate}", result.ModifiedCount, asOfDate);

                return (int)result.ModifiedCount;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error bulk marking overdue accounts as of {AsOfDate}", asOfDate);
                throw;
            }
        }

        public async Task<int> UpdateNextPaymentDatesAsync()
        {
            var filter = Builders<Account>.Filter.And(
                Builders<Account>.Filter.In(x => x.Status, new[] { AccountStatus.Active, AccountStatus.Disbursed }),
                Builders<Account>.Filter.Eq(x => x.PaymentsRemaining, 0)
            );

            var update = Builders<Account>.Update
                .Set(x => x.Status, AccountStatus.Closed)
                .Set(x => x.ClosingDate, DateTime.UtcNow)
                .Set(x => x.UpdatedAt, DateTime.UtcNow);

            var result = await _collection.UpdateManyAsync(filter, update);
            return (int)result.ModifiedCount;
        }

        #endregion

        #region Validation

        public async Task<bool> AccountExistsAsync(string accountNumber)
        {
            if (string.IsNullOrEmpty(accountNumber))
                return false;

            var filter = Builders<Account>.Filter.Eq(x => x.AccountNumber, accountNumber);
            var count = await _collection.CountDocumentsAsync(filter);
            return count > 0;
        }

        public async Task<bool> IsAccountActiveAsync(string accountId)
        {
            var filter = Builders<Account>.Filter.And(
                Builders<Account>.Filter.Eq(x => x.AccountId, accountId),
                Builders<Account>.Filter.In(x => x.Status, new[] { AccountStatus.Active, AccountStatus.Disbursed })
            );
            var count = await _collection.CountDocumentsAsync(filter);
            return count > 0;
        }

        public async Task<bool> HasOutstandingBalanceAsync(string accountId)
        {
            var filter = Builders<Account>.Filter.Eq(x => x.AccountId, accountId);
            var account = await _collection.Find(filter).FirstOrDefaultAsync();
            return account?.OutstandingBalance > 0;
        }

        public async Task<DateTime?> GetNextPaymentDueDateAsync(string accountId)
        {
            var filter = Builders<Account>.Filter.Eq(x => x.AccountId, accountId);
            var account = await _collection.Find(filter).FirstOrDefaultAsync();
            return account?.NextPaymentDueDate;
        }

        #endregion

        #region Reporting

        public async Task<Dictionary<string, object>> GetAccountPerformanceMetricsAsync(string accountId)
        {
            var account = await GetByAccountIdAsync(accountId);
            if (account == null)
                return new Dictionary<string, object>();

            var metrics = new Dictionary<string, object>
            {
                ["AccountId"] = account.AccountId,
                ["AccountNumber"] = account.AccountNumber,
                ["CustomerId"] = account.CustomerId,
                ["Status"] = account.Status.ToString(),
                ["OutstandingBalance"] = account.OutstandingBalance,
                ["TotalPayments"] = account.PaymentHistory.Count,
                ["TotalAmountPaid"] = account.PaymentHistory.Sum(p => p.AmountPaid),
                ["TotalInterestPaid"] = account.PaymentHistory.Sum(p => p.InterestPaid),
                ["TotalLateFees"] = account.PaymentHistory.Sum(p => p.FeesPaid),
                ["OnTimePayments"] = account.PaymentHistory.Count(p => p.LateDays == 0),
                ["LatePayments"] = account.PaymentHistory.Count(p => p.LateDays > 0),
                ["AveragePaymentAmount"] = account.PaymentHistory.Any()
                    ? account.PaymentHistory.Average(p => p.AmountPaid)
                    : 0,
                ["PaymentConsistency"] = account.PaymentHistory.Any()
                    ? (decimal)account.PaymentHistory.Count(p => p.LateDays == 0) / account.PaymentHistory.Count * 100
                    : 0,
                ["DaysSinceLastPayment"] = account.LastPaymentDate.HasValue
                    ? (DateTime.UtcNow - account.LastPaymentDate.Value).Days
                    : (int?)null,
                ["IsDelinquent"] = account.IsDelinquent,
                ["DaysOverdue"] = account.DaysOverdue
            };

            return metrics;
        }

        public async Task<List<Account>> GetAccountsForCollectionAsync(int daysOverdue)
        {
            var filter = Builders<Account>.Filter.And(
                Builders<Account>.Filter.In(x => x.Status, new[] { AccountStatus.Delinquent }),
                Builders<Account>.Filter.Gte(x => x.DaysOverdue, daysOverdue),
                Builders<Account>.Filter.Gt(x => x.OutstandingBalance, 0)
            );
            var sort = Builders<Account>.Sort.Descending(x => x.DaysOverdue);
            return await _collection.Find(filter).Sort(sort).ToListAsync();
        }

        #endregion

        #region Private Helper Methods

        private async Task<string> GenerateUniqueAccountNumber()
        {
            var prefix = "ACC";
            var datePart = DateTime.Now.ToString("yyyyMMdd");
            var randomPart = GenerateRandomString(8);
            var accountNumber = $"{prefix}-{datePart}-{randomPart}";

            var exists = await AccountExistsAsync(accountNumber);
            if (exists)
            {
                return await GenerateUniqueAccountNumber();
            }

            return accountNumber;
        }

        private string GenerateRandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        private decimal CalculateEMI(decimal principal, decimal interestRate, int termMonths)
        {
            var monthlyRate = interestRate / 1200;
            var power = (decimal)Math.Pow(1 + (double)monthlyRate, termMonths);
            var emi = principal * monthlyRate * power / (power - 1);
            return Math.Round(emi, 2);
        }

        private static DateTime CalculateNextDueDate(DateTime currentDueDate)
        {
            return currentDueDate.AddMonths(1);
        }

        #endregion
    }
}