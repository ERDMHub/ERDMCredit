namespace ERDM.Credit.Contracts.DTOs.AccountDtos
{
    public class PaymentSummaryDto
    {
        public int TotalPayments { get; set; }
        public decimal TotalAmountPaid { get; set; }
        public decimal TotalPrincipalPaid { get; set; }
        public decimal TotalInterestPaid { get; set; }
        public decimal TotalFeesPaid { get; set; }
        public int LatePayments { get; set; }
        public int OnTimePayments { get; set; }
        public decimal AveragePaymentAmount { get; set; }
        public DateTime? LastPaymentDate { get; set; }
        public decimal? LastPaymentAmount { get; set; }
    }
}
