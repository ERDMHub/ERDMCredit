using ERDM.Core;
using ERDM.Credit.Domain.DomainEvents;
using ERDM.Credit.Domain.Entities;
using ERDM.Credit.Domain.Interfaces;
using ERDMCore.Infrastructure.MongoDB.Settings;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System.Linq.Expressions;

namespace ERDM.Credit.Infrastructure.Repositories
{
    public class LimitHistoryRepository : MongoRepository<LimitHistory>, ILimitHistoryRepository
    {
        public LimitHistoryRepository(IMongoDatabase database, IOptions<MongoDbSettings> settings, ILogger<LimitHistoryRepository> logger)
            : base(database, settings, logger)
        {
            CreateIndexes();
        }

        private void CreateIndexes()
        {
            var indexModels = new List<CreateIndexModel<LimitHistory>>
            {
                new CreateIndexModel<LimitHistory>(
                    Builders<LimitHistory>.IndexKeys.Ascending(x => x.LimitHistoryId),
                    new CreateIndexOptions { Unique = true, Name = "idx_limit_history_id" }),

                new CreateIndexModel<LimitHistory>(
                    Builders<LimitHistory>.IndexKeys.Ascending(x => x.CustomerId),
                    new CreateIndexOptions { Name = "idx_customer_id" }),

                new CreateIndexModel<LimitHistory>(
                    Builders<LimitHistory>.IndexKeys.Ascending(x => x.AccountId),
                    new CreateIndexOptions { Name = "idx_account_id" }),

                new CreateIndexModel<LimitHistory>(
                    Builders<LimitHistory>.IndexKeys.Combine(
                        Builders<LimitHistory>.IndexKeys.Ascending(x => x.ChangeType),
                        Builders<LimitHistory>.IndexKeys.Descending(x => x.ChangedDate)),
                    new CreateIndexOptions { Name = "idx_type_date" }),

                new CreateIndexModel<LimitHistory>(
                    Builders<LimitHistory>.IndexKeys.Ascending(x => x.ExpiryDate),
                    new CreateIndexOptions { Name = "idx_expiry_date" }),

                new CreateIndexModel<LimitHistory>(
                    Builders<LimitHistory>.IndexKeys.Combine(
                        Builders<LimitHistory>.IndexKeys.Ascending(x => x.AccountId),
                        Builders<LimitHistory>.IndexKeys.Descending(x => x.ChangedDate)),
                    new CreateIndexOptions { Name = "idx_account_latest" })
            };

            foreach (var index in indexModels)
            {
                try
                {
                    _collection.Indexes.CreateOne(index);
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, "Error creating index {IndexName}", index.Options.Name);
                }
            }
        }

        public override async Task<LimitHistory> AddAsync(LimitHistory entity, CancellationToken cancellationToken = default)
        {
            // Generate LimitHistoryId if not set
            if (string.IsNullOrEmpty(entity.LimitHistoryId))
                entity.LimitHistoryId =$"LH-{DateTime.Now:yyyyMMdd}-{Guid.NewGuid().ToString().Substring(0, 8).ToUpper()}";

            // Set default values
            if (entity.ChangedDate == default)
                entity.ChangedDate = DateTime.UtcNow;

            if (entity.CreatedAt == default)
                entity.CreatedAt = DateTime.UtcNow;

            if (string.IsNullOrEmpty(entity.CreatedBy))
                entity.CreatedBy = entity.ChangedBy ?? "system";

            if (entity.Version == 0)
                entity.Version = 1;

            // Set ID for base repository
            if (string.IsNullOrEmpty(entity.Id))
                entity.Id = entity.LimitHistoryId;

            // Initialize collections
            entity.Tags ??= new List<string>();
            entity.Metadata ??= new LimitHistoryMetadata();

            // Note: Domain events should be raised in the service layer, not here
            // The service calls entity.RaiseCreatedEvent() which adds the event

            return await base.AddAsync(entity, cancellationToken);
        }

