using ERDM.Credit.Contracts.DTOs.AccountDtos;

namespace ERDM.Credit.Contracts.DTOs.CreditApplicationDtos
{
    public class CreditApplicationUpdateDto
    {
        public CreditApplicationUpdateDto()
        {
            
        }
        public CustomerProfileDto CustomerProfile { get; set; }
        public string ProductType { get; set; }
        public decimal? RequestedAmount { get; set; }
        public int? RequestedTerm { get; set; }
        public ApplicationDataDto ApplicationData { get; set; }
        public ApplicationMetadataDto Metadata { get; set; }
    }
}
