
namespace ERDM.Credit.Contracts.DTOs.UnderwritingRuleDtos
{
    public class UnderwritingRuleResponseDto
    {
        public string Id { get; set; } = string.Empty;
        public string RuleId { get; set; } = string.Empty;
        public string RuleName { get; set; } = string.Empty;
        public string RuleCode { get; set; } = string.Empty;
        public string RuleType { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public int Priority { get; set; }
        public string Condition { get; set; } = string.Empty;
        public string ConditionExpression { get; set; } = string.Empty;
        public List<RuleActionDto> Actions { get; set; } = new();
        public RuleOutcomeDto TrueOutcome { get; set; } = new();
        public RuleOutcomeDto? FalseOutcome { get; set; }
        public string Status { get; set; } = string.Empty;
        public int RuleVersion { get; set; }
        public DateTime EffectiveFrom { get; set; }
        public DateTime? EffectiveTo { get; set; }
        public string Description { get; set; } = string.Empty;
        public decimal SuccessRate { get; set; }
        public int ExecutionCount { get; set; }
        public DateTime? LastExecutedAt { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
        public string? ApprovedBy { get; set; }
        public DateTime? ApprovedAt { get; set; }
        public List<string> DependsOnRules { get; set; } = new();
        public UnderwritingRuleMetadataDto Metadata { get; set; } = new();
        public List<string> Tags { get; set; } = new();
        public DateTime CreatedAt { get; set; }
    }

    

    
  

   

    

    public class RuleOutcomeDto
    {
        public string OutcomeType { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public Dictionary<string, object> Data { get; set; } = new();
        public List<string> NextRules { get; set; } = new();
    }

   

    public class UpdateRuleOutcomeDto
    {
        public string OutcomeType { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public Dictionary<string, object>? Data { get; set; }
        public List<string>? NextRules { get; set; }
    }

    public class UnderwritingRuleMetadataDto
    {
        public string? Source { get; set; }
        public string? Owner { get; set; }
        public string? Department { get; set; }
        public string? DeactivationReason { get; set; }
        public string? RejectionReason { get; set; }
        public string? LastFailureReason { get; set; }
        public DateTime? LastFailureAt { get; set; }
        public Dictionary<string, object>? AdditionalData { get; set; }
    }

    public class UnderwritingRuleQueryDto
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string? RuleName { get; set; }
        public string? RuleCode { get; set; }
        public string? RuleType { get; set; }
        public string? Category { get; set; }
        public string? Status { get; set; }
        public bool? IsActive { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public string SortBy { get; set; } = "Priority";
        public bool SortDescending { get; set; } = false;
    }

    public class ExecuteRuleRequestDto
    {
        public string ApplicationId { get; set; } = string.Empty;
        public Dictionary<string, object> InputData { get; set; } = new();
        public string? ExecutionContext { get; set; }
    }

    public class RuleExecutionResultDto
    {
        public string RuleId { get; set; } = string.Empty;
        public string RuleName { get; set; } = string.Empty;
        public bool IsTrue { get; set; }
        public RuleOutcomeDto Outcome { get; set; } = new();
        public List<RuleActionDto> ActionsExecuted { get; set; } = new();
        public TimeSpan ExecutionTime { get; set; }
        public DateTime ExecutedAt { get; set; }
    }

    public class RuleValidationResultDto
    {
        public bool IsValid { get; set; }
        public List<string> Errors { get; set; } = new();
        public List<string> Warnings { get; set; } = new();
        public string? CompiledExpression { get; set; }
    }

    public class UnderwritingRuleStatisticsDto
    {
        public int TotalRules { get; set; }
        public int ActiveRules { get; set; }
        public int InactiveRules { get; set; }
        public int DraftRules { get; set; }
        public int ApprovedRules { get; set; }
        public Dictionary<string, int> RulesByType { get; set; } = new();
        public Dictionary<string, int> RulesByCategory { get; set; } = new();
        public decimal AverageSuccessRate { get; set; }
        public int TotalExecutions { get; set; }
        public List<RuleExecutionStatsDto> TopPerformingRules { get; set; } = new();
        public List<RuleExecutionStatsDto> UnderperformingRules { get; set; } = new();
        public DateTime AsOfDate { get; set; }
    }

    public class RuleExecutionStatsDto
    {
        public string RuleId { get; set; } = string.Empty;
        public string RuleName { get; set; } = string.Empty;
        public int ExecutionCount { get; set; }
        public decimal SuccessRate { get; set; }
        public DateTime? LastExecutedAt { get; set; }
    }

    public class BulkRuleOperationDto
    {
        public List<string> RuleIds { get; set; } = new();
        public string Operation { get; set; } = string.Empty; // Activate, Deactivate, Delete
        public string PerformedBy { get; set; } = string.Empty;
        public string? Reason { get; set; }
    }

    public class BulkRuleOperationResponseDto
    {
        public int TotalProcessed { get; set; }
        public int Successful { get; set; }
        public int Failed { get; set; }
        public List<BulkRuleOperationErrorDto> Errors { get; set; } = new();
        public DateTime ProcessedAt { get; set; }
    }

    public class BulkRuleOperationErrorDto
    {
        public string RuleId { get; set; } = string.Empty;
        public string ErrorCode { get; set; } = string.Empty;
        public string ErrorMessage { get; set; } = string.Empty;
    }
}