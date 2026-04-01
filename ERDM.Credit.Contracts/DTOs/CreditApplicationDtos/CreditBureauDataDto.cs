namespace ERDM.Credit.Contracts.DTOs.CreditApplicationDtos
{
    public class CreditBureauDataDto
    {
        public CreditBureauDataDto()
        {
            
        }
        public string Bureau { get; set; }
        public int Score { get; set; }
        public DateTime ScoreDate { get; set; }
        public string InquiryId { get; set; }
        public string RiskLevel { get; set; }
        public decimal DebtToIncomeRatio { get; set; }
        public int TotalAccounts { get; set; }
        public int DelinquentAccounts { get; set; }
        public decimal TotalDebt { get; set; }
        public Dictionary<string, object> AdditionalData { get; set; }
    }
}
