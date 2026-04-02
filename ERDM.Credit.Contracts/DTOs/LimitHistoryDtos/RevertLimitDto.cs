using System;
using System.Collections.Generic;

namespace ERDM.Credit.Contracts.DTOs.LimitHistoryDtos
{
     public class RevertLimitDto
    {
        public decimal RevertedLimit { get; set; }
        public string RevertedBy { get; set; } = string.Empty;
        public string RevertReason { get; set; } = string.Empty;
        public string? ApprovalReference { get; set; }
    }
}