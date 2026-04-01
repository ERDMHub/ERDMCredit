namespace ERDM.Credit.Contracts.DTOs.AccountDtos
{
    public class AgingBucketSummaryDto
    {
        public int AccountCount { get; set; }
        public decimal TotalOutstandingBalance { get; set; }
        public decimal TotalOverdueAmount { get; set; }
        public decimal PercentageOfPortfolio { get; set; }
    }

}
