using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERDM.Credit.Contracts.DTOs
{
    public class CreditApplicationRequestDto
    {
        /// <summary>
        /// ID of the customer applying for credit
        /// </summary>
        public string CustomerId { get; set; }

        /// <summary>
        /// Complete customer profile information
        /// </summary>
        public CustomerProfileDto CustomerProfile { get; set; }

        /// <summary>
        /// Type of credit product (e.g., "PERSONAL_LOAN", "CREDIT_CARD", "MORTGAGE")
        /// </summary>
        public string ProductType { get; set; }

        /// <summary>
        /// Amount requested by the customer
        /// </summary>
        public decimal RequestedAmount { get; set; }

        /// <summary>
        /// Term length in months (e.g., 12, 24, 36)
        /// </summary>
        public int RequestedTerm { get; set; }

        /// <summary>
        /// Application data including source, IP, form data, etc.
        /// </summary>
        public ApplicationDataDto ApplicationData { get; set; }

        /// <summary>
        /// Additional metadata about the application (campaign, channel, etc.)
        /// </summary>
        public ApplicationMetadataDto Metadata { get; set; }
    }
}
