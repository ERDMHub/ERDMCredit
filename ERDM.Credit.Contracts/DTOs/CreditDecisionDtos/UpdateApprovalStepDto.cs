namespace ERDM.Credit.Contracts.DTOs.CreditDecisionDtos
{
    public class UpdateApprovalStepDto
    {
        public int StepNumber { get; set; }
        public string ApproverId { get; set; } = string.Empty;
        public string? Comments { get; set; }
        public string Status { get; set; } = string.Empty;
    }

}
