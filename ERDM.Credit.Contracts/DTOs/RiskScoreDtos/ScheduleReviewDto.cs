namespace ERDM.Credit.Contracts.DTOs.RiskScoreDtos
{
    public class ScheduleReviewDto
    {
        public DateTime ReviewDate { get; set; }
        public string ScheduledBy { get; set; } = string.Empty;
    }

}