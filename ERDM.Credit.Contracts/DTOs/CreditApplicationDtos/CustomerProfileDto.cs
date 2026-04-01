using ERDM.Credit.Contracts.DTOs.AccountDtos;

namespace ERDM.Credit.Contracts.DTOs.CreditApplicationDtos
{
    public class CustomerProfileDto
    {
        public CustomerProfileDto()
        {
            
        }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string NationalId { get; set; }
        public AddressDto ResidentialAddress { get; set; }
        public EmploymentDto Employment { get; set; }
        public FinancialProfileDto FinancialProfile { get; set; }
        public string MaritalStatus { get; set; }
        public int Dependents { get; set; }
        public string EducationLevel { get; set; }
        public string Citizenship { get; set; }
        public string IdVerificationStatus { get; set; }
        public DateTime? IdVerificationDate { get; set; }
        public string IdVerifiedBy { get; set; }
    }
}
