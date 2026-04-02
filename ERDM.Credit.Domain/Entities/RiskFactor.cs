namespace ERDM.Credit.Domain.Entities
{
       public class RiskFactor
    {
        public string FactorId { get; set; } = string.Empty;
        public string FactorName { get; set; } = string.Empty;
        public string FactorValue { get; set; } = string.Empty;
        public decimal Weight { get; set; }
        public string Impact { get; set; } = string.Empty;
        public DateTime AssessedAt { get; set; }
    }
}   