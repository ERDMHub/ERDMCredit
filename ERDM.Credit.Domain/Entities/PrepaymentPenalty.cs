namespace ERDM.Credit.Domain.Entities
{
    public class PrepaymentPenalty
    {
        public string PenaltyType { get; set; } = string.Empty; // Percentage or Fixed
        public decimal PenaltyValue { get; set; }
        public int LockInPeriodMonths { get; set; }
        public int? AppliesWithinMonths { get; set; }
    }
}
