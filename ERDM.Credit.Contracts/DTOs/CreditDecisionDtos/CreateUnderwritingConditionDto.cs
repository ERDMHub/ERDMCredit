
namespace ERDM.Credit.Contracts.DTOs.CreditDecisionDtos
{
    public class CreateUnderwritingConditionDto
    {
        public string ConditionType { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime DueDate { get; set; }
    }

}
