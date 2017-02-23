using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Api.Extensions;
using Api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/status")]
    public class StatusController : Controller
    {
        private readonly IAppSettings _appsettings;

        public StatusController(IAppSettings settings)
        {
            _appsettings = settings;
        }

        [HttpGet]
        [Route("")]
        public string Get()
        {
            return "Service Looking Good!";
        }

        [Authorize]
        [HttpGet]
        [Route("secure")]
        public string GetSecured()
        {
            return "All good. You only get this message if you are authenticated.";
        }



        [Authorize]
        [HttpGet]
        [Route("secure/user/identity")]
        public string GetSecuredUserIdentity()
        {
            return User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
        }

        [Authorize]
        [HttpGet]
        [Route("secure/user/claims")]
        public string GetSecuredUserClaims()
        {
            return string.Join(",", User.Claims.Select(c => $"{c.Type}:{c.Value}"));
        }

        [Authorize]
        [HttpGet]
        [Route("secure/user/claim/{name}")]
        public string GetSecuredUserClaim(string name)
        {
            return User.Claims.FirstOrDefault(c => c.Type == name)?.Value;
        }

        [Authorize]
        [HttpGet]
        [Route("secure/user/auth0")]
        public async Task<Auth0User> GetSecuredUser()
        {
            return await WebService.Request<Auth0User>(RequestType.Get, $"{_appsettings.Auth0_Domain}userinfo", token: Request.Headers["Authorization"]);
        }

        [Authorize]
        [HttpGet]
        [Route("secure/auth/domain")]
        public string GetAuth0Domain()
        {
            return _appsettings.Auth0_Domain;
        }

        [HttpGet]
        [Route("environment")]
        public string GetEnvironment()
        {
            return Environment.GetEnvironmentVariable("APP_ENVIRONMENT") ?? "Development";
        }

        //[Authorize]
        //[HttpGet]
        //[Route("status/secure")]
        //public string GetSecured()
        //{
        //    var claims = string.Join(",", User.Claims.Select(c => $"{c.Type}:{c.Value}"));
        //    return $"Hello, {claims}! You are currently authenticated.";
        //}

        [Authorize]
        [HttpGet]
        [Route("db")]
        public string GetDBStatus()
        {
            //var result = await ValidateMember();
            //return result != null ? $"Hey, {result.UserName}! DB Looking Good!": "User not found.";
            return "DB Looking Good";
        }
    }
}
