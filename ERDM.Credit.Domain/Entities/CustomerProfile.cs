namespace ERDM.Credit.Domain.Entities
{

    public class CustomerProfile
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }

        // ✅ Match DTO name
        public string PhoneNumber { get; set; }

        public DateTime DateOfBirth { get; set; }
        public string NationalId { get; set; }

        public Address ResidentialAddress { get; set; }
        public Employment Employment { get; set; }
        public FinancialProfile FinancialProfile { get; set; }

        public string MaritalStatus { get; set; }
        public int Dependents { get; set; }
        public string EducationLevel { get; set; }
        public string Citizenship { get; set; }

        public string IdVerificationStatus { get; set; }
        public DateTime? IdVerificationDate { get; set; }
        public string IdVerifiedBy { get; set; }
    }
}
