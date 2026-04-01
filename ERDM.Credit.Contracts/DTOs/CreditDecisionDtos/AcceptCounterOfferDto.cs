namespace ERDM.Credit.Contracts.DTOs.CreditDecisionDtos
{
    public class AcceptCounterOfferDto
    {
        public string AcceptedBy { get; set; } = string.Empty;
        public DateTime AcceptanceDate { get; set; }
        public string? Comments { get; set; }
    }
}
