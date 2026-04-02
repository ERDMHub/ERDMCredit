using ERDM.Core.Entities;
using ERDM.Credit.Domain.Entities;


namespace ERDM.Credit.Domain.DomainEvents
{
    // Event raised for bulk rule operations
    public class UnderwritingRuleBulkOperationEvent : DomainEventBase
    {
        public UnderwritingRuleBulkOperationEvent(List<string> ruleIds, string operation, string performedBy, int successCount, int failureCount)
        {
            EntityId = Guid.NewGuid().ToString();
            EntityType = nameof(UnderwritingRule);
            RuleIds = ruleIds;
            Operation = operation; // Activate, Deactivate, Delete
            PerformedBy = performedBy;
            SuccessCount = successCount;
            FailureCount = failureCount;
            OperationDate = DateTime.UtcNow;
        }

        public List<string> RuleIds { get; }
        public string Operation { get; }
        public string PerformedBy { get; }
        public int SuccessCount { get; }
        public int FailureCount { get; }
        public DateTime OperationDate { get; }
    }

}
