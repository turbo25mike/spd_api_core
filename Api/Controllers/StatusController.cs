using System;
using System.Linq;
using Api.DataContext.Models;
using Api.DataContext.Stores;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/status")]
    public class StatusController : Controller
    {
        private readonly IStore<Member, MemberStore.Column> _store;

        public StatusController(IStore<Member, MemberStore.Column> store)
        {
            _store = store;
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
            var result = _store.Get(limit:1);
            var enumerable = result as Member[] ?? result.ToArray();
            return enumerable.Any() ? $"Hey, {enumerable[0].UserName}! DB Looking Good!": "DB empty";
        }
    }
}
