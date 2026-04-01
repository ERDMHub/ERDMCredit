namespace ERDM.Credit.Domain.Entities
{
    public class CollateralDetails
    {
        public string CollateralType { get; set; } = string.Empty;
        public decimal CollateralValue { get; set; }
        public string CollateralDescription { get; set; } = string.Empty;
        public List<string> CollateralDocuments { get; set; } = new();
        public DateTime ValuationDate { get; set; }
        public string ValuationOfficer { get; set; } = string.Empty;
        public InsuranceDetails? InsuranceDetails { get; set; }
    }
}
