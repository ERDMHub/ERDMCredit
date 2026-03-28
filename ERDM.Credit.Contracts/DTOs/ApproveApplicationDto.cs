namespace ERDM.Credit.Contracts.DTOs
{
    public class ApproveApplicationDto
    {
        public decimal ApprovedAmount { get; set; }
        public decimal InterestRate { get; set; }
        public string RiskGrade { get; set; }
        public List<string> ReasonCodes { get; set; }
        public string DecidedBy { get; set; }
    }
}
