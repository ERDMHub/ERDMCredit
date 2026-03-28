namespace ERDM.Credit.Domain.Entities
{
    public class UnderwritingStage
    {
        public string Stage { get; set; }
        public string Status { get; set; }
        public DateTime StartedAt { get; set; }
        public DateTime? CompletedAt { get; set; }
        public string Result { get; set; }
    }
}
