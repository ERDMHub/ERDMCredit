using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERDM.Credit.Contracts.DTOs.AccountDtos
{
    public class AgingReportDto
    {
        public DateTime AsOfDate { get; set; }
        public AgingBucketSummaryDto Current { get; set; } = new();
        public AgingBucketSummaryDto Days1To30 { get; set; } = new();
        public AgingBucketSummaryDto Days31To60 { get; set; } = new();
        public AgingBucketSummaryDto Days61To90 { get; set; } = new();
        public AgingBucketSummaryDto Days91Plus { get; set; } = new();

        public List<AgingAccountDto> Accounts { get; set; } = new();
        public AgingSummaryDto Total { get; set; } = new();
    }
}
