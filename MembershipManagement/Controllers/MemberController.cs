using Azure;
using MembershipManagement.DBContext;
using MembershipManagement.Models.Request;
using MembershipManagement.Models.Response;
using MembershipManagement.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;


namespace MembershipManagement.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class MemberController : ControllerBase
    {
        private readonly memberDetailsContext _memberDetailsContext;
        private readonly IMemberCommand _imemberCommands;
        public MemberController(memberDetailsContext memberDetailsContext, IMemberCommand imemberCommand)
        {
            _memberDetailsContext = memberDetailsContext;
            _imemberCommands = imemberCommand;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("Login")]
        public async Task<ActionResult> Login([FromBody] LoginReq request)
        {
            ResponseData response = new ResponseData();
            try
            {
                if (request != null)
                {
                    response = await _imemberCommands.LoginMember(request);
                }
                else
                {
                    response.Success = false;
                }
            }
            catch (Exception ex)
            {
                response.Message = ex.Message.ToString();
            }
            return Ok(response);
        }

        
        [HttpPost]
        [Route("GetMemberList")]
        public async Task<ActionResult> GetMemberList([FromBody] GetMemberListReq memberListReq)
        {
            ResponseData response = new ResponseData();
            try
            {
                int CurrentPage = memberListReq.PageNumber;
                int PageSize = memberListReq.ItemPerPage;
                int SkippedElementSize = (CurrentPage - 1) * PageSize;

                var memberList = _memberDetailsContext.MemberDetails.OrderByDescending(p => p.CreatedDate).Skip(SkippedElementSize).Take(PageSize).ToList();
                //var memberList = _memberDetailsContext.MemberDetails.ToList();
                MemberListResponse memberListResponse = new MemberListResponse();
                if(memberList != null)
                {
                    response.Success = true;
                    response.Message = "Here is List";
                    response.Data = memberList;
                }
                else
                {
                    response.Success = false;
                    response.Message = "Members Not Found";
                }
            }
            catch (Exception e)
            {
                    response.Success = false;
                    response.Message = e.ToString();
            }
            return Ok(response);
        }

        [HttpPost]
        [Route("GetMember")]
        public async Task<ActionResult> GetMember([FromBody] GetMemberReq memberListReq)
        {
            ResponseData response = new ResponseData();
            try
            {
                object memberList;

                if (!string.IsNullOrEmpty(memberListReq.MemberId))
                {
                     memberList = _memberDetailsContext.MemberDetails.Where(x => x.MemberId == memberListReq.MemberId).FirstOrDefault();
                }
                else
                {
                    memberList = _memberDetailsContext.MemberDetails.Where(x => x.Email == memberListReq.Email && x.Password == memberListReq.Password).FirstOrDefault();
                }
                
                //var memberList = _memberDetailsContext.MemberDetails.ToList();
                MemberListResponse memberListResponse = new MemberListResponse();
                if (memberList != null)
                {
                    response.Success = true;
                    response.Data = memberList;
                }
                else
                {
                    response.Success = false;
                    response.Message = "Members Not Found";
                }
            }
            catch (Exception e)
            {
                response.Success = false;
                response.Message = e.ToString();
            }
            return Ok(response);
        }

        [HttpPost]
        [Route("AddMemberDetails")]
        public async Task<ActionResult> AddMemberDetails([FromBody] SaveMemberDetailRequest saveMemberDetail)
        {
                ResponseData responseData = new ResponseData();
            string errormessage = string.Empty;

            try
            {
                if(saveMemberDetail!=null)
                {
                    errormessage =  _imemberCommands.ValidateMemberDetails(saveMemberDetail,false);

                   if (string.IsNullOrEmpty(errormessage))
                   {
                        responseData = await _imemberCommands.saveMemberDetails(saveMemberDetail);
                   }
                   else
                   {
                        responseData.Message = errormessage;
                   }
                }
                else
                {
                    responseData.Success = false;
                }
            }
            catch (Exception e)
            {
                responseData.Success = false;
                responseData.Message = e.ToString();
            }
            return Ok(JsonConvert.SerializeObject(responseData));
        }
        
        [HttpPost]
        [Route("EditMemberDetails")]
        public async Task<ActionResult> EditMemberDetails([FromBody] SaveMemberDetailRequest saveMemberDetail)
        {
            ResponseData responseData = new ResponseData();
            string errormessage = string.Empty;

            try
            {
                if (saveMemberDetail != null)
                {
                    errormessage =  _imemberCommands.ValidateMemberDetails(saveMemberDetail,true);

                    if (string.IsNullOrEmpty(errormessage))
                    {
                        responseData = await _imemberCommands.UpdateMemberDetails(saveMemberDetail);
                    }
                    else
                    {
                        responseData.Message = errormessage;
                    }
                }
                else
                {
                    responseData.Success = false;
                }
            }
            catch (Exception e)
            {
                responseData.Success = false;
                responseData.Message = e.ToString();
            }

            return Ok(responseData);
        }

        [Authorize]
        [HttpPost]
        [Route("DeleteMemberByMemberId")]
        public async Task<ActionResult> DeleteMemberByMemberId(DeleteMemberReq deleteMember)
        {
            ResponseData responseData = new ResponseData();

            try
            {
                if (deleteMember!=null)
                {
                    var memberRecord = await _memberDetailsContext.MemberDetails.Where(x => x.MemberId == deleteMember.MemberId).FirstOrDefaultAsync();
                    if (memberRecord != null)
                    {
                        memberRecord.IsActive= false;
                        memberRecord.Status = "InActive";
                        _memberDetailsContext.MemberDetails.Remove(memberRecord);
                        var success =  _memberDetailsContext.SaveChanges();
                        if (success==1)
                        {
                            responseData.Success = true;
                            responseData.Message = "Data deleted Succesfully";
                        }
                        else
                        {
                            responseData.Success = false;
                            responseData.Message = "Something Went Wrong";
                        }
                    }
                    else
                    {
                        responseData.Success = false;
                        responseData.Message = "Invalid MemberId";
                    }
                }
                else
                {
                    responseData.Success = false;
                    responseData.Message = "Please Enter MemberId";
                }
            }
            catch (Exception ex)
            {
                responseData.Success = false;
                responseData.Message = ex.ToString(); 
            }
            return Ok(responseData);
        }

        [HttpPost]
        [Route("SearchMemberByFullName")]
        public async Task<ActionResult> SearchMember(SearchMemberReq searchMember)
        {
            ResponseData responseData = new ResponseData();
            try
            {
                if(searchMember!=null)
                {
                    var result = await _imemberCommands.searchMember(searchMember);
                    if (result!=null)
                    {
                        responseData.Success = true;
                        responseData.Data= result;
                    }
                    else
                    {
                        responseData.Success = false;
                        responseData.Message = "Not found";
                    }
                }
            }
            catch (Exception ex)
            {

                responseData.Success = false;
                responseData.Message = ex.ToString();
            }
            return Ok(responseData);
        }
    }
}

