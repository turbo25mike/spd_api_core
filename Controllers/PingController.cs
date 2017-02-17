using System;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAPIApplication.Controllers
{
    [Route("api")]
    public class PingController : Controller
    {
        [Authorize]
        [HttpGet]
        [Route("ping/secure")]
        public string PingSecured()
        {
            return "All good. You only get this message if you are authenticated.";
        }

        [HttpGet]
        [Route("status")]
        public string Get()
        {
            return "Looking Good!";
        }

        [HttpGet]
        [Route("status/environment")]
        public string GetEnvironment()
        {
            return Environment.GetEnvironmentVariable("APP_ENVIRONMENT") ?? "Development";
        }

        [Authorize]
        [HttpGet]
        [Route("status/secure")]
        public string GetSecured()
        {
            var claims = string.Join(",",User.Claims.Select(c => $"{c.Type}:{c.Value}"));
            return $"Hello, {claims}! You are currently authenticated.";
        }
    }
}
