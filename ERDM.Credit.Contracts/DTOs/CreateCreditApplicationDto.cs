namespace ERDM.Credit.Contracts.DTOs
{
    public class CreateCreditApplicationDto
    {
        public CreateCreditApplicationDto() { } 

        public string CustomerId { get; set; }
        public CustomerProfileDto CustomerProfile { get; set; }
        public string ProductType { get; set; }
        public decimal RequestedAmount { get; set; }
        public int RequestedTerm { get; set; }
        public ApplicationDataDto ApplicationData { get; set; }
        public ApplicationMetadataDto Metadata { get; set; }
    }
}
