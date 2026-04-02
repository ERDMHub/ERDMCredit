using System;
using System.Collections.Generic;

namespace ERDM.Credit.Contracts.DTOs.LimitHistoryDtos
{
    public class LimitHistoryStatisticsDto
    {
        public int TotalChanges { get; set; }
        public int Increases { get; set; }
        public int Decreases { get; set; }
        public int TemporaryChanges { get; set; }
        public int PermanentChanges { get; set; }
        public decimal TotalIncreaseAmount { get; set; }
        public decimal TotalDecreaseAmount { get; set; }
        public decimal AverageIncreasePercentage { get; set; }
        public decimal AverageDecreasePercentage { get; set; }
        public Dictionary<string, int> ChangesByReason { get; set; } = new();
        public Dictionary<string, int> ChangesByType { get; set; } = new();
        public Dictionary<string, int> MonthlyTrend { get; set; } = new();
        public DateTime AsOfDate { get; set; }
    }

}