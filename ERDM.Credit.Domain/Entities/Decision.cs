namespace ERDM.Credit.Domain.Entities
{
    public class Decision
    {
        public string Status { get; set; }
        public decimal? ApprovedAmount { get; set; }
        public decimal? InterestRate { get; set; }
        public string RiskGrade { get; set; }
        public List<string> ReasonCodes { get; set; }
        public List<string> DeclineReasons { get; set; }
        public string DecidedBy { get; set; }
        public DateTime? DecidedAt { get; set; }
    }
}
