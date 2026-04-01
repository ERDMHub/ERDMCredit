
namespace ERDM.Credit.Domain.Entities
{
    public class PaymentHistory
    {
        public string PaymentId { get; set; } = Guid.NewGuid().ToString();
        public DateTime PaymentDate { get; set; }
        public decimal AmountDue { get; set; }
        public decimal AmountPaid { get; set; }
        public decimal PrincipalPaid { get; set; }
        public decimal InterestPaid { get; set; }
        public decimal FeesPaid { get; set; }
        public RepaymentMethod PaymentMethod { get; set; }
        public string TransactionReference { get; set; } = string.Empty;
        public PaymentStatus Status { get; set; }
        public DateTime DueDate { get; set; }
        public int LateDays { get; set; }
        public decimal LateFeeCharged { get; set; }
        public string? Notes { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
