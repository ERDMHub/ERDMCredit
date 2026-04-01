namespace ERDM.Credit.Contracts.DTOs.AccountDtos
{
    public class LoanPerformanceMetricsDto
    {
        public decimal TotalDisbursements { get; set; }
        public decimal TotalCollections { get; set; }
        public decimal TotalInterestEarned { get; set; }
        public decimal TotalLateFees { get; set; }

        public decimal PortfolioAtRisk30Days { get; set; }
        public decimal PortfolioAtRisk60Days { get; set; }
        public decimal PortfolioAtRisk90Days { get; set; }

        public decimal DelinquencyRate { get; set; }
        public decimal DefaultRate { get; set; }
        public decimal PrepaymentRate { get; set; }

        public int NumberOfActiveLoans { get; set; }
        public int NumberOfDelinquentLoans { get; set; }
        public int NumberOfDefaultedLoans { get; set; }

        public decimal AverageLoanSize { get; set; }
        public decimal AverageInterestRate { get; set; }
        public int AverageTermMonths { get; set; }

        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
    }
}
