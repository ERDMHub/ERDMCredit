namespace ERDM.Credit.Contracts.DTOs
{
    public class LoanDto
    {
        public string LoanType { get; set; }
        public decimal OriginalAmount { get; set; }
        public decimal CurrentBalance { get; set; }
        public decimal MonthlyPayment { get; set; }
        public decimal InterestRate { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool Secured { get; set; }
        public string Collateral { get; set; }
    }
}
