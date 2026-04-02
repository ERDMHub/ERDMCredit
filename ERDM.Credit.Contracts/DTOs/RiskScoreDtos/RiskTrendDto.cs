using System;
using System.Collections.Generic;

namespace ERDM.Credit.Contracts.DTOs.RiskScoreDtos
{
    public class RiskTrendDto
    {
        public DateTime Date { get; set; }
        public int AverageScore { get; set; }
        public int ScoreCount { get; set; }
        public decimal AverageProbabilityOfDefault { get; set; }
    }
}