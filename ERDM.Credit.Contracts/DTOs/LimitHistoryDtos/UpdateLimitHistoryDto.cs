using System;
using System.Collections.Generic;

namespace ERDM.Credit.Contracts.DTOs.LimitHistoryDtos
{
     public class UpdateLimitHistoryDto
    {
        public string? Notes { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public List<string>? Tags { get; set; }
    }
}