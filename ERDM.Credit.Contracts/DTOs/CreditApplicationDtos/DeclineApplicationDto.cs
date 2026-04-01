namespace ERDM.Credit.Contracts.DTOs.CreditApplicationDtos
{
    public class DeclineApplicationDto
    {
        public DeclineApplicationDto()
        {
            
        }
        public List<string> DeclineReasons { get; set; }
        public string DecidedBy { get; set; }
    }
}
