using System;
using System.Collections.Generic;

namespace ERDM.Credit.Contracts.DTOs.LimitHistoryDtos
{
     public class LimitHistorySummaryItemDto
    {
        public string LimitHistoryId { get; set; } = string.Empty;
        public string ChangeType { get; set; } = string.Empty;
        public decimal PreviousLimit { get; set; }
        public decimal NewLimit { get; set; }
        public decimal ChangeAmount { get; set; }
        public DateTime ChangedDate { get; set; }
        public string Reason { get; set; } = string.Empty;
        public string ChangedBy { get; set; } = string.Empty;
        public bool IsTemporary { get; set; }
        public DateTime? ExpiryDate { get; set; }
    }

}