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
    public class UnderwritingRuleRepository : MongoRepository<UnderwritingRule>, IUnderwritingRuleRepository
    {
        public UnderwritingRuleRepository(IMongoDatabase database, IOptions<MongoDbSettings> settings, ILogger<UnderwritingRuleRepository> logger)
            : base(database, settings, logger)
        {
            CreateIndexes();
        }

        private void CreateIndexes()
        {
            var indexModels = new List<CreateIndexModel<UnderwritingRule>>
            {
                new CreateIndexModel<UnderwritingRule>(
                    Builders<UnderwritingRule>.IndexKeys.Ascending(x => x.RuleId),
                    new CreateIndexOptions { Unique = true, Name = "idx_rule_id" }),

                new CreateIndexModel<UnderwritingRule>(
                    Builders<UnderwritingRule>.IndexKeys.Ascending(x => x.RuleCode),
                    new CreateIndexOptions { Unique = true, Name = "idx_rule_code" }),

                new CreateIndexModel<UnderwritingRule>(
                    Builders<UnderwritingRule>.IndexKeys.Ascending(x => x.RuleName),
                    new CreateIndexOptions { Name = "idx_rule_name" }),

                new CreateIndexModel<UnderwritingRule>(
                    Builders<UnderwritingRule>.IndexKeys.Combine(
                        Builders<UnderwritingRule>.IndexKeys.Ascending(x => x.RuleType),
                        Builders<UnderwritingRule>.IndexKeys.Ascending(x => x.Status)),
                    new CreateIndexOptions { Name = "idx_type_status" }),

                new CreateIndexModel<UnderwritingRule>(
                    Builders<UnderwritingRule>.IndexKeys.Ascending(x => x.Category),
                    new CreateIndexOptions { Name = "idx_category" }),

                new CreateIndexModel<UnderwritingRule>(
                    Builders<UnderwritingRule>.IndexKeys.Ascending(x => x.Priority),
                    new CreateIndexOptions { Name = "idx_priority" }),

                new CreateIndexModel<UnderwritingRule>(
                    Builders<UnderwritingRule>.IndexKeys.Ascending(x => x.Status),
                    new CreateIndexOptions { Name = "idx_status" }),

                new CreateIndexModel<UnderwritingRule>(
                    Builders<UnderwritingRule>.IndexKeys.Ascending(x => x.EffectiveFrom),
                    new CreateIndexOptions { Name = "idx_effective_from" })
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

        public override async Task<UnderwritingRule> AddAsync(UnderwritingRule entity, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(entity.RuleId))
                entity.RuleId = GenerateRuleId();

            if (entity.CreatedAt == default)
                entity.CreatedAt = DateTime.UtcNow;

            if (string.IsNullOrEmpty(entity.CreatedBy))
                entity.CreatedBy = "system";

            if (entity.Version == 0)
                entity.Version = 1;

            if (string.IsNullOrEmpty(entity.Id))
                entity.Id = entity.RuleId;

            entity.Actions ??= new List<RuleAction>();
            entity.DependsOnRules ??= new List<string>();
            entity.Tags ??= new List<string>();
            entity.Metadata ??= new UnderwritingRuleMetadata();

            // ✅ Fix Actions
            foreach (var action in entity.Actions)
            {
                if (action.Parameters != null)
                {
                    action.Parameters = NormalizeDictionary(action.Parameters);
                }
            }

            // ✅ Fix Outcomes
            if (entity.TrueOutcome?.Data != null)
            {
                entity.TrueOutcome.Data = NormalizeDictionary(entity.TrueOutcome.Data);
            }

            if (entity.FalseOutcome?.Data != null)
            {
                entity.FalseOutcome.Data = NormalizeDictionary(entity.FalseOutcome.Data);
            }

            return await base.AddAsync(entity, cancellationToken);
        }

        public async Task<UnderwritingRule?> GetByRuleIdAsync(string ruleId)
        {
            var filter = Builders<UnderwritingRule>.Filter.Eq(x => x.RuleId, ruleId);
            return await _collection.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<UnderwritingRule?> GetByRuleCodeAsync(string ruleCode)
        {
            var filter = Builders<UnderwritingRule>.Filter.Eq(x => x.RuleCode, ruleCode);
            return await _collection.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<UnderwritingRule>> GetByRuleTypeAsync(RuleType ruleType)
        {
            var filter = Builders<UnderwritingRule>.Filter.Eq(x => x.RuleType, ruleType);
            var sort = Builders<UnderwritingRule>.Sort.Ascending(x => x.Priority);
            return await _collection.Find(filter).Sort(sort).ToListAsync();
        }

        public async Task<IEnumerable<UnderwritingRule>> GetByCategoryAsync(RuleCategory category)
        {
            var filter = Builders<UnderwritingRule>.Filter.Eq(x => x.Category, category);
            var sort = Builders<UnderwritingRule>.Sort.Ascending(x => x.Priority);
            return await _collection.Find(filter).Sort(sort).ToListAsync();
        }

        public async Task<IEnumerable<UnderwritingRule>> GetByStatusAsync(RuleStatus status)
        {
            var filter = Builders<UnderwritingRule>.Filter.Eq(x => x.Status, status);
            var sort = Builders<UnderwritingRule>.Sort.Ascending(x => x.Priority);
            return await _collection.Find(filter).Sort(sort).ToListAsync();
        }

        public async Task<IEnumerable<UnderwritingRule>> GetActiveRulesAsync()
        {
            var filter = Builders<UnderwritingRule>.Filter.And(
                Builders<UnderwritingRule>.Filter.Eq(x => x.Status, RuleStatus.Active),
                Builders<UnderwritingRule>.Filter.Lte(x => x.EffectiveFrom, DateTime.UtcNow),
                Builders<UnderwritingRule>.Filter.Or(
                    Builders<UnderwritingRule>.Filter.Exists(x => x.EffectiveTo, false),
                    Builders<UnderwritingRule>.Filter.Gt(x => x.EffectiveTo, DateTime.UtcNow)
                )
            );
            var sort = Builders<UnderwritingRule>.Sort.Ascending(x => x.Priority);
            return await _collection.Find(filter).Sort(sort).ToListAsync();
        }

        public async Task<IEnumerable<UnderwritingRule>> GetRulesByPriorityAsync(int minPriority, int maxPriority)
        {
            var filter = Builders<UnderwritingRule>.Filter.And(
                Builders<UnderwritingRule>.Filter.Gte(x => x.Priority, minPriority),
                Builders<UnderwritingRule>.Filter.Lte(x => x.Priority, maxPriority)
            );
            var sort = Builders<UnderwritingRule>.Sort.Ascending(x => x.Priority);
            return await _collection.Find(filter).Sort(sort).ToListAsync();
        }

        public async Task<IEnumerable<UnderwritingRule>> GetRulesDependingOnAsync(string ruleId)
        {
            var filter = Builders<UnderwritingRule>.Filter.AnyEq(x => x.DependsOnRules, ruleId);
            return await _collection.Find(filter).ToListAsync();
        }

        public async Task<IEnumerable<UnderwritingRule>> GetRulesForExecutionAsync()
        {
            return await GetActiveRulesAsync();
        }

        public async Task<Dictionary<string, int>> GetRuleStatisticsAsync()
        {
            var result = await _collection.Aggregate()
                .Group(x => x.Status.ToString(), g => new { Status = g.Key, Count = g.Count() })
                .ToListAsync();

            return result.ToDictionary(x => x.Status, x => x.Count);
        }

        public async Task<PaginatedResult<UnderwritingRule>> GetPaginatedWithFiltersAsync(
            UnderwritingRuleFilter filter,
            int pageNumber,
            int pageSize,
            string sortBy = "Priority",
            bool sortDescending = false)
        {
            var filterBuilder = Builders<UnderwritingRule>.Filter;
            var filters = new List<FilterDefinition<UnderwritingRule>>();

            if (!string.IsNullOrEmpty(filter.RuleName))
                filters.Add(filterBuilder.Regex(x => x.RuleName, new MongoDB.Bson.BsonRegularExpression(filter.RuleName, "i")));
            if (!string.IsNullOrEmpty(filter.RuleCode))
                filters.Add(filterBuilder.Eq(x => x.RuleCode, filter.RuleCode));
            if (filter.RuleType.HasValue)
                filters.Add(filterBuilder.Eq(x => x.RuleType, filter.RuleType.Value));
            if (filter.Category.HasValue)
                filters.Add(filterBuilder.Eq(x => x.Category, filter.Category.Value));
            if (filter.Status.HasValue)
                filters.Add(filterBuilder.Eq(x => x.Status, filter.Status.Value));
            if (!string.IsNullOrEmpty(filter.CreatedBy))
                filters.Add(filterBuilder.Eq(x => x.CreatedBy, filter.CreatedBy));
            if (filter.FromDate.HasValue)
                filters.Add(filterBuilder.Gte(x => x.CreatedAt, filter.FromDate.Value));
            if (filter.ToDate.HasValue)
                filters.Add(filterBuilder.Lte(x => x.CreatedAt, filter.ToDate.Value));
            if (filter.MinPriority.HasValue)
                filters.Add(filterBuilder.Gte(x => x.Priority, filter.MinPriority.Value));
            if (filter.MaxPriority.HasValue)
                filters.Add(filterBuilder.Lte(x => x.Priority, filter.MaxPriority.Value));
            if (filter.IsActive.HasValue && filter.IsActive.Value)
                filters.Add(filterBuilder.Eq(x => x.Status, RuleStatus.Active));

            var finalFilter = filters.Any() ? filterBuilder.And(filters) : filterBuilder.Empty;

            Expression<Func<UnderwritingRule, object>> sortExpression = sortBy?.ToLower() switch
            {
                "ruleid" => x => x.RuleId,
                "rulename" => x => x.RuleName,
                "rulecode" => x => x.RuleCode,
                "ruletype" => x => x.RuleType,
                "category" => x => x.Category,
                "priority" => x => x.Priority,
                "status" => x => x.Status,
                "successrate" => x => x.SuccessRate,
                "executioncount" => x => x.ExecutionCount,
                "createdat" => x => x.CreatedAt,
                _ => x => x.Priority
            };

            var sortDefinition = sortDescending
                ? Builders<UnderwritingRule>.Sort.Descending(sortExpression)
                : Builders<UnderwritingRule>.Sort.Ascending(sortExpression);

            var totalCount = await _collection.CountDocumentsAsync(finalFilter);
            var items = await _collection
                .Find(finalFilter)
                .Sort(sortDefinition)
                .Skip((pageNumber - 1) * pageSize)
                .Limit(pageSize)
                .ToListAsync();

            return new PaginatedResult<UnderwritingRule>
            {
                Data = items,
                TotalCount = (int)totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }

        public async Task<bool> UpdateRuleStatusAsync(string ruleId, RuleStatus newStatus, string updatedBy)
        {
            var filter = Builders<UnderwritingRule>.Filter.Eq(x => x.RuleId, ruleId);
            var update = Builders<UnderwritingRule>.Update
                .Set(x => x.Status, newStatus)
                .Set(x => x.UpdatedAt, DateTime.UtcNow)
                .Set(x => x.UpdatedBy, updatedBy);

            var result = await _collection.UpdateOneAsync(filter, update);
            return result.ModifiedCount > 0;
        }

        public async Task<bool> IncrementExecutionCountAsync(string ruleId, bool succeeded)
        {
            var filter = Builders<UnderwritingRule>.Filter.Eq(x => x.RuleId, ruleId);
            var update = Builders<UnderwritingRule>.Update
                .Inc(x => x.ExecutionCount, 1)
                .Set(x => x.LastExecutedAt, DateTime.UtcNow);

            var result = await _collection.UpdateOneAsync(filter, update);
            return result.ModifiedCount > 0;
        }

        public async Task<int> BulkUpdateStatusAsync(List<string> ruleIds, RuleStatus newStatus, string updatedBy)
        {
            var filter = Builders<UnderwritingRule>.Filter.In(x => x.RuleId, ruleIds);
            var update = Builders<UnderwritingRule>.Update
                .Set(x => x.Status, newStatus)
                .Set(x => x.UpdatedAt, DateTime.UtcNow)
                .Set(x => x.UpdatedBy, updatedBy);

            var result = await _collection.UpdateManyAsync(filter, update);
            return (int)result.ModifiedCount;
        }

        public async Task<bool> ValidateRuleDependenciesAsync(string ruleId, List<string> dependencyIds)
        {
            // Check for circular dependencies
            var allDependencies = new HashSet<string>();
            var toCheck = new Queue<string>(dependencyIds);

            while (toCheck.Count > 0)
            {
                var currentId = toCheck.Dequeue();
                if (currentId == ruleId)
                    return false; // Circular dependency detected

                if (allDependencies.Contains(currentId))
                    continue;

                allDependencies.Add(currentId);

                var dependentRule = await GetByRuleIdAsync(currentId);
                if (dependentRule?.DependsOnRules != null)
                {
                    foreach (var dep in dependentRule.DependsOnRules)
                    {
                        toCheck.Enqueue(dep);
                    }
                }
            }

            return true;
        }

        private string GenerateRuleId()
        {
            return $"RULE-{DateTime.Now:yyyyMMdd}-{Guid.NewGuid().ToString().Substring(0, 8).ToUpper()}";
        }

        private static Dictionary<string, object> NormalizeDictionary(Dictionary<string, object> input)
        {
            return input?.ToDictionary(kvp => kvp.Key, kvp => NormalizeValue(kvp.Value))
                   ?? new Dictionary<string, object>();
        }

        private static object NormalizeValue(object value)
        {
            if (value is System.Text.Json.JsonElement element)
            {
                switch (element.ValueKind)
                {
                    case System.Text.Json.JsonValueKind.Object:
                        var dict = new Dictionary<string, object>();
                        foreach (var prop in element.EnumerateObject())
                        {
                            dict[prop.Name] = NormalizeValue(prop.Value);
                        }
                        return dict;

                    case System.Text.Json.JsonValueKind.Array:
                        var list = new List<object>();
                        foreach (var item in element.EnumerateArray())
                        {
                            list.Add(NormalizeValue(item));
                        }
                        return list;

                    case System.Text.Json.JsonValueKind.String:
                        return element.GetString();

                    case System.Text.Json.JsonValueKind.Number:
                        return element.TryGetInt64(out var l) ? l : element.GetDouble();

                    case System.Text.Json.JsonValueKind.True:
                    case System.Text.Json.JsonValueKind.False:
                        return element.GetBoolean();

                    case System.Text.Json.JsonValueKind.Null:
                        return null;

                    default:
                        return element.ToString();
                }
            }

            return value;
        }
    }
}