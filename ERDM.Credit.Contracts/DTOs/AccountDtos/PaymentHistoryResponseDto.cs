using ERDM.Credit.Contracts.Wrapper;

namespace ERDM.Credit.Contracts.DTOs.AccountDtos
{
    public class PaymentHistoryResponseDto
    {
        public PaginatedResponse<PaymentResponseDto> Payments { get; set; } = new();
        public PaymentSummaryDto Summary { get; set; } = new();
    }
}
