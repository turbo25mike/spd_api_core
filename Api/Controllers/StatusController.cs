using System;
using System.Threading.Tasks;
using Api.DataContext;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/status")]
    public class StatusController : BaseController
    {
        public StatusController(IDatabase db, IAppSettings settings) : base(db, settings){}

        [HttpGet]
        [Route("")]
        public string Get()
        {
            return "Service Looking Good!";
        }

        [Authorize]
        [HttpGet]
        [Route("secure/auth/domain")]
        public string GetAuth0Domain()
        {
            return Appsettings.Auth0_Domain;
        }

        [Authorize]
        [HttpGet]
        [Route("environment")]
        public string GetEnvironment()
        {
            return Environment.GetEnvironmentVariable("APP_ENVIRONMENT") ?? "Development";
        }

        [Authorize]
        [HttpGet]
        [Route("db")]
        public async Task<string> GetDBStatus()
        {
            var member = await GetCurrentMember();
            return member != null ? $"Hey, {member.UserName}! DB Looking Good!": "User not found.";
        }
    }
}
