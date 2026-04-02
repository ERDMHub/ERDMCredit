using System;
using System.Collections.Generic;

namespace ERDM.Credit.Contracts.DTOs.RiskScoreDtos
{
     public class InvalidateRiskScoreDto
    {
        public string InvalidatedBy { get; set; } = string.Empty;
        public string Reason { get; set; } = string.Empty;
    }
}