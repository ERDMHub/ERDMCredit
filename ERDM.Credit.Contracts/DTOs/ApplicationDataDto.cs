namespace ERDM.Credit.Contracts.DTOs
{
    public class ApplicationDataDto
    {
        public string EmploymentStatus { get; set; }
        public decimal AnnualIncome { get; set; }
        public string EmployerName { get; set; }
        public int YearsAtEmployer { get; set; }
        public string HousingStatus { get; set; }
        public decimal MonthlyExpenses { get; set; }
        public decimal ExistingDebts { get; set; }
        public string Purpose { get; set; }
    }
}
