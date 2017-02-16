using System;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace WebAPIApplication.Controllers
{
    [Route("api")]
    public class PingController : Controller
    {
        private IMemoryCache _cache;

        public PingController(IMemoryCache cache)
        {
            _cache = cache;
        }

        [HttpGet]
        [Route("ping")]
        public string Ping()
        {
            return "Pong";
        }

        [Authorize]
        [HttpGet("claims")]
        public object Claims()
        {
            return User.Claims.Select(c =>
            new
            {
                Type = c.Type,
                Value = c.Value
            });
        }

        [Authorize]
        [HttpGet]
        [Route("ping/secure")]
        public string PingSecured()
        {
            return "All good. You only get this message if you are authenticated.";
        }

        [HttpGet]
        [Route("status")]
        public object Get()
        {
            return "Looking Good!";
        }

        [HttpGet]
        [Route("status/environment")]
        public object GetEnvironment()
        {
            return Environment.GetEnvironmentVariable("APP_ENVIRONMENT") ?? "Development";
        }

        [Authorize]
        [HttpGet]
        [Route("status/secure")]
        public object GetSecured()
        {
            var claims = string.Join(",",User.Claims.Select(c => $"{c.Type}:{c.Value}"));
            return $"Hello, {claims}! You are currently authenticated.";
        }

        [HttpGet]
        [Route("auth")]
        public void GetAuth(string code, string state)
        {
            _cache.Set(state.TrimEnd('#'), code, new TimeSpan(0, 0, 0, 20));
        }
        
        [HttpGet]
        [Route("auth/{state}/code")]
        public string GetAuthCode(string state)
        {
            var result = _cache.Get<string>(state);
            if (!string.IsNullOrEmpty(result))
                return result;
            throw new ArgumentNullException();
        }


    }
}
