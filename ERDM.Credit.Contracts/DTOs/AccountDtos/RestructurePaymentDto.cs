using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERDM.Credit.Contracts.DTOs.AccountDtos
{
    public class RestructurePaymentDto
    {
        public string RestructuredBy { get; set; } = string.Empty;
        public string RestructuringType { get; set; } = string.Empty; // Deferment, ReducedPayment, SkipPayment
        public int? DeferralMonths { get; set; }
        public decimal? ReducedPaymentAmount { get; set; }
        public DateTime? NewPaymentDate { get; set; }
        public string Reason { get; set; } = string.Empty;
        public string? ApprovalReference { get; set; }
        public string? Comments { get; set; }
    }
}
