
namespace ERDM.Credit.Contracts.DTOs
{
    public class CreditApplicationResponseDto
    {
        public string Id { get; set; }
        public string ApplicationId { get; set; }
        public string CustomerId { get; set; }
        public CustomerProfileDto CustomerProfile { get; set; }
        public string ProductType { get; set; }
        public decimal RequestedAmount { get; set; }
        public int RequestedTerm { get; set; }
        public string Status { get; set; }
        public DecisionDto Decision { get; set; }
        public ApplicationDataDto ApplicationData { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
