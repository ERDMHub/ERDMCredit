namespace ERDM.Credit.Contracts.DTOs.AccountDtos
{
    public class AccountStatisticsDto
    {
        // Counts
        public int TotalAccounts { get; set; }
        public int ActiveAccounts { get; set; }
        public int DelinquentAccounts { get; set; }
        public int ClosedAccounts { get; set; }
        public int WrittenOffAccounts { get; set; }
        public int DefaultedAccounts { get; set; }

        // Balances
        public decimal TotalOutstandingBalance { get; set; }
        public decimal TotalDisbursedAmount { get; set; }
        public decimal TotalPrincipalPaid { get; set; }
        public decimal TotalInterestPaid { get; set; }
        public decimal TotalInterestEarned { get; set; }
        public decimal TotalLateFeesCollected { get; set; }

        // Averages
        public decimal AverageOutstandingBalance { get; set; }
        public decimal AverageDisbursedAmount { get; set; }
        public decimal AverageInterestRate { get; set; }
        public int AverageTermMonths { get; set; }
        public decimal AverageEMI { get; set; }

        // Breakdowns
        public Dictionary<string, int> AccountsByStatus { get; set; } = new();
        public Dictionary<string, int> AccountsByType { get; set; } = new();
        public Dictionary<string, decimal> BalanceByProductType { get; set; } = new();
        public Dictionary<string, int> AccountsByBranch { get; set; } = new();
        public Dictionary<string, int> DelinquencyBuckets { get; set; } = new();

        public DateTime AsOfDate { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
    }
}
