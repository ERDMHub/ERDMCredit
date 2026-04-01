namespace ERDM.Credit.Contracts.DTOs.AccountDtos
{
    public class ProcessPaymentDto
    {
        public decimal Amount { get; set; }
        public string PaymentMethod { get; set; } = string.Empty;
        public string TransactionReference { get; set; } = string.Empty;
        public DateTime PaymentDate { get; set; } = DateTime.UtcNow;
        public string? Notes { get; set; }
        public Dictionary<string, string>? PaymentMetadata { get; set; }
    }
}
