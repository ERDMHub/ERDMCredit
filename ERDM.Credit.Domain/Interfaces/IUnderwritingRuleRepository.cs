using ERDM.Core;
using ERDM.Core.Interfaces;
using ERDM.Credit.Domain.Entities;
using ERDM.Credit.Domain.Enums;

namespace ERDM.Credit.Domain.Interfaces
{
    public interface IUnderwritingRuleRepository : IRepository<UnderwritingRule>
    {
        Task<UnderwritingRule?> GetByRuleIdAsync(string ruleId);
        Task<UnderwritingRule?> GetByRuleCodeAsync(string ruleCode);
        Task<IEnumerable<UnderwritingRule>> GetByRuleTypeAsync(RuleType ruleType);
        Task<IEnumerable<UnderwritingRule>> GetByCategoryAsync(RuleCategory category);
        Task<IEnumerable<UnderwritingRule>> GetByStatusAsync(RuleStatus status);
        Task<IEnumerable<UnderwritingRule>> GetActiveRulesAsync();
        Task<IEnumerable<UnderwritingRule>> GetRulesByPriorityAsync(int minPriority, int maxPriority);
        Task<IEnumerable<UnderwritingRule>> GetRulesDependingOnAsync(string ruleId);
        Task<IEnumerable<UnderwritingRule>> GetRulesForExecutionAsync();
        Task<Dictionary<string, int>> GetRuleStatisticsAsync();
        Task<PaginatedResult<UnderwritingRule>> GetPaginatedWithFiltersAsync(UnderwritingRuleFilter filter, int pageNumber, int pageSize, string sortBy = "Priority", bool sortDescending = false);
        Task<bool> UpdateRuleStatusAsync(string ruleId, RuleStatus newStatus, string updatedBy);
        Task<bool> IncrementExecutionCountAsync(string ruleId, bool succeeded);
        Task<int> BulkUpdateStatusAsync(List<string> ruleIds, RuleStatus newStatus, string updatedBy);
        Task<bool> ValidateRuleDependenciesAsync(string ruleId, List<string> dependencyIds);
    }
}