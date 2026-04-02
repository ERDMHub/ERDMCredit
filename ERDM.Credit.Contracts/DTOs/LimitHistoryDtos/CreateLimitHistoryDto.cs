using System;
using System.Collections.Generic;

namespace ERDM.Credit.Contracts.DTOs.LimitHistoryDtos
{
    public class CreateLimitHistoryDto
    {
        public string CustomerId { get; set; } = string.Empty;
        public string AccountId { get; set; } = string.Empty;
        public string AccountNumber { get; set; } = string.Empty;
        public string ChangeType { get; set; } = string.Empty;
        public decimal PreviousLimit { get; set; }
        public decimal NewLimit { get; set; }
        public string Reason { get; set; } = string.Empty;
        public string ReasonCode { get; set; } = string.Empty;
        public string ChangedBy { get; set; } = string.Empty;
        public DateTime EffectiveDate { get; set; }
        public bool IsTemporary { get; set; }
        public int? TemporaryDurationDays { get; set; }
        public string? ApprovalReference { get; set; }
        public string? RelatedDecisionId { get; set; }
        public string? RelatedApplicationId { get; set; }
        public string? Notes { get; set; }
        public List<string>? Tags { get; set; }
    }
}