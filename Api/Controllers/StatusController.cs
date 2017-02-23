using System;
using System.Linq;
using System.Threading.Tasks;
using Api.DataContext;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/status")]
    public class StatusController : BaseController
    {
        public StatusController(IDatabase db, IAppSettings settings)
        {
            _db = db;
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
        public string GetSecuredUser()
        {
            var results = "Claims: " + string.Join(",", User.Claims.Select(c => $"{c.Type}:{c.Value}"));
            return results;
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
        public async Task<string> GetDBStatus()
        {
            var result = await ValidateMember();
            return result != null ? $"Hey, {result.UserName}! DB Looking Good!": "User not found.";
        }
    }
}
