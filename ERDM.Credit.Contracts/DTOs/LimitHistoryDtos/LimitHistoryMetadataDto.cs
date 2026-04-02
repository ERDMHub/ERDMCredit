using System;
using System.Collections.Generic;

namespace ERDM.Credit.Contracts.DTOs.LimitHistoryDtos
{
        public class LimitHistoryMetadataDto
    {
        public string? Source { get; set; }
        public string? IpAddress { get; set; }
        public string? UserAgent { get; set; }
        public string? WorkflowId { get; set; }
        public Dictionary<string, object>? AdditionalData { get; set; }
    }
}