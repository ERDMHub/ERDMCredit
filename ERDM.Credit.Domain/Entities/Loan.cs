using MongoDB.Bson.Serialization.Attributes;

namespace ERDM.Credit.Domain.Entities
{
    public class Loan
    {
        [BsonElement("loanType")]
        public string LoanType { get; set; } 

        [BsonElement("originalAmount")]
        public decimal OriginalAmount { get; set; }

        [BsonElement("currentBalance")]
        public decimal CurrentBalance { get; set; }

        [BsonElement("monthlyPayment")]
        public decimal MonthlyPayment { get; set; }

        [BsonElement("interestRate")]
        public decimal InterestRate { get; set; }

        [BsonElement("startDate")]
        public DateTime StartDate { get; set; }

        [BsonElement("endDate")]
        public DateTime EndDate { get; set; }

        [BsonElement("secured")]
        public bool Secured { get; set; }

        [BsonElement("collateral")]
        public string Collateral { get; set; }

        public int GetRemainingMonths()
        {
            var today = DateTime.UtcNow;
            if (today > EndDate) return 0;
            return ((EndDate.Year - today.Year) * 12) + (EndDate.Month - today.Month);
        }
    }
}
