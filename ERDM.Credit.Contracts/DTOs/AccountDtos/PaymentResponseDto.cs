namespace ERDM.Credit.Contracts.DTOs.AccountDtos
{
    public class PaymentResponseDto
    {
        public string PaymentId { get; set; } = string.Empty;
        public string AccountId { get; set; } = string.Empty;
        public string AccountNumber { get; set; } = string.Empty;
        public decimal AmountPaid { get; set; }
        public decimal PrincipalPaid { get; set; }
        public decimal InterestPaid { get; set; }
        public decimal FeesPaid { get; set; }
        public string PaymentMethod { get; set; } = string.Empty;
        public string TransactionReference { get; set; } = string.Empty;
        public DateTime PaymentDate { get; set; }
        public decimal RemainingBalance { get; set; }
        public bool IsFullPayment { get; set; }
        public bool IsLatePayment { get; set; }
        public int LateDays { get; set; }
        public string Status { get; set; } = string.Empty;
        public string? Notes { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
