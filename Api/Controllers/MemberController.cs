using System.Threading.Tasks;
using Api.DataStore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/member")]
    public class MemberController : BaseController
    {
        public MemberController(IDatabase db, IAppSettings settings) : base(db, settings) {}

        [Authorize]
        [HttpGet]
        [Route("")]
        public async Task<Member> GetMemberName()
        {
            return await GetCurrentMember();
        }

        //[Authorize]
        //[HttpGet]
        //[Route("identity")]
        //public string GetIdentity()
        //{
        //    return User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
        //}

        //[Authorize]
        //[HttpGet]
        //[Route("token")]
        //public string GetToken()
        //{
        //    return Request.Headers["Authorization"].ToString().Substring("Bearer".Length).Trim();
        //}

        //[Authorize]
        //[HttpGet]
        //[Route("claims")]
        //public string GetClaims()
        //{
        //    return string.Join(",", User.Claims.Select(c => $"{c.Type}:{c.Value}"));
        //}

        //[Authorize]
        //[HttpGet]
        //[Route("claim/{name}")]
        //public string GetClaim(string name)
        //{
        //    return User.Claims.FirstOrDefault(c => c.Type == name)?.Value;
        //}

        //[Authorize]
        //[HttpPost]
        //[Route("")]
        //public async void Post([FromBody] Auth0User data)
        //{
        //    var currentMember = await GetCurrentMember() ?? new Member();
        //    currentMember.LoginID = data.user_id;
        //    currentMember.Nickname = data.nickname;
        //    currentMember.Picture = data.picture;
        //    currentMember.Name = data.name;
        //    currentMember.GivenName = data.given_name;
        //    currentMember.FamilyName = data.family_name;
        //    currentMember.Email = data.email;
        //    currentMember.EmailVerified = data.email_verified;

        //    DB.Execute(currentMember.MemberID > 0 ? MemberScripts.Insert : MemberScripts.Update, currentMember);
        //}
    }
}
