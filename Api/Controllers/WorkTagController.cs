using System.Collections.Generic;
using Business;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models;

namespace Api.Controllers
{
    [Route("api/work/{id}/tag")]
    public class WorkTagController : BaseController
    {
        private readonly IWorkTagDatasource _workTagDatasource;

        public WorkTagController(IWorkTagDatasource workTagDatasource)
        {
            _workTagDatasource = workTagDatasource;
        }

        [Authorize]
        [HttpGet]
        [Route("")]
        public IEnumerable<WorkTag> Get(int id) => _workTagDatasource.Get(id, CurrentMemberID);

        [Authorize]
        [HttpPut]
        [Route("")]
        public void Put([FromBody] WorkTag tag) => _workTagDatasource.Update(tag, CurrentMemberID);

        [Authorize]
        [HttpPost]
        [Route("")]
        public int Post([FromBody] WorkTag tag) => _workTagDatasource.Insert(tag, CurrentMemberID);
    }
}
