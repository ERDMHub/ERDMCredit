namespace ERDM.Credit.Contracts.DTOs.AccountDtos
{
    public class WriteOffAccountDto
    {
        public string WrittenOffBy { get; set; } = string.Empty;
        public string WriteOffReason { get; set; } = string.Empty;
        public decimal WriteOffAmount { get; set; }
        public string WriteOffType { get; set; } = string.Empty; // Full, Partial
        public string? ApprovalReference { get; set; }
        public bool TransferToCollections { get; set; } = true;
        public string? Comments { get; set; }
    }
}
