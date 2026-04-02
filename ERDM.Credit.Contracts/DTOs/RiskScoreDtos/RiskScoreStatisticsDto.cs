using System;
using System.Collections.Generic;

namespace ERDM.Credit.Contracts.DTOs.RiskScoreDtos
{
        public class RiskScoreStatisticsDto
    {
        public int TotalScores { get; set; }
        public int ActiveScores { get; set; }
        public int ExpiredScores { get; set; }
        public Dictionary<string, int> ScoresByCategory { get; set; } = new();
        public Dictionary<string, int> ScoresByType { get; set; } = new();
        public decimal AverageScore { get; set; }
        public int MinScore { get; set; }
        public int MaxScore { get; set; }
        public decimal AverageProbabilityOfDefault { get; set; }
        public decimal TotalExpectedLoss { get; set; }
        public Dictionary<string, int> ScoreDistribution { get; set; } = new();
        public List<RiskTrendDto> ScoreTrend { get; set; } = new();
        public DateTime AsOfDate { get; set; }
    }

}