namespace MembershipManagement.Models.Request
{
    public class GetMemberListReq
    {
        public int PageNumber { get; set;}
        public int ItemPerPage { get; set;}
    }
    public class GetMemberReq
    {
        public string? MemberId{ get; set; }
        public string? Email{ get; set; }
        public string? Password{ get; set; }

    }
}
