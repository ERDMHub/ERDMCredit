using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERDM.Credit.Contracts.DTOs.AccountDtos
{
    public class AccountStatusHistoryDto
    {
        /// <summary>
        /// The status of the account at this point in time
        /// </summary>
        public string Status { get; set; } = string.Empty;

        /// <summary>
        /// Date and time when the status change occurred
        /// </summary>
        public DateTime ChangedAt { get; set; }

        /// <summary>
        /// User or system that performed the status change
        /// </summary>
        public string ChangedBy { get; set; } = string.Empty;

        /// <summary>
        /// Primary reason for the status change
        /// </summary>
        public string Reason { get; set; } = string.Empty;

        /// <summary>
        /// Additional comments or notes about the status change
        /// </summary>
        public string? Comments { get; set; }

        /// <summary>
        /// Previous status before the change (for tracking transitions)
        /// </summary>
        public string? PreviousStatus { get; set; }

        /// <summary>
        /// Reference ID to related event or document (e.g., approval reference, write-off approval)
        /// </summary>
        public string? ReferenceId { get; set; }

        /// <summary>
        /// Additional metadata about the status change
        /// </summary>
        public Dictionary<string, object>? Metadata { get; set; }
    }

}
