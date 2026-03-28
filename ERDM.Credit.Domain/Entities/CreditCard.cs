using MongoDB.Bson.Serialization.Attributes;

namespace ERDM.Credit.Domain.Entities
{
    public class CreditCard
    {
        [BsonElement("issuer")]
        public string Issuer { get; set; }

        [BsonElement("creditLimit")]
        public decimal CreditLimit { get; set; }

        [BsonElement("currentBalance")]
        public decimal CurrentBalance { get; set; }

        [BsonElement("paymentAmount")]
        public decimal PaymentAmount { get; set; }

        [BsonElement("interestRate")]
        public decimal InterestRate { get; set; }

        [BsonElement("openedDate")]
        public DateTime OpenedDate { get; set; }

        public decimal GetUtilizationRate()
        {
            if (CreditLimit <= 0) return 0;
            return (CurrentBalance / CreditLimit) * 100;
        }
    }
}
