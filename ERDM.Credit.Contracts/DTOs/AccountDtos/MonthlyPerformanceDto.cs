namespace ERDM.Credit.Contracts.DTOs.AccountDtos
{
    public class MonthlyPerformanceDto
    {
        public int Year { get; set; }
        public int Month { get; set; }
        public decimal Disbursements { get; set; }
        public decimal Collections { get; set; }
        public decimal InterestEarned { get; set; }
        public decimal DelinquencyRate { get; set; }
        public int NewAccounts { get; set; }
        public int ClosedAccounts { get; set; }
    }
}
