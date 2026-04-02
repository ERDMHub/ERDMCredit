using MongoDB.Bson.Serialization.Attributes;

namespace ERDM.Credit.Domain.Entities
{
     public class LimitHistoryFilter
    {
        public string? CustomerId { get; set; }
        public string? AccountId { get; set; }
        public LimitChangeType? ChangeType { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public string? ChangedBy { get; set; }
        public bool? IsTemporary { get; set; }
        public bool? IsExpired { get; set; }
    }
}