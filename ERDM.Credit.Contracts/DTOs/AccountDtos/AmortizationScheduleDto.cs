using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERDM.Credit.Contracts.DTOs.AccountDtos
{
    public class AmortizationScheduleDto
    {
        public decimal PrincipalAmount { get; set; }
        public decimal InterestRate { get; set; }
        public int TermMonths { get; set; }
        public decimal TotalInterest { get; set; }
        public decimal TotalPayment { get; set; }
        public decimal EmiAmount { get; set; }
        public List<AmortizationEntryDto> Entries { get; set; } = new();
    }
}
