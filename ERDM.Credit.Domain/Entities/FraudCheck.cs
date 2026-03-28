namespace ERDM.Credit.Domain.Entities
{
    public class FraudCheck
    {
        public string Status { get; set; }
        public int RiskScore { get; set; }
        public DateTime CheckedAt { get; set; }
        public string CaseId { get; set; }
    }
}
