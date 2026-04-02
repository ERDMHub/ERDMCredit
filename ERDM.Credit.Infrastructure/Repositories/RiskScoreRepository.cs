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
    public class RiskScoreRepository : MongoRepository<RiskScore>, IRiskScoreRepository
    {
        public RiskScoreRepository(IMongoDatabase database, IOptions<MongoDbSettings> settings, ILogger<RiskScoreRepository> logger)
            : base(database, settings, logger)
        {
            CreateIndexes();
        }

        private void CreateIndexes()
        {
            var indexModels = new List<CreateIndexModel<RiskScore>>
            {
                new CreateIndexModel<RiskScore>(
                    Builders<RiskScore>.IndexKeys.Ascending(x => x.RiskScoreId),
                    new CreateIndexOptions { Unique = true, Name = "idx_risk_score_id" }),

                new CreateIndexModel<RiskScore>(
                    Builders<RiskScore>.IndexKeys.Ascending(x => x.CustomerId),
                    new CreateIndexOptions { Name = "idx_customer_id" }),

                new CreateIndexModel<RiskScore>(
                    Builders<RiskScore>.IndexKeys.Ascending(x => x.ApplicationId),
                    new CreateIndexOptions { Name = "idx_application_id" }),

                new CreateIndexModel<RiskScore>(
                    Builders<RiskScore>.IndexKeys.Ascending(x => x.AccountId),
                    new CreateIndexOptions { Name = "idx_account_id" }),

                new CreateIndexModel<RiskScore>(
                    Builders<RiskScore>.IndexKeys.Combine(
                        Builders<RiskScore>.IndexKeys.Ascending(x => x.ScoreType),
                        Builders<RiskScore>.IndexKeys.Descending(x => x.ScoringDate)),
                    new CreateIndexOptions { Name = "idx_type_date" }),

                new CreateIndexModel<RiskScore>(
                    Builders<RiskScore>.IndexKeys.Ascending(x => x.RiskCategory),
                    new CreateIndexOptions { Name = "idx_risk_category" }),

                new CreateIndexModel<RiskScore>(
                    Builders<RiskScore>.IndexKeys.Ascending(x => x.ScoreValue),
                    new CreateIndexOptions { Name = "idx_score_value" }),

                new CreateIndexModel<RiskScore>(
                    Builders<RiskScore>.IndexKeys.Ascending(x => x.ValidUntil),
                    new CreateIndexOptions { Name = "idx_valid_until" }),

                new CreateIndexModel<RiskScore>(
                    Builders<RiskScore>.IndexKeys.Ascending(x => x.NextReviewDate),
                    new CreateIndexOptions { Name = "idx_next_review_date" })
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

        public override async Task<RiskScore> AddAsync(RiskScore entity, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(entity.RiskScoreId))
                entity.RiskScoreId = GenerateRiskScoreId();

            if (entity.ScoringDate == default)
                entity.ScoringDate = DateTime.UtcNow;

            if (entity.CreatedAt == default)
                entity.CreatedAt = DateTime.UtcNow;

            if (string.IsNullOrEmpty(entity.CreatedBy))
                entity.CreatedBy = entity.ScoredBy ?? "system";

            if (entity.Version == 0)
                entity.Version = 1;

            if (string.IsNullOrEmpty(entity.Id))
                entity.Id = entity.RiskScoreId;

            entity.RiskFactors ??= new List<RiskFactor>();
            entity.Tags ??= new List<string>();
            entity.Metadata ??= new RiskScoreMetadata();

            return await base.AddAsync(entity, cancellationToken);
        }

        public async Task<RiskScore?> GetByRiskScoreIdAsync(string riskScoreId)
        {
            var filter = Builders<RiskScore>.Filter.Eq(x => x.RiskScoreId, riskScoreId);
            return await _collection.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<RiskScore>> GetByCustomerIdAsync(string customerId)
        {
            var filter = Builders<RiskScore>.Filter.Eq(x => x.CustomerId, customerId);
            var sort = Builders<RiskScore>.Sort.Descending(x => x.ScoringDate);
            return await _collection.Find(filter).Sort(sort).ToListAsync();
        }

        public async Task<RiskScore?> GetLatestByCustomerIdAsync(string customerId)
        {
            var filter = Builders<RiskScore>.Filter.Eq(x => x.CustomerId, customerId);
            var sort = Builders<RiskScore>.Sort.Descending(x => x.ScoringDate);
            return await _collection.Find(filter).Sort(sort).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<RiskScore>> GetByApplicationIdAsync(string applicationId)
        {
            var filter = Builders<RiskScore>.Filter.Eq(x => x.ApplicationId, applicationId);
            var sort = Builders<RiskScore>.Sort.Descending(x => x.ScoringDate);
            return await _collection.Find(filter).Sort(sort).ToListAsync();
        }

        public async Task<IEnumerable<RiskScore>> GetByAccountIdAsync(string accountId)
        {
            var filter = Builders<RiskScore>.Filter.Eq(x => x.AccountId, accountId);
            var sort = Builders<RiskScore>.Sort.Descending(x => x.ScoringDate);
            return await _collection.Find(filter).Sort(sort).ToListAsync();
        }

        public async Task<IEnumerable<RiskScore>> GetByScoreTypeAsync(ScoreType scoreType)
        {
            var filter = Builders<RiskScore>.Filter.Eq(x => x.ScoreType, scoreType);
            var sort = Builders<RiskScore>.Sort.Descending(x => x.ScoringDate);
            return await _collection.Find(filter).Sort(sort).ToListAsync();
        }

        public async Task<IEnumerable<RiskScore>> GetByRiskCategoryAsync(RiskCategory riskCategory)
        {
            var filter = Builders<RiskScore>.Filter.Eq(x => x.RiskCategory, riskCategory);
            var sort = Builders<RiskScore>.Sort.Descending(x => x.ScoringDate);
            return await _collection.Find(filter).Sort(sort).ToListAsync();
        }

        public async Task<IEnumerable<RiskScore>> GetValidScoresAsync()
        {
            var filter = Builders<RiskScore>.Filter.And(
                Builders<RiskScore>.Filter.Eq(x => x.IsValid, true),
                Builders<RiskScore>.Filter.Or(
                    Builders<RiskScore>.Filter.Exists(x => x.ValidUntil, false),
                    Builders<RiskScore>.Filter.Gt(x => x.ValidUntil, DateTime.UtcNow)
                )
            );
            var sort = Builders<RiskScore>.Sort.Descending(x => x.ScoringDate);
            return await _collection.Find(filter).Sort(sort).ToListAsync();
        }

        public async Task<IEnumerable<RiskScore>> GetExpiredScoresAsync(DateTime asOfDate)
        {
            var filter = Builders<RiskScore>.Filter.And(
                Builders<RiskScore>.Filter.Eq(x => x.IsValid, true),
                Builders<RiskScore>.Filter.Lt(x => x.ValidUntil, asOfDate)
            );
            return await _collection.Find(filter).ToListAsync();
        }

        public async Task<IEnumerable<RiskScore>> GetScoresForReviewAsync(DateTime reviewDate)
        {
            var filter = Builders<RiskScore>.Filter.And(
                Builders<RiskScore>.Filter.Eq(x => x.IsValid, true),
                Builders<RiskScore>.Filter.Lte(x => x.NextReviewDate, reviewDate)
            );
            var sort = Builders<RiskScore>.Sort.Ascending(x => x.NextReviewDate);
            return await _collection.Find(filter).Sort(sort).ToListAsync();
        }

        public async Task<Dictionary<string, int>> GetRiskScoreStatisticsAsync(DateTime? fromDate = null, DateTime? toDate = null)
        {
            var filterBuilder = Builders<RiskScore>.Filter;
            var filters = new List<FilterDefinition<RiskScore>>();

            if (fromDate.HasValue)
                filters.Add(filterBuilder.Gte(x => x.ScoringDate, fromDate.Value));
            if (toDate.HasValue)
                filters.Add(filterBuilder.Lte(x => x.ScoringDate, toDate.Value));

            var finalFilter = filters.Any() ? filterBuilder.And(filters) : filterBuilder.Empty;

            var result = await _collection.Aggregate()
                .Match(finalFilter)
                .Group(x => x.RiskCategory.ToString(), g => new { Category = g.Key, Count = g.Count() })
                .ToListAsync();

            return result.ToDictionary(x => x.Category, x => x.Count);
        }

        public async Task<PaginatedResult<RiskScore>> GetPaginatedWithFiltersAsync(
            RiskScoreFilter filter,
            int pageNumber,
            int pageSize,
            string sortBy = "ScoringDate",
            bool sortDescending = true)
        {
            var filterBuilder = Builders<RiskScore>.Filter;
            var filters = new List<FilterDefinition<RiskScore>>();

            if (!string.IsNullOrEmpty(filter.CustomerId))
                filters.Add(filterBuilder.Eq(x => x.CustomerId, filter.CustomerId));
            if (!string.IsNullOrEmpty(filter.ApplicationId))
                filters.Add(filterBuilder.Eq(x => x.ApplicationId, filter.ApplicationId));
            if (!string.IsNullOrEmpty(filter.AccountId))
                filters.Add(filterBuilder.Eq(x => x.AccountId, filter.AccountId));
            if (filter.ScoreType.HasValue)
                filters.Add(filterBuilder.Eq(x => x.ScoreType, filter.ScoreType.Value));
            if (filter.RiskCategory.HasValue)
                filters.Add(filterBuilder.Eq(x => x.RiskCategory, filter.RiskCategory.Value));
            if (filter.MinScore.HasValue)
                filters.Add(filterBuilder.Gte(x => x.ScoreValue, filter.MinScore.Value));
            if (filter.MaxScore.HasValue)
                filters.Add(filterBuilder.Lte(x => x.ScoreValue, filter.MaxScore.Value));
            if (filter.FromDate.HasValue)
                filters.Add(filterBuilder.Gte(x => x.ScoringDate, filter.FromDate.Value));
            if (filter.ToDate.HasValue)
                filters.Add(filterBuilder.Lte(x => x.ScoringDate, filter.ToDate.Value));
            if (filter.IsValid.HasValue)
                filters.Add(filterBuilder.Eq(x => x.IsValid, filter.IsValid.Value));
            if (filter.RequiresReview.HasValue && filter.RequiresReview.Value)
                filters.Add(filterBuilder.Lte(x => x.NextReviewDate, DateTime.UtcNow.AddDays(7)));

            var finalFilter = filters.Any() ? filterBuilder.And(filters) : filterBuilder.Empty;

            Expression<Func<RiskScore, object>> sortExpression = sortBy?.ToLower() switch
            {
                "riskscoreid" => x => x.RiskScoreId,
                "customerid" => x => x.CustomerId,
                "scoretype" => x => x.ScoreType,
                "scorevalue" => x => x.ScoreValue,
                "riskcategory" => x => x.RiskCategory,
                "scoringdate" => x => x.ScoringDate,
                "validuntil" => x => x.ValidUntil,
                "nextreviewdate" => x => x.NextReviewDate,
                _ => x => x.ScoringDate
            };

            var sortDefinition = sortDescending
                ? Builders<RiskScore>.Sort.Descending(sortExpression)
                : Builders<RiskScore>.Sort.Ascending(sortExpression);

            var totalCount = await _collection.CountDocumentsAsync(finalFilter);
            var items = await _collection
                .Find(finalFilter)
                .Sort(sortDefinition)
                .Skip((pageNumber - 1) * pageSize)
                .Limit(pageSize)
                .ToListAsync();

            return new PaginatedResult<RiskScore>
            {
                Data = items,
                TotalCount = (int)totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }

        public async Task<bool> InvalidateScoreAsync(string riskScoreId, string invalidatedBy, string reason)
        {
            var filter = Builders<RiskScore>.Filter.Eq(x => x.RiskScoreId, riskScoreId);
            var update = Builders<RiskScore>.Update
                .Set(x => x.IsValid, false)
                .Set(x => x.ValidUntil, null)
                .Set(x => x.ReviewNotes, reason)
                .Set(x => x.UpdatedAt, DateTime.UtcNow)
                .Set(x => x.UpdatedBy, invalidatedBy);

            var result = await _collection.UpdateOneAsync(filter, update);
            return result.ModifiedCount > 0;
        }

        public async Task<int> BulkUpdateValidityAsync(DateTime asOfDate)
        {
            var filter = Builders<RiskScore>.Filter.And(
                Builders<RiskScore>.Filter.Eq(x => x.IsValid, true),
                Builders<RiskScore>.Filter.Lt(x => x.ValidUntil, asOfDate)
            );

            var update = Builders<RiskScore>.Update
                .Set(x => x.IsValid, false)
                .Set(x => x.UpdatedAt, DateTime.UtcNow)
                .Set(x => x.UpdatedBy, "system");

            var result = await _collection.UpdateManyAsync(filter, update);
            return (int)result.ModifiedCount;
        }

        private string GenerateRiskScoreId()
        {
            return $"RS-{DateTime.Now:yyyyMMdd}-{Guid.NewGuid().ToString().Substring(0, 8).ToUpper()}";
        }
    }
}