using System;
using System.Collections.Generic;

namespace ERDM.Credit.Contracts.DTOs.LimitHistoryDtos
{
        public class LimitHistoryQueryDto
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string? CustomerId { get; set; }
        public string? AccountId { get; set; }
        public string? ChangeType { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public string? ChangedBy { get; set; }
        public bool? IsTemporary { get; set; }
        public string SortBy { get; set; } = "ChangedDate";
        public bool SortDescending { get; set; } = true;
    }
}