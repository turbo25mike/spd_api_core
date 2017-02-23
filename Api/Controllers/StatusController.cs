using System;
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
        [Route("secure/user")]
        public async Task<string> GetSecuredUser()
        {
            var user = await WebService.Request<Auth0User>(RequestType.Get, $"{_appsettings.Auth0_Domain}userinfo", token: Request.Headers["Authorization"]);
            //var results = "User: " + user.nickname + ", Claims: " + string.Join(",", User.Claims.Select(c => $"{c.Type}:{c.Value}"));
            return user.nickname;
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
