using System;
using System.Collections.Generic;

namespace ERDM.Credit.Contracts.DTOs.LimitHistoryDtos
{
      public class ActiveTemporaryLimitDto
    {
        public string LimitHistoryId { get; set; } = string.Empty;
        public string AccountId { get; set; } = string.Empty;
        public string AccountNumber { get; set; } = string.Empty;
        public decimal TemporaryLimit { get; set; }
        public decimal OriginalLimit { get; set; }
        public DateTime EffectiveDate { get; set; }
        public DateTime ExpiryDate { get; set; }
        public int DaysRemaining { get; set; }
        public string Reason { get; set; } = string.Empty;
    }
}