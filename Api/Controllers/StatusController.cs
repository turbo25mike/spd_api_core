using System;
using Api.DataContext;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/status")]
    public class StatusController : BaseController
    {
        public StatusController(IDatabase db, IAppSettings settings) : base(db, settings){}

        [HttpGet]
        [Route("")]
        public string Get()
        {
            return "Service Looking Good!";
        }

        [Authorize]
        [HttpGet]
        [Route("secure/auth/domain")]
        public string GetAuth0Domain()
        {
            return Appsettings.Auth0_Domain;
        }

        [Authorize]
        [HttpGet]
        [Route("environment")]
        public string GetEnvironment()
        {
            return Appsettings.Environment;
        }

        [Authorize]
        [HttpGet]
        [Route("db")]
        public string GetDBStatus()
        {
            DB.Select<Org>(limit: 1);
            return "DB Looking Good!";
        }
    }
}
