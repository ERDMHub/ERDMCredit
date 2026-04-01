namespace ERDM.Credit.Domain.Entities
{
    public enum AccountStatus
    {
        PendingApproval = 1,
        Approved = 2,
        Active = 3,
        Disbursed = 4,
        Delinquent = 5,
        Restructured = 6,
        Closed = 7,
        WrittenOff = 8,
        Defaulted = 9,
        Suspended = 10
    }
}
