namespace ERDM.Credit.Domain.DomainEvents
{
    public class RestructuringDetails
    {
        public decimal OldOutstandingBalance { get; set; }
        public decimal OldInterestRate { get; set; }
        public int OldTermMonths { get; set; }
        public decimal OldEMI { get; set; }
        public string RestructuringType { get; set; } = string.Empty;
        public string RestructuringReason { get; set; } = string.Empty;
        public string RestructuredBy { get; set; } = string.Empty;
    }
}
