namespace ERDM.Credit.Contracts.DTOs.AccountDtos
{
    public class PaymentHistoryQueryDto
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public string? PaymentStatus { get; set; }
        public bool? IsLatePayment { get; set; }
    }
}
