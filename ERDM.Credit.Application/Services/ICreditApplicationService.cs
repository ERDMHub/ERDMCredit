using ERDM.Credit.Contracts.DTOs;
using ERDM.Credit.Contracts.Wrapper;


namespace ERDM.Credit.Application.Services
{
    public interface ICreditApplicationService
    {
        Task<ApiResponse<CreditApplicationResponseDto>> CreateAsync(CreateCreditApplicationDto dto);
        Task<ApiResponse<CreditApplicationResponseDto>> GetByIdAsync(string id);
        Task<ApiResponse<CreditApplicationResponseDto>> GetByApplicationNumberAsync(string applicationNumber);
        Task<ApiResponse<PaginatedResponse<CreditApplicationResponseDto>>> GetAllAsync(ApplicationQueryDto query);
        Task<ApiResponse<PaginatedResponse<CreditApplicationResponseDto>>> GetAllAsync();
        Task<ApiResponse<CreditApplicationResponseDto>> SubmitAsync(string id);
        Task<ApiResponse<CreditApplicationResponseDto>> ApproveAsync(string id, ApproveApplicationDto dto);
        Task<ApiResponse<CreditApplicationResponseDto>> DeclineAsync(string id, DeclineApplicationDto dto);
        Task<ApiResponse<Dictionary<string, int>>> GetStatisticsAsync();
    }
}
