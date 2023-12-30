using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MembershipManagement.Models.Response
{
    public class MemberListResponse
    {    
        public string MemberId { get; set; }

        public string FullName { get; set; }

        public DateTime DOB { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public string MobileNumber { get; set; }

        public string? OtherNumber { get; set; }

        public string Gender { get; set; }

        public string Address { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }
 
        public string? PackageDetails { get; set; }

        public DateTime? PurchaseDate { get; set; }

        public string PurchaseBy { get; set; }

        public string Status { get; set; }

    }
}
