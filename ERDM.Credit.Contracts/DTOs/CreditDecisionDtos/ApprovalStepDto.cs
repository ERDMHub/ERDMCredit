
namespace ERDM.Credit.Contracts.DTOs.CreditDecisionDtos
{
    public class ApprovalStepDto
    {
        public int StepNumber { get; set; }
        public string ApproverRole { get; set; } = string.Empty;
        public string? ApproverId { get; set; }
        public string? ApproverName { get; set; }
        public DateTime? ApprovedDate { get; set; }
        public string? Comments { get; set; }
        public string Status { get; set; } = string.Empty;
    }
}
