namespace ERDM.Credit.Contracts.DTOs.CreditApplicationDtos
{
    public class CreditCardDto
    {
        public CreditCardDto()
        {
            
        }
        public string Issuer { get; set; }
        public decimal CreditLimit { get; set; }
        public decimal CurrentBalance { get; set; }
        public decimal PaymentAmount { get; set; }
        public decimal InterestRate { get; set; }
        public DateTime OpenedDate { get; set; }
    }
}
