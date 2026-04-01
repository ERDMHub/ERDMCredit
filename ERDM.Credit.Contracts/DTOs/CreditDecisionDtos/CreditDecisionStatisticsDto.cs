namespace ERDM.Credit.Contracts.DTOs.CreditDecisionDtos
{
    public class CreditDecisionStatisticsDto
    {
        public int TotalDecisions { get; set; }
        public int Approved { get; set; }
        public int Declined { get; set; }
        public int CounterOffers { get; set; }
        public int Pending { get; set; }
        public int Referred { get; set; }
        public decimal TotalApprovedAmount { get; set; }
        public decimal AverageApprovedAmount { get; set; }
        public decimal AverageInterestRate { get; set; }
        public Dictionary<string, int> DecisionsByRiskGrade { get; set; } = new();
        public Dictionary<string, int> DecisionsByProductType { get; set; } = new();
        public Dictionary<string, int> DecisionsByDay { get; set; } = new();
        public DateTime AsOfDate { get; set; }
    }
}
