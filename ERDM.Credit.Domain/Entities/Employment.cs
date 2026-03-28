using MongoDB.Bson.Serialization.Attributes;

namespace ERDM.Credit.Domain.Entities
{
    public class Employment
    {
        [BsonElement("employerName")]
        public string EmployerName { get; set; }

        [BsonElement("jobTitle")]
        public string JobTitle { get; set; }

        [BsonElement("monthlyIncome")]
        public decimal MonthlyIncome { get; set; }

        [BsonElement("yearsEmployed")]
        public int YearsEmployed { get; set; }

        [BsonElement("monthsEmployed")]
        public int MonthsEmployed { get; set; }

        [BsonElement("employmentStatus")]
        public string EmploymentStatus { get; set; }

        [BsonElement("employerPhone")]
        public string EmployerPhone { get; set; }

        [BsonElement("employerAddress")]
        public Address EmployerAddress { get; set; }

        [BsonElement("incomeType")]
        public string IncomeType { get; set; } 

        [BsonElement("additionalIncome")]
        public decimal AdditionalIncome { get; set; }

        [BsonElement("incomeVerified")]
        public bool IncomeVerified { get; set; }

        [BsonElement("verificationDate")]
        public DateTime? VerificationDate { get; set; }

        [BsonElement("verifiedBy")]
        public string VerifiedBy { get; set; }

        public Employment()
        {
            EmploymentStatus = "FullTime";
            IncomeType = "Salary";
            IncomeVerified = false;
            AdditionalIncome = 0;
        }

        public decimal GetTotalMonthlyIncome()
        {
            return MonthlyIncome + AdditionalIncome;
        }

        public decimal GetAnnualIncome()
        {
            return GetTotalMonthlyIncome() * 12;
        }

        public bool IsStablyEmployed()
        {
            return YearsEmployed >= 2 || (YearsEmployed >= 1 && MonthsEmployed >= 6);
        }
    }
}
