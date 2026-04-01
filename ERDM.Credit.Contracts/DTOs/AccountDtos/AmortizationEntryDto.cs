namespace ERDM.Credit.Contracts.DTOs.AccountDtos
{
    public class AmortizationEntryDto
    {
        public int Period { get; set; }
        public DateTime PaymentDate { get; set; }
        public decimal BeginningBalance { get; set; }
        public decimal PaymentAmount { get; set; }
        public decimal PrincipalPaid { get; set; }
        public decimal InterestPaid { get; set; }
        public decimal EndingBalance { get; set; }
        public bool IsCompleted { get; set; }
    }
}
