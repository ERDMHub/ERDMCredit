namespace ERDM.Credit.Contracts.DTOs
{
    public class DecisionDto
    {
        public DecisionDto()
        {
            
        }
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
