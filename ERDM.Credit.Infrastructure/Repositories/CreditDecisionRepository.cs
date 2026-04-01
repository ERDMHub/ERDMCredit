using ERDM.Core;
using ERDM.Credit.Domain.Entities;
using ERDM.Credit.Domain.Enums;
using ERDM.Credit.Domain.Interfaces;
using ERDMCore.Infrastructure.MongoDB.Settings;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System.Linq.Expressions;


namespace ERDM.Credit.Infrastructure.Repositories
{
    public class CreditDecisionRepository : MongoRepository<CreditDecision>, ICreditDecisionRepository
    {
        public CreditDecisionRepository(IMongoDatabase database, IOptions<MongoDbSettings> settings, ILogger<CreditDecisionRepository> logger)
            : base(database, settings, logger)
        {
            CreateIndexes();
        }

        private void CreateIndexes()
        {
            var indexModels = new List<CreateIndexModel<CreditDecision>>
            {
                new CreateIndexModel<CreditDecision>(
                    Builders<CreditDecision>.IndexKeys.Ascending(x => x.DecisionId),
                    new CreateIndexOptions { Unique = true, Name = "idx_decision_id" }),

                new CreateIndexModel<CreditDecision>(
                    Builders<CreditDecision>.IndexKeys.Ascending(x => x.ApplicationId),
                    new CreateIndexOptions { Unique = true, Name = "idx_application_id" }),

                new CreateIndexModel<CreditDecision>(
                    Builders<CreditDecision>.IndexKeys.Ascending(x => x.CustomerId),
                    new CreateIndexOptions { Name = "idx_customer_id" }),

                new CreateIndexModel<CreditDecision>(
                    Builders<CreditDecision>.IndexKeys.Combine(
                        Builders<CreditDecision>.IndexKeys.Ascending(x => x.DecisionType),
                        Builders<CreditDecision>.IndexKeys.Descending(x => x.DecisionDate)),
                    new CreateIndexOptions { Name = "idx_type_date" }),

                new CreateIndexModel<CreditDecision>(
                    Builders<CreditDecision>.IndexKeys.Ascending(x => x.Status),
                    new CreateIndexOptions { Name = "idx_status" }),

                new CreateIndexModel<CreditDecision>(
                    Builders<CreditDecision>.IndexKeys.Ascending(x => x.RiskGrade),
                    new CreateIndexOptions { Name = "idx_risk_grade" })
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

        public override async Task<CreditDecision> AddAsync(CreditDecision entity, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(entity.DecisionId))
                entity.DecisionId = $"DEC-{DateTime.Now:yyyyMMdd}-{Guid.NewGuid().ToString().Substring(0, 8).ToUpper()}";

            if (entity.DecisionDate == default)
                entity.DecisionDate = DateTime.UtcNow;

            entity.Status = DecisionStatus.Pending;

            return await base.AddAsync(entity, cancellationToken);
        }

        public async Task<CreditDecision?> GetByDecisionIdAsync(string decisionId)
        {
            var filter = Builders<CreditDecision>.Filter.Eq(x => x.DecisionId, decisionId);
            return await _collection.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<CreditDecision?> GetByApplicationIdAsync(string applicationId)
        {
            var filter = Builders<CreditDecision>.Filter.Eq(x => x.ApplicationId, applicationId);
            return await _collection.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<CreditDecision>> GetByCustomerIdAsync(string customerId)
        {
            var filter = Builders<CreditDecision>.Filter.Eq(x => x.CustomerId, customerId);
            var sort = Builders<CreditDecision>.Sort.Descending(x => x.DecisionDate);
            return await _collection.Find(filter).Sort(sort).ToListAsync();
        }

        public async Task<IEnumerable<CreditDecision>> GetByDecisionTypeAsync(DecisionType decisionType)
        {
            var filter = Builders<CreditDecision>.Filter.Eq(x => x.DecisionType, decisionType);
            var sort = Builders<CreditDecision>.Sort.Descending(x => x.DecisionDate);
            return await _collection.Find(filter).Sort(sort).ToListAsync();
        }

        public async Task<IEnumerable<CreditDecision>> GetByStatusAsync(DecisionStatus status)
        {
            var filter = Builders<CreditDecision>.Filter.Eq(x => x.Status, status);
            var sort = Builders<CreditDecision>.Sort.Descending(x => x.DecisionDate);
            return await _collection.Find(filter).Sort(sort).ToListAsync();
        }

        public async Task<IEnumerable<CreditDecision>> GetPendingApprovalsAsync()
        {
            var filter = Builders<CreditDecision>.Filter.And(
                Builders<CreditDecision>.Filter.In(x => x.Status, new[] { DecisionStatus.Pending }),
                Builders<CreditDecision>.Filter.Eq(x => x.DecisionType, DecisionType.Approved)
            );
            return await _collection.Find(filter).ToListAsync();
        }

        public async Task<IEnumerable<CreditDecision>> GetExpiredCounterOffersAsync()
        {
            var filter = Builders<CreditDecision>.Filter.And(
                Builders<CreditDecision>.Filter.Eq(x => x.IsCounterOffer, true),
                Builders<CreditDecision>.Filter.Eq(x => x.Status, DecisionStatus.Pending),
                Builders<CreditDecision>.Filter.Lt(x => x.CounterOfferExpiryDate, DateTime.UtcNow)
            );
            return await _collection.Find(filter).ToListAsync();
        }

        public async Task<decimal> GetTotalApprovedAmountByCustomerAsync(string customerId)
        {
            var filter = Builders<CreditDecision>.Filter.And(
                Builders<CreditDecision>.Filter.Eq(x => x.CustomerId, customerId),
                Builders<CreditDecision>.Filter.Eq(x => x.DecisionType, DecisionType.Approved),
                Builders<CreditDecision>.Filter.In(x => x.Status, new[] { DecisionStatus.Completed, DecisionStatus.Accepted })
            );

            var result = await _collection.Aggregate()
                .Match(filter)
                .Group(x => x.CustomerId, g => new { Total = g.Sum(x => x.ApprovedAmount ?? 0) })
                .FirstOrDefaultAsync();

            return result?.Total ?? 0;
        }

        public async Task<Dictionary<string, int>> GetDecisionStatisticsAsync(DateTime? fromDate = null, DateTime? toDate = null)
        {
            var filterBuilder = Builders<CreditDecision>.Filter;
            var filters = new List<FilterDefinition<CreditDecision>>();

            if (fromDate.HasValue)
                filters.Add(filterBuilder.Gte(x => x.DecisionDate, fromDate.Value));
            if (toDate.HasValue)
                filters.Add(filterBuilder.Lte(x => x.DecisionDate, toDate.Value));

            var finalFilter = filters.Any() ? filterBuilder.And(filters) : filterBuilder.Empty;

            var result = await _collection.Aggregate()
                .Match(finalFilter)
                .Group(x => x.DecisionType.ToString(), g => new { Type = g.Key, Count = g.Count() })
                .ToListAsync();

            return result.ToDictionary(x => x.Type, x => x.Count);
        }

        public async Task<PaginatedResult<CreditDecision>> GetPaginatedWithFiltersAsync(
            CreditDecisionFilter filter,
            int pageNumber,
            int pageSize,
            string sortBy = "DecisionDate",
            bool sortDescending = true)
        {
            var filterBuilder = Builders<CreditDecision>.Filter;
            var filters = new List<FilterDefinition<CreditDecision>>();

            if (!string.IsNullOrEmpty(filter.ApplicationId))
                filters.Add(filterBuilder.Eq(x => x.ApplicationId, filter.ApplicationId));
            if (!string.IsNullOrEmpty(filter.CustomerId))
                filters.Add(filterBuilder.Eq(x => x.CustomerId, filter.CustomerId));
            if (filter.DecisionType.HasValue)
                filters.Add(filterBuilder.Eq(x => x.DecisionType, filter.DecisionType.Value));
            if (filter.Status.HasValue)
                filters.Add(filterBuilder.Eq(x => x.Status, filter.Status.Value));
            if (filter.FromDate.HasValue)
                filters.Add(filterBuilder.Gte(x => x.DecisionDate, filter.FromDate.Value));
            if (filter.ToDate.HasValue)
                filters.Add(filterBuilder.Lte(x => x.DecisionDate, filter.ToDate.Value));
            if (!string.IsNullOrEmpty(filter.DecisionBy))
                filters.Add(filterBuilder.Eq(x => x.DecisionBy, filter.DecisionBy));
            if (!string.IsNullOrEmpty(filter.RiskGrade))
                filters.Add(filterBuilder.Eq(x => x.RiskGrade, filter.RiskGrade));
            if (filter.IsCounterOffer.HasValue)
                filters.Add(filterBuilder.Eq(x => x.IsCounterOffer, filter.IsCounterOffer.Value));

            var finalFilter = filters.Any() ? filterBuilder.And(filters) : filterBuilder.Empty;

            Expression<Func<CreditDecision, object>> sortExpression = sortBy?.ToLower() switch
            {
                "decisionid" => x => x.DecisionId,
                "applicationid" => x => x.ApplicationId,
                "decisiontype" => x => x.DecisionType,
                "status" => x => x.Status,
                "approvedamount" => x => x.ApprovedAmount,
                "decisiondate" => x => x.DecisionDate,
                _ => x => x.DecisionDate
            };

            var sortDefinition = sortDescending
                ? Builders<CreditDecision>.Sort.Descending(sortExpression)
                : Builders<CreditDecision>.Sort.Ascending(sortExpression);

            var totalCount = await _collection.CountDocumentsAsync(finalFilter);
            var items = await _collection
                .Find(finalFilter)
                .Sort(sortDefinition)
                .Skip((pageNumber - 1) * pageSize)
                .Limit(pageSize)
                .ToListAsync();

            return new PaginatedResult<CreditDecision>
            {
                Data = items,
                TotalCount = (int)totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }

        public async Task<bool> UpdateDecisionStatusAsync(string decisionId, DecisionStatus newStatus, string updatedBy)
        {
            var filter = Builders<CreditDecision>.Filter.Eq(x => x.DecisionId, decisionId);
            var update = Builders<CreditDecision>.Update
                .Set(x => x.Status, newStatus)
                .Set(x => x.UpdatedAt, DateTime.UtcNow)
                .Set(x => x.UpdatedBy, updatedBy);

            var result = await _collection.UpdateOneAsync(filter, update);
            return result.ModifiedCount > 0;
        }

        public async Task<bool> AcceptCounterOfferAsync(string decisionId, string acceptedBy)
        {
            var filter = Builders<CreditDecision>.Filter.Eq(x => x.DecisionId, decisionId);
            var update = Builders<CreditDecision>.Update
                .Set(x => x.Status, DecisionStatus.Accepted)
                .Set(x => x.DecisionType, DecisionType.Approved)
                .Set(x => x.UpdatedAt, DateTime.UtcNow)
                .Set(x => x.UpdatedBy, acceptedBy);

            var result = await _collection.UpdateOneAsync(filter, update);
            return result.ModifiedCount > 0;
        }

        public async Task<bool> UpdateConditionsAsync(string decisionId, List<UnderwritingCondition> conditions)
        {
            var filter = Builders<CreditDecision>.Filter.Eq(x => x.DecisionId, decisionId);
            var update = Builders<CreditDecision>.Update
                .Set(x => x.Conditions, conditions)
                .Set(x => x.UpdatedAt, DateTime.UtcNow);

            var result = await _collection.UpdateOneAsync(filter, update);
            return result.ModifiedCount > 0;
        }
    }
}
