namespace ERDM.Credit.Contracts.DTOs.AccountDtos
{
    public class PaymentBehaviorDto
    {
        public int OnTimePayments { get; set; }
        public int LatePayments { get; set; }
        public int MissedPayments { get; set; }
        public decimal PaymentConsistencyScore { get; set; } // 0-100
        public int AveragePaymentDelayDays { get; set; }
        public bool IsHighRisk { get; set; }
    }
}
