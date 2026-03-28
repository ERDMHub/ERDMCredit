using MongoDB.Bson.Serialization.Attributes;

namespace ERDM.Credit.Domain.Entities
{
    public class Address
    {
        [BsonElement("street")]
        public string Street { get; set; }

        [BsonElement("city")]
        public string City { get; set; }

        [BsonElement("state")]
        public string State { get; set; }

        [BsonElement("postalCode")]
        public string PostalCode { get; set; }

        [BsonElement("country")]
        public string Country { get; set; }

        [BsonElement("addressType")]
        public string AddressType { get; set; }

        [BsonElement("isPrimary")]
        public bool IsPrimary { get; set; }

        [BsonElement("yearsAtAddress")]
        public int? YearsAtAddress { get; set; }

        [BsonElement("monthsAtAddress")]
        public int? MonthsAtAddress { get; set; }

        public Address()
        {
            IsPrimary = false;
        }

        public string GetFullAddress()
        {
            return $"{Street}, {City}, {State} {PostalCode}, {Country}";
        }
    }
}
