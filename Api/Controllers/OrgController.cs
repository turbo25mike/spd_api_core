using System;
using System.Linq;
using Api.DataContext;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/org")]
    public class OrgController : Controller
    {
        private readonly IDatabase _db;

        public OrgController(IDatabase db)
        {
            _db = db;
        }

        [Authorize]
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
            var result = _db.Select<Member>(limit:1);
            return result != null && result.Any() ? $"Hey, {result[0].UserName}! DB Looking Good!": "DB empty";
        }
    }
}
