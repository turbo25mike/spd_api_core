using Api.DataStore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Api.Controllers
{
    [Route("api/status")]
    public class StatusController : BaseController
    {
        private readonly IAppSettings _appSettings;

        public StatusController(IDatabase db, IAppSettings settings) : base(db, settings)
        {
            _appSettings = settings;
        }

        [HttpGet]
        [Route("")]
        public string Get()
        {
            return "Service Looking Good!";
        }

        [Authorize]
        [HttpGet]
        [Route("auth")]
        public string GetAuth0Domain()
        {
            return _appSettings.Auth0_Domain;
        }

        [Authorize]
        [HttpGet]
        [Route("environment")]
        public string GetEnvironment()
        {
            return _appSettings.Environment;
        }

        [Authorize]
        [HttpGet]
        [Route("db")]
        public string GetDBStatus()
        {
            //DB.QuerySingle<Member>();
            return "DB Looking Good!";
        }
    }
}
