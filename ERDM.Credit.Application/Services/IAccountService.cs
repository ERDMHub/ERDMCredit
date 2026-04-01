using ERDM.Credit.Contracts.DTOs.AccountDtos;
using ERDM.Credit.Contracts.DTOs.CreditApplicationDtos;
using ERDM.Credit.Contracts.Wrapper;

namespace ERDM.Credit.Application.Services
{
    public interface IAccountService
    {
        // Create Operations
        Task<ApiResponse<AccountResponseDto>> CreateAsync(CreateAccountDto dto);
        Task<ApiResponse<AccountResponseDto>> CreateFromApplicationAsync(string applicationId, CreateAccountFromApplicationDto dto);

        // Read Operations
        Task<ApiResponse<AccountResponseDto>> GetByIdAsync(string id);
        Task<ApiResponse<AccountResponseDto>> GetByAccountNumberAsync(string accountNumber);
        Task<ApiResponse<AccountResponseDto>> GetByApplicationIdAsync(string applicationId);
        Task<ApiResponse<PaginatedResponse<AccountResponseDto>>> GetAllAsync(AccountQueryDto query);
        Task<ApiResponse<PaginatedResponse<AccountResponseDto>>> GetAllAsync();
        Task<ApiResponse<List<AccountResponseDto>>> GetByCustomerIdAsync(string customerId);

        // Status Operations
        Task<ApiResponse<AccountResponseDto>> ActivateAccountAsync(string id, ActivateAccountDto dto);
        Task<ApiResponse<AccountResponseDto>> CloseAccountAsync(string id, CloseAccountDto dto);
        Task<ApiResponse<AccountResponseDto>> SuspendAccountAsync(string id, SuspendAccountDto dto);
        Task<ApiResponse<AccountResponseDto>> RestructureAccountAsync(string id, RestructureAccountDto dto);
        Task<ApiResponse<AccountResponseDto>> MarkAsDelinquentAsync(string id, MarkDelinquentDto dto);
        Task<ApiResponse<AccountResponseDto>> WriteOffAccountAsync(string id, WriteOffAccountDto dto);

        // Payment Operations
        Task<ApiResponse<PaymentResponseDto>> ProcessPaymentAsync(string accountId, ProcessPaymentDto dto);
        Task<ApiResponse<PaymentScheduleResponseDto>> GetPaymentScheduleAsync(string accountId);
        Task<ApiResponse<PaymentHistoryResponseDto>> GetPaymentHistoryAsync(string accountId, PaymentHistoryQueryDto query);
        Task<ApiResponse<PaymentResponseDto>> ReversePaymentAsync(string accountId, string paymentId, ReversePaymentDto dto);

        // Financial Operations
        Task<ApiResponse<AccountResponseDto>> UpdateOutstandingBalanceAsync(string id, UpdateBalanceDto dto);
        Task<ApiResponse<AccountResponseDto>> AdjustAvailableCreditAsync(string id, AdjustCreditDto dto);
        Task<ApiResponse<AccountResponseDto>> ApplyLateFeeAsync(string id, ApplyLateFeeDto dto);
        Task<ApiResponse<AccountResponseDto>> RestructurePaymentAsync(string id, RestructurePaymentDto dto);

        // Disbursement Operations
        Task<ApiResponse<AccountResponseDto>> DisburseAmountAsync(string id, DisburseAmountDto dto);
        Task<ApiResponse<AccountResponseDto>> ConfirmDisbursementAsync(string id, ConfirmDisbursementDto dto);

        // Statistics and Reports
        Task<ApiResponse<AccountStatisticsDto>> GetStatisticsAsync(AccountStatisticsQueryDto query);
        Task<ApiResponse<AccountSummaryDto>> GetAccountSummaryAsync(string? customerId = null);
        Task<ApiResponse<PortfolioPerformanceDto>> GetPortfolioPerformanceAsync(PerformanceQueryDto query);
        
        // Bulk Operations
        Task<ApiResponse<BulkOperationResponseDto>> BulkUpdateStatusAsync(BulkStatusUpdateDto dto);
        Task<ApiResponse<BulkOperationResponseDto>> BulkMarkOverdueAccountsAsync(BulkOverdueDto dto);

        // Validation and Utilities
        Task<ApiResponse<bool>> ValidateAccountForDisbursementAsync(string accountId);
        Task<ApiResponse<decimal>> CalculateEMIAsync(CalculateEMIDto dto);
        Task<ApiResponse<AmortizationScheduleDto>> CalculateAmortizationScheduleAsync(CalculateAmortizationDto dto);
    }
}
