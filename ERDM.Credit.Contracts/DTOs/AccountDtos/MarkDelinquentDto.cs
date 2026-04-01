namespace ERDM.Credit.Contracts.DTOs.AccountDtos
{
    public class MarkDelinquentDto
    {
        public string MarkedBy { get; set; } = string.Empty;
        public int DaysOverdue { get; set; }
        public decimal OverdueAmount { get; set; }
        public string? DelinquencyReason { get; set; }
        public bool AssignToCollections { get; set; } = true;
        public string? AssignedCollectionOfficer { get; set; }
        public string? Comments { get; set; }
    }
}
