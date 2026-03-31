namespace ERDM.Credit.Contracts.DTOs
{
    public class AddressDto
    {
        public AddressDto()
        {
            
        }
        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }
    }
}
