
using MembershipManagement.Models.Request;
using MembershipManagement.Models.Response;

namespace MembershipManagement.Service
{
    public interface IMemberCommand
    {
        Task<ResponseData> LoginMember(LoginReq request);
        Task<ResponseData> saveMemberDetails(SaveMemberDetailRequest saveMemberDetail);
        string ValidateMemberDetails(SaveMemberDetailRequest saveMemberDetail, bool isUpdate);
        Task<ResponseData> UpdateMemberDetails(SaveMemberDetailRequest saveMemberDetail);
        Task<ResponseData> searchMember(SearchMemberReq searchMember);
    }
}
