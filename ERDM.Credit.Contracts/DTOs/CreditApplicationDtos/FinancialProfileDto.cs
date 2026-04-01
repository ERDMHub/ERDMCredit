namespace ERDM.Credit.Contracts.DTOs.CreditApplicationDtos
{
    public class FinancialProfileDto
    {
        public FinancialProfileDto()
        {
            
        }
        public decimal MonthlyExpenses { get; set; }
        public decimal ExistingDebt { get; set; }
        public int CreditScore { get; set; }
        public decimal SavingsAmount { get; set; }
        public string BankName { get; set; }
        public string AccountNumber { get; set; }
        public string AccountType { get; set; }
        public decimal OtherAssets { get; set; }
        public decimal Liabilities { get; set; }
        public decimal MonthlyDebtPayments { get; set; }
        public List<CreditCardDto> CreditCards { get; set; }
        public List<LoanDto> Loans { get; set; }
        public bool BankStatementsProvided { get; set; }
        public DateTime? LastCreditCheckDate { get; set; }
    }

}
