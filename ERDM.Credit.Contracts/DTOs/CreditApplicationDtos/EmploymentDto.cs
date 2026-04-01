namespace ERDM.Credit.Contracts.DTOs.CreditApplicationDtos
{
    public class EmploymentDto
    {
        public EmploymentDto()
        {
            
        }
        public string EmployerName { get; set; }
        public string JobTitle { get; set; }
        public decimal MonthlyIncome { get; set; }
        public int YearsEmployed { get; set; }
        public string EmploymentStatus { get; set; }
    }
}
