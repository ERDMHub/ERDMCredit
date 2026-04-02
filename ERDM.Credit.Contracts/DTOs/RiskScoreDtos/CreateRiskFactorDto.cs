using System;
using System.Collections.Generic;

namespace ERDM.Credit.Contracts.DTOs.RiskScoreDtos
{
    
    public class CreateRiskFactorDto
    {
        public string FactorName { get; set; } = string.Empty;
        public string FactorValue { get; set; } = string.Empty;
        public decimal Weight { get; set; }
        public string Impact { get; set; } = string.Empty;
    }
}