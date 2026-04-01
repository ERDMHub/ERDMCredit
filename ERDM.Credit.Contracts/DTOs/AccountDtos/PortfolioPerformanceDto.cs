namespace ERDM.Credit.Contracts.DTOs.AccountDtos
{
    public class PortfolioPerformanceDto
    {
        public decimal TotalPortfolioValue { get; set; }
        public decimal TotalDisbursements { get; set; }
        public decimal TotalCollections { get; set; }
        public decimal TotalInterestEarned { get; set; }
        public decimal TotalFeesCollected { get; set; }

        // Delinquency Metrics
        public decimal PortfolioAtRisk30Days { get; set; }
        public decimal PortfolioAtRisk60Days { get; set; }
        public decimal PortfolioAtRisk90Days { get; set; }
        public decimal PortfolioAtRiskPercentage { get; set; }

        // Performance Ratios
        public decimal DelinquencyRate { get; set; }
        public decimal DefaultRate { get; set; }
        public decimal PrepaymentRate { get; set; }
        public decimal CollectionEfficiency { get; set; }

        // Trends
        public List<MonthlyPerformanceDto> MonthlyTrends { get; set; } = new();

        // Comparisons
        public Dictionary<string, decimal> PerformanceByProduct { get; set; } = new();
        public Dictionary<string, decimal> PerformanceByBranch { get; set; } = new();

        public DateTime AsOfDate { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
    }
}
