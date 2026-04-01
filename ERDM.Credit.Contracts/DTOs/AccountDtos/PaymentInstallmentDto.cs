namespace ERDM.Credit.Contracts.DTOs.AccountDtos
{
    public class PaymentInstallmentDto
    {
        public int InstallmentNumber { get; set; }
        public DateTime DueDate { get; set; }
        public decimal AmountDue { get; set; }
        public decimal PrincipalAmount { get; set; }
        public decimal InterestAmount { get; set; }
        public decimal BalanceAfterPayment { get; set; }
        public string Status { get; set; } = string.Empty; // Paid, Due, Overdue, Upcoming
        public decimal? AmountPaid { get; set; }
        public DateTime? PaidDate { get; set; }
        public int? LateDays { get; set; }
        public decimal? LateFeeCharged { get; set; }
    }
}
