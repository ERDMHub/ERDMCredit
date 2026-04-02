namespace ERDM.Credit.Contracts.DTOs.RiskScoreDtos
{
     public class UpdateRiskScoreDto
    {
        public int ScoreValue { get; set; }
        public string ScoreGrade { get; set; } = string.Empty;
        public string RiskCategory { get; set; } = string.Empty;
        public string UpdatedBy { get; set; } = string.Empty;
        public string? ReviewNotes { get; set; }
    }
}