using System.Collections.Generic;
using Business;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models;

namespace Api.Controllers
{
    [Route("api/org")]
    public class OrgController : BaseController
    {
        private readonly IOrgDatasource _orgDatasource;

        public OrgController(IOrgDatasource orgDatasource)
        {
            _orgDatasource = orgDatasource;
        }

        [Authorize]
        [HttpGet]
        [Route("")]
        public IEnumerable<Org> Get() => _orgDatasource.Get(CurrentMemberID);

        [Authorize]
        [HttpGet]
        [Route("{id}/tickets/open")]
        public IEnumerable<Ticket> GetTickets(int id) => _orgDatasource.GetTickets(id, CurrentMemberID);

        [Authorize]
        [HttpPost]
        [Route("")]
        public int Post([FromBody] Org org) => _orgDatasource.Insert(org, CurrentMemberID);

        [Authorize]
        [HttpPut]
        [Route("")]
        public void Put([FromBody] Org org) => _orgDatasource.Update(org, CurrentMemberID);
    }
}
