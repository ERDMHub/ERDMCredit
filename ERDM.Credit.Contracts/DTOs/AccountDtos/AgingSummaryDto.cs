namespace ERDM.Credit.Contracts.DTOs.AccountDtos
{
    public class AgingSummaryDto
    {
        public int TotalAccounts { get; set; }
        public int DelinquentAccounts { get; set; }
        public decimal TotalOutstandingBalance { get; set; }
        public decimal TotalOverdueAmount { get; set; }
        public decimal DelinquencyPercentage { get; set; }
    }
}
