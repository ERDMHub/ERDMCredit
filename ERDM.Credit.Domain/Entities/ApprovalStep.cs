using ERDM.Credit.Domain.Enums;

namespace ERDM.Credit.Domain.Entities
{
    public class ApprovalStep
    {
        public int StepNumber { get; set; }
        public string ApproverRole { get; set; } = string.Empty;
        public string? ApproverId { get; set; }
        public string? ApproverName { get; set; }
        public DateTime? ApprovedDate { get; set; }
        public string? Comments { get; set; }
        public ApprovalStepStatus Status { get; set; }
    }
}
