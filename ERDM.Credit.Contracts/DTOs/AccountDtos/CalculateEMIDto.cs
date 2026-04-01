namespace ERDM.Credit.Contracts.DTOs.AccountDtos
{
    public class CalculateEMIDto
    {
        public decimal PrincipalAmount { get; set; }
        public decimal InterestRate { get; set; }
        public int TermMonths { get; set; }
        public string InterestType { get; set; } = "Reducing"; // Reducing, Flat, Fixed
    }

}
