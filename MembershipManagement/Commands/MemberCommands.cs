
using MembershipManagement.DBContext;
using MembershipManagement.Models.DTO;
using MembershipManagement.Models.Request;
using MembershipManagement.Models.Response;
using MembershipManagement.Service;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MembershipManagement.Commands
{
    public class MemberCommands : IMemberCommand
    {
        private readonly memberDetailsContext _memberDetailsContext;
        private readonly IConfiguration _configuration;
        public MemberCommands(memberDetailsContext memberDetailsContext, IConfiguration configuration)
        {
            _memberDetailsContext = memberDetailsContext;
            _configuration = configuration;
        }

        public async Task<ResponseData> LoginMember(LoginReq request)
        {
            ResponseData response = new ResponseData();
            try
            {
                var memberDetails = _memberDetailsContext.MemberDetails.Where(x => x.Email == request.Email).FirstOrDefault();
                string adminEmai = "admin@superiorholiday.in";
                string adminPass = "Admin@123";
                if (memberDetails != null)
                {
                    if (memberDetails.Password == request.Password)
                    {

                        response.Success = true;
                        response.Message = "Login Success";
                        response.Data = CreateToken(memberDetails);

                    }
                    else
                    {
                        response.Success = false;
                        response.Message = "Please Enter Valid Password";
                    }
                }
                else if(adminEmai == request.Email && adminPass==request.Password)
                {
                    response.Success = true;
                    response.Message = "Admin";
                    response.Data = CreateToken(memberDetails);

                }
                else
                {
                    response.Success = false;
                    response.Message = "Not found";
                    response.Data = null;
                }

            }
            catch (Exception e)
            {
                response.Message += e.ToString();

            }
            return response;
        }

        public string ValidateMemberDetails(SaveMemberDetailRequest saveMemberDetail, bool isUpdate)
        {
            StringBuilder stringBuilder = new StringBuilder();
            if (isUpdate)
            {
                if (string.IsNullOrEmpty(saveMemberDetail.MemberId))
                    stringBuilder.Append("Enter MemberId");
            }
            if (string.IsNullOrEmpty(saveMemberDetail.FullName))
                stringBuilder.Append("Enter FullName");
            // string dob = saveMemberDetail.DOB.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
            if (string.IsNullOrEmpty(saveMemberDetail.DOB.ToString()))
                stringBuilder.Append("Enter FullName");
            // string startDate = saveMemberDetail.StartDate.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
            if (string.IsNullOrEmpty(saveMemberDetail.StartDate.ToString()))
                stringBuilder.Append("Enter StartDate");
            // string endDate = saveMemberDetail.EndDate.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
            if (string.IsNullOrEmpty(saveMemberDetail.EndDate.ToString()))
                stringBuilder.Append("Enter EndDate");
            if (string.IsNullOrEmpty(saveMemberDetail.Email))
                stringBuilder.Append("Enter Email");
            if (string.IsNullOrEmpty(saveMemberDetail.Password))
                stringBuilder.Append("Enter Password");
            if (string.IsNullOrEmpty(saveMemberDetail.MobileNumber))
                stringBuilder.Append("Enter MobileNumber");
            if (string.IsNullOrEmpty(saveMemberDetail.Gender))
                stringBuilder.Append("Enter Gender");
            if (string.IsNullOrEmpty(saveMemberDetail.Address))
                stringBuilder.Append("Enter Address");
            if (string.IsNullOrEmpty(saveMemberDetail.PackageDetails))
                stringBuilder.Append("Enter PackageDetails");
            if (string.IsNullOrEmpty(saveMemberDetail.PurchaseBy))
                stringBuilder.Append("Enter ReferalDetails");


            return stringBuilder.ToString();
        }

        public async Task<ResponseData> saveMemberDetails(SaveMemberDetailRequest saveMemberDetail)
        {
            ResponseData response = new ResponseData();
            SaveMemberDetailResponse saveMemberDetailResponse = new SaveMemberDetailResponse();
            try
            {
                var checkExisting = await _memberDetailsContext.MemberDetails.Where(x => x.Email == saveMemberDetail.Email).FirstOrDefaultAsync();
                if (checkExisting != null)
                {
                    response.Success = true;
                    response.Message = "User Already Exist";
                }
                
                else if (saveMemberDetail != null)
                {
                    var memberIDGen = await _memberDetailsContext.MemberDetails.OrderByDescending(x => x.MemberId).Select(x => x.MemberId).FirstOrDefaultAsync();
                    string memberIdGen2 = memberIDGen;
                    string[] memberGenrator = memberIdGen2.Split("M");
                    string vhjv = memberGenrator[1];
                    int x = Convert.ToInt32(vhjv) + 1;
                    
                    memberDetails memberDetails = new memberDetails();

                    //Random genrator = new Random();
                    //string r = genrator.Next(0, 00).ToString();
                    //int r1 = Convert.ToInt32(r) + 1;

                    memberDetails.MemberId = "M" + x;
                    memberDetails.FullName = saveMemberDetail.FullName;
                    memberDetails.DOB = Convert.ToDateTime(saveMemberDetail.DOB);
                    memberDetails.Email = saveMemberDetail.Email;
                    memberDetails.Password = saveMemberDetail.Password;
                    memberDetails.MobileNumber = saveMemberDetail.MobileNumber;
                    memberDetails.OtherNumber = saveMemberDetail.OtherNumber;
                    memberDetails.Gender = saveMemberDetail.Gender;
                    memberDetails.Address = saveMemberDetail.Address;
                    memberDetails.PackageDetails = saveMemberDetail.PackageDetails;
                    memberDetails.PurchaseBy = saveMemberDetail.PurchaseBy;
                    memberDetails.PurchaseDate = !string.IsNullOrEmpty(saveMemberDetail.PurchaseDate) ? Convert.ToDateTime(saveMemberDetail.PurchaseDate) : null;
                    memberDetails.CreatedDate = DateTime.Now;
                    memberDetails.StartDate = Convert.ToDateTime(saveMemberDetail.StartDate);
                    memberDetails.EndDate = Convert.ToDateTime(saveMemberDetail.EndDate);
                    memberDetails.Status = "Active";
                    memberDetails.IsActive = true;

                    _memberDetailsContext.Add(memberDetails);
                    _memberDetailsContext.SaveChanges();

                    saveMemberDetailResponse.MemberId = memberDetails.MemberId;
                    saveMemberDetailResponse.FullName = memberDetails.FullName;
                    saveMemberDetailResponse.Email = memberDetails.Email;
                    saveMemberDetailResponse.Password = memberDetails.Password;
                    saveMemberDetailResponse.Status = memberDetails.Status;

                    response.Success = true;
                    response.Data = saveMemberDetailResponse;
                    response.Message = "Member Added SuccessFuly";
                }
                else
                {
                    response.Success = false;
                    response.Message = "Something Went Wrong.Try again after Sometime";
                }

            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }
            return response;
        }

        public async Task<ResponseData> UpdateMemberDetails(SaveMemberDetailRequest saveMemberDetail)
        {
            ResponseData response = new ResponseData();
            try
            {
                if (saveMemberDetail != null)
                {
                    var Details = await _memberDetailsContext.MemberDetails.Where(x => x.MemberId == saveMemberDetail.MemberId).FirstOrDefaultAsync();
                    if (Details != null)
                    {
                        Details.FullName = saveMemberDetail.FullName;
                        Details.DOB = Convert.ToDateTime(saveMemberDetail.DOB);
                        Details.Email = saveMemberDetail.Email;
                        Details.Password = saveMemberDetail.Password;
                        Details.MobileNumber = saveMemberDetail.MobileNumber;
                        Details.OtherNumber = saveMemberDetail.OtherNumber;
                        Details.Gender = saveMemberDetail.Gender;
                        Details.Address = saveMemberDetail.Address;
                        Details.StartDate = Convert.ToDateTime(saveMemberDetail.StartDate);
                        Details.EndDate = Convert.ToDateTime(saveMemberDetail.EndDate);
                        Details.PackageDetails = saveMemberDetail.PackageDetails;
                        Details.PurchaseDate = Convert.ToDateTime(saveMemberDetail.PurchaseDate);
                        Details.PurchaseBy = saveMemberDetail.PurchaseBy;
                        Details.PackageDetails = saveMemberDetail.PackageDetails;
                        Details.UpdatedDate = DateTime.Now;

                        _memberDetailsContext.MemberDetails.Update(Details);
                        var success = await _memberDetailsContext.SaveChangesAsync();

                        if (success == 1)
                        {
                            response.Success = true;
                            response.Message = "Data Updated Succefully";
                        }
                        else
                        {
                            response.Success = false;
                            response.Message = "Something Went Wrong";
                        }
                    }
                    else
                    {
                        response.Success = false;
                        response.Message = "Enter Valid MemberId";
                    }
                }
                else
                {
                    response.Success = false;
                    response.Message = "Enter Valid Update Details";
                }
            }
            catch (Exception ex)
            {

                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public string CreateToken(memberDetails member)
        {
            try
            {
                var claims = new[] {
                                    new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]),
                                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                                    new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                                    new Claim("MemberId",member == null ? "ADMIN" : member.MemberId),
                                    new Claim("FullName",member == null ? "ADMIN" : member.FullName),
                                    new Claim("Email", member == null ? "admin@superiorholiday.in" : member.Email)
                                    };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
                var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                var token = new JwtSecurityToken(
                    _configuration["Jwt:Issuer"],
                    _configuration["Jwt:Audience"],
                    claims,
                    expires: DateTime.UtcNow.AddHours(Convert.ToInt16(_configuration["Jwt:ExpireTime"])),
                    signingCredentials: signIn);

                return (new JwtSecurityTokenHandler().WriteToken(token));
            }
            catch (Exception ex) { }
            return "";
        }

        public async Task<ResponseData> searchMember(SearchMemberReq searchMember)
        {
            ResponseData responseData = new ResponseData();
            try
            {
                if (searchMember.FullName != null)
                {
                    var query = _memberDetailsContext.MemberDetails.Where(x => x.FullName.Contains(searchMember.FullName));
                }
            }
            catch (Exception)
            {

                throw;
            }

            return responseData;
        }
    }
}
