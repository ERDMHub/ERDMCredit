using ERDM.Core.Entities;
using ERDM.Core.Exceptions;
using ERDM.Credit.Domain.DomainEvents;
using MongoDB.Bson.Serialization.Attributes;

namespace ERDM.Credit.Domain.Entities
{
    public class CreditApplication : BaseEntity
    {
        [BsonElement("applicationId")]
        public string ApplicationId { get; private set; }

        [BsonElement("customerId")]
        public string CustomerId { get; private set; }

        [BsonElement("customerProfile")]
        public CustomerProfile CustomerProfile { get; private set; }

        [BsonElement("productType")]
        public string ProductType { get; private set; }

        [BsonElement("requestedAmount")]
        public decimal RequestedAmount { get; private set; }

        [BsonElement("requestedTerm")]
        public int RequestedTerm { get; private set; }

        [BsonElement("status")]
        public string Status { get; private set; }

        [BsonElement("decision")]
        public Decision Decision { get; private set; }

        [BsonElement("applicationData")]
        public ApplicationData ApplicationData { get; private set; }

        [BsonElement("creditBureauData")]
        public CreditBureauData CreditBureauData { get; private set; }

        [BsonElement("fraudCheck")]
        public FraudCheck FraudCheck { get; private set; }

        [BsonElement("underwritingHistory")]
        public List<UnderwritingStage> UnderwritingHistory { get; private set; }

        [BsonElement("documents")]
        public List<Document> Documents { get; private set; }

        [BsonElement("metadata")]
        public ApplicationMetadata Metadata { get; private set; }

        [BsonElement("expiresAt")]
        public DateTime? ExpiresAt { get; private set; }

        public CreditApplication()
        {
            UnderwritingHistory = new List<UnderwritingStage>();
            Documents = new List<Document>();
        }

        public CreditApplication(
            string customerId,
            CustomerProfile customerProfile,
            string productType,
            decimal requestedAmount,
            int requestedTerm,
            ApplicationData applicationData,
            ApplicationMetadata metadata) : this()
        {
            ApplicationId = GenerateApplicationId();
            CustomerId = customerId;
            CustomerProfile = customerProfile;
            ProductType = productType;
            RequestedAmount = requestedAmount;
            RequestedTerm = requestedTerm;
            ApplicationData = applicationData;
            Metadata = metadata;
            Status = "PENDING";
            ExpiresAt = DateTime.UtcNow.AddDays(30);
            MarkAsCreated("system");

            AddUnderwritingHistory("INITIATION", "COMPLETED", "Application created");
        }

        public void Submit()
        {
            if (Status != "PENDING")
                throw new BusinessRuleException("Submit", "Only pending applications can be submitted");

            Status = "SUBMITTED";
            AddUnderwritingHistory("SUBMISSION", "COMPLETED", "Application submitted");
            AddDomainEvent(new CreditApplicationSubmittedEvent(this));
        }

        public void StartUnderwriting()
        {
            if (Status != "SUBMITTED")
                throw new BusinessRuleException("StartUnderwriting", "Only submitted applications can start underwriting");

            Status = "UNDERWRITING";
            AddUnderwritingHistory("UNDERWRITING_START", "COMPLETED", "Underwriting started");
        }

        public void AddCreditBureauData(string bureau, int score, DateTime scoreDate, string inquiryId)
        {
            CreditBureauData = new CreditBureauData
            {
                Bureau = bureau,
                Score = score,
                ScoreDate = scoreDate,
                InquiryId = inquiryId
            };
            AddUnderwritingHistory("CREDIT_BUREAU", "COMPLETED", $"Credit score: {score}");
        }

        public void Approve(decimal approvedAmount, decimal interestRate, string riskGrade,
            List<string> reasonCodes, string decidedBy)
        {
            if (Status != "UNDERWRITING")
                throw new BusinessRuleException("Approve", "Only applications under underwriting can be approved");

            Decision = new Decision
            {
                Status = "APPROVED",
                ApprovedAmount = approvedAmount,
                InterestRate = interestRate,
                RiskGrade = riskGrade,
                ReasonCodes = reasonCodes,
                DecidedBy = decidedBy,
                DecidedAt = DateTime.UtcNow
            };

            Status = "APPROVED";
            ExpiresAt = null;
            AddUnderwritingHistory("UNDERWRITING", "COMPLETED", $"Approved for {approvedAmount:C}");
            AddDomainEvent(new CreditApplicationApprovedEvent(this));
        }

        public void Decline(List<string> declineReasons, string decidedBy)
        {
            if (Status != "UNDERWRITING")
                throw new BusinessRuleException("Decline", "Only applications under underwriting can be declined");

            Decision = new Decision
            {
                Status = "DECLINED",
                DeclineReasons = declineReasons,
                DecidedBy = decidedBy,
                DecidedAt = DateTime.UtcNow
            };

            Status = "DECLINED";
            AddUnderwritingHistory("UNDERWRITING", "COMPLETED", $"Declined: {string.Join(", ", declineReasons)}");
            AddDomainEvent(new CreditApplicationDeclinedEvent(this));
        }

        public void AddDocument(string documentId, string type, string uploadedBy)
        {
            Documents.Add(new Document
            {
                DocumentId = documentId,
                Type = type,
                UploadedAt = DateTime.UtcNow,
                UploadedBy = uploadedBy,
                Verified = false
            });
            AddUnderwritingHistory("DOCUMENT_UPLOAD", "COMPLETED", $"Document {type} uploaded");
        }

        private void AddUnderwritingHistory(string stage, string status, string result)
        {
            UnderwritingHistory.Add(new UnderwritingStage
            {
                Stage = stage,
                Status = status,
                StartedAt = DateTime.UtcNow,
                CompletedAt = DateTime.UtcNow,
                Result = result
            });
        }

        private string GenerateApplicationId()
        {
            return $"APP-{DateTime.Now:yyyyMMdd}-{Guid.NewGuid().ToString().Substring(0, 8).ToUpper()}";
        }
    }
}
