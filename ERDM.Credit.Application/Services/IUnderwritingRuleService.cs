using ERDM.Credit.Contracts.DTOs.UnderwritingRuleDtos;
using ERDM.Credit.Contracts.Wrapper;

namespace ERDM.Credit.Application.Services
{
    public interface IUnderwritingRuleService
    {
        // Create Operations
        Task<ApiResponse<UnderwritingRuleResponseDto>> CreateAsync(CreateUnderwritingRuleDto dto);

        // Read Operations
        Task<ApiResponse<UnderwritingRuleResponseDto>> GetByIdAsync(string id);
        Task<ApiResponse<UnderwritingRuleResponseDto>> GetByRuleIdAsync(string ruleId);
        Task<ApiResponse<UnderwritingRuleResponseDto>> GetByRuleCodeAsync(string ruleCode);
        Task<ApiResponse<PaginatedResponse<UnderwritingRuleResponseDto>>> GetAllAsync(UnderwritingRuleQueryDto query);
        Task<ApiResponse<List<UnderwritingRuleResponseDto>>> GetActiveRulesAsync();
        Task<ApiResponse<List<UnderwritingRuleResponseDto>>> GetRulesByTypeAsync(string ruleType);
        Task<ApiResponse<List<UnderwritingRuleResponseDto>>> GetRulesByCategoryAsync(string category);

        // Update Operations
        Task<ApiResponse<UnderwritingRuleResponseDto>> UpdateRuleAsync(string id, UpdateUnderwritingRuleDto dto);
        Task<ApiResponse<UnderwritingRuleResponseDto>> ActivateRuleAsync(string id, string activatedBy);
        Task<ApiResponse<UnderwritingRuleResponseDto>> DeactivateRuleAsync(string id, string deactivatedBy, string reason);
        Task<ApiResponse<UnderwritingRuleResponseDto>> ApproveRuleAsync(string id, string approvedBy);
        Task<ApiResponse<UnderwritingRuleResponseDto>> RejectRuleAsync(string id, string rejectedBy, string reason);

        // Execution Operations
        Task<ApiResponse<RuleExecutionResultDto>> ExecuteRuleAsync(string ruleId, ExecuteRuleRequestDto request);
        Task<ApiResponse<List<RuleExecutionResultDto>>> ExecuteRuleSetAsync(List<string> ruleIds, ExecuteRuleRequestDto request);

        // Validation Operations
        Task<ApiResponse<RuleValidationResultDto>> ValidateRuleAsync(string id);
        Task<ApiResponse<bool>> ValidateRuleDependenciesAsync(string id);

        // Statistics and Reports
        Task<ApiResponse<UnderwritingRuleStatisticsDto>> GetStatisticsAsync();
        Task<ApiResponse<List<RuleExecutionStatsDto>>> GetTopPerformingRulesAsync(int count = 10);
        Task<ApiResponse<List<RuleExecutionStatsDto>>> GetUnderperformingRulesAsync(int count = 10);

        // Bulk Operations
        Task<ApiResponse<BulkRuleOperationResponseDto>> BulkActivateRulesAsync(BulkRuleOperationDto dto);
        Task<ApiResponse<BulkRuleOperationResponseDto>> BulkDeactivateRulesAsync(BulkRuleOperationDto dto);
    }
}