namespace ERDM.Credit.Contracts.DTOs.RiskScoreDtos
{
      public class UpdateRiskMetricsDto
    {
        public decimal ProbabilityOfDefault { get; set; }
        public decimal LossGivenDefault { get; set; }
        public decimal ExposureAtDefault { get; set; }
        public string UpdatedBy { get; set; } = string.Empty;
    }
}   