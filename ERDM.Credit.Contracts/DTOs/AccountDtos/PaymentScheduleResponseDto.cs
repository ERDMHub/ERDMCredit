namespace ERDM.Credit.Contracts.DTOs.AccountDtos
{
    public class PaymentScheduleResponseDto
    {
        public string AccountId { get; set; } = string.Empty;
        public string AccountNumber { get; set; } = string.Empty;
        public decimal TotalPrincipal { get; set; }
        public decimal TotalInterest { get; set; }
        public decimal TotalAmount { get; set; }
        public List<PaymentInstallmentDto> Installments { get; set; } = new();
    }
}
