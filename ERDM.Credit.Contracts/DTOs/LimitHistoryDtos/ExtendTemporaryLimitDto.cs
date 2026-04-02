using System;
using System.Collections.Generic;

namespace ERDM.Credit.Contracts.DTOs.LimitHistoryDtos
{
      public class ExtendTemporaryLimitDto
    {
        public DateTime NewExpiryDate { get; set; }
        public string ExtendedBy { get; set; } = string.Empty;
        public string? Reason { get; set; }
    }
}