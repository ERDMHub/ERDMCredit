namespace ERDM.Credit.Domain.Entities
{
    public class InsuranceDetails
    {
        public string PolicyNumber { get; set; } = string.Empty;
        public string InsuranceProvider { get; set; } = string.Empty;
        public decimal CoverageAmount { get; set; }
        public DateTime ExpiryDate { get; set; }
        public bool PremiumPaid { get; set; }
    }
}
