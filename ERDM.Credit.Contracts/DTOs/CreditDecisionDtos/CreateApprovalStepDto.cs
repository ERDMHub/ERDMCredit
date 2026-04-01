namespace ERDM.Credit.Contracts.DTOs.CreditDecisionDtos
{
    public class CreateApprovalStepDto
    {
        public int StepNumber { get; set; }
        public string ApproverRole { get; set; } = string.Empty;
    }

}
