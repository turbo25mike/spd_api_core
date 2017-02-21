using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Api.DataContext;
using Api.Models;

namespace Api.Controllers
{
    [Route("api/status")]
    public class StatusController : Controller
    {
        private readonly IAppSettings _settings;

        public StatusController(IAppSettings settings)
        {
            _settings = settings;
        }

        [HttpGet]
        [Route("")]
        public string Get()
        {
            return "Looking Good!";
        }

        [Authorize]
        [HttpGet]
        [Route("secure")]
        public string GetSecured()
        {
            return "All good. You only get this message if you are authenticated.";
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

        [HttpGet]
        [Route("db")]
        public string GetDBStatus()
        {
            new MemberContext(_settings).GetAdmin();
            return "Looking Good!";
        }
    }
}
