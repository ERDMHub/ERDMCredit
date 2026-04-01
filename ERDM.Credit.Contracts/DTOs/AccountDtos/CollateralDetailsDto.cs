using ERDM.Credit.Contracts.DTOs.CreditApplicationDtos;

namespace ERDM.Credit.Contracts.DTOs.AccountDtos
{
    public class CollateralDetailsDto
    {
        public string CollateralType { get; set; } = string.Empty;
        public decimal CollateralValue { get; set; }
        public string CollateralDescription { get; set; } = string.Empty;
        public List<string> CollateralDocuments { get; set; } = new();
        public DateTime ValuationDate { get; set; }
        public string ValuationOfficer { get; set; } = string.Empty;
        public InsuranceDetailsDto? InsuranceDetails { get; set; }
    }
}