        public async Task<LimitHistory?> GetByLimitHistoryIdAsync(string limitHistoryId)
        {
            var filter = Builders<LimitHistory>.Filter.Eq(x => x.LimitHistoryId, limitHistoryId);
            return await _collection.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<LimitHistory>> GetByCustomerIdAsync(string customerId)
        {
            var filter = Builders<LimitHistory>.Filter.Eq(x => x.CustomerId, customerId);
            var sort = Builders<LimitHistory>.Sort.Descending(x => x.ChangedDate);
            return await _collection.Find(filter).Sort(sort).ToListAsync();
        }

        public async Task<IEnumerable<LimitHistory>> GetByAccountIdAsync(string accountId)
        {
            var filter = Builders<LimitHistory>.Filter.Eq(x => x.AccountId, accountId);
            var sort = Builders<LimitHistory>.Sort.Descending(x => x.ChangedDate);
            return await _collection.Find(filter).Sort(sort).ToListAsync();
        }

        public async Task<IEnumerable<LimitHistory>> GetByChangeTypeAsync(LimitChangeType changeType)
        {
            var filter = Builders<LimitHistory>.Filter.Eq(x => x.ChangeType, changeType);
            var sort = Builders<LimitHistory>.Sort.Descending(x => x.ChangedDate);
            return await _collection.Find(filter).Sort(sort).ToListAsync();
        }

        public async Task<IEnumerable<LimitHistory>> GetTemporaryLimitsAsync(bool activeOnly = true)
        {
            var filter = Builders<LimitHistory>.Filter.Eq(x => x.IsTemporary, true);
            
            if (activeOnly)
            {
                filter = Builders<LimitHistory>.Filter.And(
                    filter,
                    Builders<LimitHistory>.Filter.Or(
                        Builders<LimitHistory>.Filter.Exists(x => x.ExpiryDate, false),
                        Builders<LimitHistory>.Filter.Gt(x => x.ExpiryDate, DateTime.UtcNow)
                    )
                );
            }
            
            var sort = Builders<LimitHistory>.Sort.Ascending(x => x.ExpiryDate);
            return await _collection.Find(filter).Sort(sort).ToListAsync();
        }

        public async Task<IEnumerable<LimitHistory>> GetExpiredTemporaryLimitsAsync(DateTime asOfDate)
        {
            var filter = Builders<LimitHistory>.Filter.And(
                Builders<LimitHistory>.Filter.Eq(x => x.IsTemporary, true),
                Builders<LimitHistory>.Filter.Lt(x => x.ExpiryDate, asOfDate)
            );
            return await _collection.Find(filter).ToListAsync();
        }

        public async Task<LimitHistory?> GetLatestLimitChangeAsync(string accountId)
        {
            var filter = Builders<LimitHistory>.Filter.Eq(x => x.AccountId, accountId);
            var sort = Builders<LimitHistory>.Sort.Descending(x => x.ChangedDate);
            return await _collection.Find(filter).Sort(sort).FirstOrDefaultAsync();
        }

        public async Task<decimal> GetCurrentLimitAsync(string accountId)
        {
            var latest = await GetLatestLimitChangeAsync(accountId);
            return latest?.NewLimit ?? 0;
        }

        public async Task<Dictionary<string, int>> GetLimitStatisticsAsync(DateTime? fromDate = null, DateTime? toDate = null)
        {
            var filterBuilder = Builders<LimitHistory>.Filter;
            var filters = new List<FilterDefinition<LimitHistory>>();

            if (fromDate.HasValue)
                filters.Add(filterBuilder.Gte(x => x.ChangedDate, fromDate.Value));
            if (toDate.HasValue)
                filters.Add(filterBuilder.Lte(x => x.ChangedDate, toDate.Value));

            var finalFilter = filters.Any() ? filterBuilder.And(filters) : filterBuilder.Empty;

            var result = await _collection.Aggregate()
                .Match(finalFilter)
                .Group(x => x.ChangeType.ToString(), g => new { Type = g.Key, Count = g.Count() })
                .ToListAsync();

            return result.ToDictionary(x => x.Type, x => x.Count);
        }

        public async Task<PaginatedResult<LimitHistory>> GetPaginatedWithFiltersAsync(
            LimitHistoryFilter filter,
            int pageNumber,
            int pageSize,
            string sortBy = "ChangedDate",
            bool sortDescending = true)
        {
            var filterBuilder = Builders<LimitHistory>.Filter;
            var filters = new List<FilterDefinition<LimitHistory>>();

            if (!string.IsNullOrEmpty(filter.CustomerId))
                filters.Add(filterBuilder.Eq(x => x.CustomerId, filter.CustomerId));
            if (!string.IsNullOrEmpty(filter.AccountId))
                filters.Add(filterBuilder.Eq(x => x.AccountId, filter.AccountId));
            if (filter.ChangeType.HasValue)
                filters.Add(filterBuilder.Eq(x => x.ChangeType, filter.ChangeType.Value));
            if (filter.FromDate.HasValue)
                filters.Add(filterBuilder.Gte(x => x.ChangedDate, filter.FromDate.Value));
            if (filter.ToDate.HasValue)
                filters.Add(filterBuilder.Lte(x => x.ChangedDate, filter.ToDate.Value));
            if (!string.IsNullOrEmpty(filter.ChangedBy))
                filters.Add(filterBuilder.Eq(x => x.ChangedBy, filter.ChangedBy));
            if (filter.IsTemporary.HasValue)
                filters.Add(filterBuilder.Eq(x => x.IsTemporary, filter.IsTemporary.Value));
            if (filter.IsExpired.HasValue && filter.IsExpired.Value)
                filters.Add(filterBuilder.Lt(x => x.ExpiryDate, DateTime.UtcNow));

            var finalFilter = filters.Any() ? filterBuilder.And(filters) : filterBuilder.Empty;

            Expression<Func<LimitHistory, object>> sortExpression = sortBy?.ToLower() switch
            {
                "limithistoryid" => x => x.LimitHistoryId,
                "customerid" => x => x.CustomerId,
                "accountid" => x => x.AccountId,
                "changetype" => x => x.ChangeType,
                "previouslimit" => x => x.PreviousLimit,
                "newlimit" => x => x.NewLimit,
                "changeddate" => x => x.ChangedDate,
                "effectivedate" => x => x.EffectiveDate,
                "expirydate" => x => x.ExpiryDate,
                _ => x => x.ChangedDate
            };

            var sortDefinition = sortDescending
                ? Builders<LimitHistory>.Sort.Descending(sortExpression)
                : Builders<LimitHistory>.Sort.Ascending(sortExpression);

            var totalCount = await _collection.CountDocumentsAsync(finalFilter);
            var items = await _collection
                .Find(finalFilter)
                .Sort(sortDefinition)
                .Skip((pageNumber - 1) * pageSize)
                .Limit(pageSize)
                .ToListAsync();

            return new PaginatedResult<LimitHistory>
            {
                Data = items,
                TotalCount = (int)totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }

        public async Task<bool> UpdateExpiryDateAsync(string limitHistoryId, DateTime newExpiryDate, string updatedBy)
        {
            var filter = Builders<LimitHistory>.Filter.Eq(x => x.LimitHistoryId, limitHistoryId);
            var update = Builders<LimitHistory>.Update
                .Set(x => x.ExpiryDate, newExpiryDate)
                .Set(x => x.UpdatedAt, DateTime.UtcNow)
                .Set(x => x.UpdatedBy, updatedBy);

            var result = await _collection.UpdateOneAsync(filter, update);
            return result.ModifiedCount > 0;
        }

        public async Task<int> BulkExpireTemporaryLimitsAsync(DateTime asOfDate)
        {
            var filter = Builders<LimitHistory>.Filter.And(
                Builders<LimitHistory>.Filter.Eq(x => x.IsTemporary, true),
                Builders<LimitHistory>.Filter.Lt(x => x.ExpiryDate, asOfDate),
                Builders<LimitHistory>.Filter.Gt(x => x.ExpiryDate, DateTime.MinValue)
            );

            var update = Builders<LimitHistory>.Update
                .Set(x => x.IsTemporary, false)
                .Set(x => x.UpdatedAt, DateTime.UtcNow)
                .Set(x => x.UpdatedBy, "system");

            var result = await _collection.UpdateManyAsync(filter, update);
            return (int)result.ModifiedCount;
        }

        private string GenerateLimitHistoryId()
        {
            return $"LH-{DateTime.Now:yyyyMMdd}-{Guid.NewGuid().ToString().Substring(0, 8).ToUpper()}";
        }
    }
}