using System.Linq;
using System.Security.Claims;
using Api.DataStore;
using Api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/member")]
    public class MemberController : BaseController
    {
        public MemberController(IDatabase db) : base(db) {}

        [Authorize]
        [HttpGet]
        [Route("")]
        public string GetMemberName()
        {
            return $"Welcome, {GetCurrentMember().UserName}!";
        }
    
        [Authorize]
        [HttpGet]
        [Route("identity")]
        public string GetIdentity()
        {
            return User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
        }

        [Authorize]
        [HttpGet]
        [Route("token")]
        public string GetToken()
        {
            return Request.Headers["Authorization"].ToString().Substring("Bearer".Length).Trim();
        }

        [Authorize]
        [HttpGet]
        [Route("claims")]
        public string GetClaims()
        {
            return string.Join(",", User.Claims.Select(c => $"{c.Type}:{c.Value}"));
        }

        [Authorize]
        [HttpGet]
        [Route("claim/{name}")]
        public string GetClaim(string name)
        {
            return User.Claims.FirstOrDefault(c => c.Type == name)?.Value;
        }

        [Authorize]
        [HttpPost]
        [Route("")]
        public string Post([FromBody] Auth0User data)
        {
            if (GetCurrentMember() != null)
            {
                var currentMember = GetCurrentMember();
                currentMember.LoginID = data.user_id;
                currentMember.UserName = data.nickname;
                DB.Update(currentMember, currentMember.MemberID, new[] {nameof(Member.LoginID), nameof(Member.UserName)});
            }
            else
            {
                DB.Insert(new Member
                    {
                        LoginID = data.user_id,
                        UserName = data.nickname
                    }, 0);
            }


            return $"Welcome, {data.nickname}";
        }
    }
}
