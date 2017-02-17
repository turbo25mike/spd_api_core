using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace WebAPIApplication.Controllers
{
    [Route("api")]
    public class AuthController : Controller
    {
        private IMemoryCache _cache;

        public AuthController(IMemoryCache cache)
        {
            _cache = cache;
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
