using ERDM.Credit.Contracts.DTOs.RiskScoreDtos;
using ERDM.Credit.Contracts.Wrapper;

namespace ERDM.Credit.Application.Services
{
    public interface IRiskScoreService
    {
        // Create Operations
        Task<ApiResponse<RiskScoreResponseDto>> CreateAsync(CreateRiskScoreDto dto);
        
        // Read Operations
        Task<ApiResponse<RiskScoreResponseDto>> GetByIdAsync(string id);
        Task<ApiResponse<RiskScoreResponseDto>> GetByRiskScoreIdAsync(string riskScoreId);
        Task<ApiResponse<List<RiskScoreResponseDto>>> GetByCustomerIdAsync(string customerId);
        Task<ApiResponse<RiskScoreResponseDto>> GetLatestByCustomerIdAsync(string customerId);
        Task<ApiResponse<List<RiskScoreResponseDto>>> GetByApplicationIdAsync(string applicationId);
        Task<ApiResponse<List<RiskScoreResponseDto>>> GetByAccountIdAsync(string accountId);
        Task<ApiResponse<PaginatedResponse<RiskScoreResponseDto>>> GetAllAsync(RiskScoreQueryDto query);
        
        // Update Operations
        Task<ApiResponse<RiskScoreResponseDto>> UpdateScoreAsync(string id, UpdateRiskScoreDto dto);
        Task<ApiResponse<RiskScoreResponseDto>> UpdateRiskMetricsAsync(string id, UpdateRiskMetricsDto dto);
        Task<ApiResponse<RiskScoreResponseDto>> AddRiskFactorAsync(string id, AddRiskFactorDto dto);
        Task<ApiResponse<RiskScoreResponseDto>> InvalidateScoreAsync(string id, InvalidateRiskScoreDto dto);
        Task<ApiResponse<RiskScoreResponseDto>> ScheduleReviewAsync(string id, ScheduleReviewDto dto);
        
        // Statistics and Reports
        Task<ApiResponse<RiskScoreStatisticsDto>> GetStatisticsAsync(DateTime? fromDate = null, DateTime? toDate = null);
        Task<ApiResponse<CustomerRiskProfileDto>> GetCustomerRiskProfileAsync(string customerId);
        Task<ApiResponse<List<RiskThresholdAlertDto>>> GetRiskThresholdAlertsAsync(int threshold = 600);
        
        // Bulk Operations
        Task<ApiResponse<int>> BulkUpdateValidityAsync();
    }
}