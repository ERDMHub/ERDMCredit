using ERDM.Credit.Contracts.DTOs.CreditDecisionDtos;
using ERDM.Credit.Contracts.Wrapper;


namespace ERDM.Credit.Application.Services
{
    public interface ICreditDecisionService
    {
        // Create Operations
        Task<ApiResponse<CreditDecisionResponseDto>> CreateAsync(CreateCreditDecisionDto dto);

        // Read Operations
        Task<ApiResponse<CreditDecisionResponseDto>> GetByIdAsync(string id);
        Task<ApiResponse<CreditDecisionResponseDto>> GetByDecisionIdAsync(string decisionId);
        Task<ApiResponse<CreditDecisionResponseDto>> GetByApplicationIdAsync(string applicationId);
        Task<ApiResponse<List<CreditDecisionResponseDto>>> GetByCustomerIdAsync(string customerId);
        Task<ApiResponse<PaginatedResponse<CreditDecisionResponseDto>>> GetAllAsync(CreditDecisionQueryDto query);

        // Decision Operations
        Task<ApiResponse<CreditDecisionResponseDto>> ApproveDecisionAsync(string id, ApproveCreditDecisionDto dto);
        Task<ApiResponse<CreditDecisionResponseDto>> DeclineDecisionAsync(string id, DeclineCreditDecisionDto dto);
        Task<ApiResponse<CreditDecisionResponseDto>> AcceptCounterOfferAsync(string id, AcceptCounterOfferDto dto);
        Task<ApiResponse<CreditDecisionResponseDto>> UpdateDecisionStatusAsync(string id, string status, string updatedBy);

        // Condition Operations
        Task<ApiResponse<CreditDecisionResponseDto>> UpdateConditionsAsync(string id, List<UpdateUnderwritingConditionDto> conditions);
        Task<ApiResponse<bool>> MetConditionAsync(string decisionId, string conditionId, string metBy);

        // Statistics
        Task<ApiResponse<CreditDecisionStatisticsDto>> GetStatisticsAsync(DateTime? fromDate = null, DateTime? toDate = null);
    }
}
