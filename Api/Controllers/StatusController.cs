using System;
using Api.DataContext;
using Api.DataStore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/status")]
    public class StatusController : BaseController
    {
        private readonly IAppSettings _appSettings;

        public StatusController(IDatabase db, IMemberContext memberContext, IAppSettings settings) : base(db, memberContext)
        {
            _appSettings = settings;
        }

        [Route("")]
        public string Get()
        {
            return "Service Looking Good!";
        }

        [Authorize]
        [Route("auth")]
        public string GetAuth0Domain()
        {
            return _appSettings.Auth0_Domain;
        }

        [Authorize]
        [Route("environment")]
        public string GetEnvironment()
        {
            return _appSettings.Environment;
        }

        [Authorize]
        [Route("db")]
        public string GetDBStatus()
        {
            DB.Select<Org>(limit: 1);
            return "DB Looking Good!";
        }
    }
}
