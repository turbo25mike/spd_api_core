using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models;

namespace Api.Controllers
{
    [Route("api/status")]
    public class StatusController : BaseController
    {
        private readonly IAppSettings _settings;

        public StatusController(IAppSettings settings)
        {
            _settings = settings;
        }

        [HttpGet]
        [Route("")]
        public string Get() => "Service Looking Good!";

        [Authorize]
        [HttpGet]
        [Route("environment")]
        public string GetEnvironment() => _settings.Environment;
    }
}
