
namespace ERDM.Credit.Contracts.DTOs
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
