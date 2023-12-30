namespace MembershipManagement.Models.Request
{
    public class SaveMemberDetailRequest
    {
        public string? MemberId { get; set; }
        public string FullName { get; set; }

        public string DOB { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public string MobileNumber { get; set; }

        public string? OtherNumber { get; set; }

        public string Gender { get; set; }

        public string Address { get; set; }

        public string StartDate { get; set; }

        public string EndDate { get; set; }

        public string? PackageDetails { get; set; }

        public string PurchaseDate { get; set; }

        public string PurchaseBy { get; set; }
    }
}
