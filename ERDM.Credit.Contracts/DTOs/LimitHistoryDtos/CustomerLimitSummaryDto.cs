using System;
using System.Collections.Generic;

namespace ERDM.Credit.Contracts.DTOs.LimitHistoryDtos
{
        public class CustomerLimitSummaryDto
    {
        public string CustomerId { get; set; } = string.Empty;
        public string CustomerName { get; set; } = string.Empty;
        public decimal CurrentTotalLimit { get; set; }
        public decimal PreviousTotalLimit { get; set; }
        public decimal AvailableCredit { get; set; }
        public decimal UtilizedCredit { get; set; }
        public int TotalLimitChanges { get; set; }
        public DateTime LastLimitChangeDate { get; set; }
        public string? LastChangeType { get; set; }
        public List<LimitHistorySummaryItemDto> RecentChanges { get; set; } = new();
        public List<ActiveTemporaryLimitDto> ActiveTemporaryLimits { get; set; } = new();
    }

}
