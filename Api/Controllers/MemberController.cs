using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Api.DataContext;
using Api.Extensions;
using Api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/member")]
    public class MemberController : BaseController
    {
        public MemberController(IDatabase db, IAppSettings settings) : base(db, settings) { }

        [Authorize]
        [HttpGet]
        [Route("")]
        public async Task<string> GetMemberName()
        {
            var member = await GetCurrentMember();
            return $"Welcome, {member.UserName}!";
        }

        [Authorize]
        [HttpGet]
        [Route("identity")]
        public string GetSecuredUserIdentity()
        {
            return User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
        }

        [Authorize]
        [HttpGet]
        [Route("claims")]
        public string GetSecuredUserClaims()
        {
            return string.Join(",", User.Claims.Select(c => $"{c.Type}:{c.Value}"));
        }

        [Authorize]
        [HttpGet]
        [Route("claim/{name}")]
        public string GetSecuredUserClaim(string name)
        {
            return User.Claims.FirstOrDefault(c => c.Type == name)?.Value;
        }

        [Authorize]
        [HttpGet]
        [Route("auth0")]
        public async Task<Auth0User> GetSecuredUser()
        {
            return await WebService.Request<Auth0User>(RequestType.Get, $"{Appsettings.Auth0_Domain}userinfo", token: Request.Headers["Authorization"]);
        }
    }
}
