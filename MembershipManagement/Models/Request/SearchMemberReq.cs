namespace MembershipManagement.Models.Request
{
    public class SearchMemberReq
    {
        public string FullName { get; set; }
        public int PageNo { get; set; }
        public int PageSize { get; set; }
    }
}
