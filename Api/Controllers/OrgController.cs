using System.Collections.Generic;
using Api.DataContext;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/org")]
    public class OrgController : BaseController
    {
        public OrgController(IDatabase db, IAppSettings settings): base(db, settings){}

        [Authorize]
        [HttpGet]
        [Route("")]
        public List<Org> Get()
        {
            return DB.Select<Org>();
        }
    }
}
