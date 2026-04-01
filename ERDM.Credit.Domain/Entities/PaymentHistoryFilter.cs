
namespace ERDM.Credit.Domain.Entities
{
    public class PaymentHistoryFilter
    {
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public PaymentStatus? Status { get; set; }
        public bool? IsLatePayment { get; set; }
    }
}
