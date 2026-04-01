namespace ERDM.Credit.Domain.Entities
{
    public enum PaymentStatus
    {
        Paid = 1,
        PartiallyPaid = 2,
        Due = 3,
        Overdue = 4,
        Missed = 5,
        Advance = 6,
        LateFeeApplied = 7,
        Reversed = 8,
    }
}
