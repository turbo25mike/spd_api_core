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
        private readonly IWorkDatasource _workDatasource;

        public WorkTagController(IWorkDatasource workDatasource)
        {
            _workDatasource = workDatasource;
        }

        [Authorize]
        [HttpGet]
        [Route("")]
        public IEnumerable<WorkTag> Get(int id) => _workDatasource.GetTags(id, CurrentMemberID);

        [Authorize]
        [HttpPut]
        [Route("")]
        public void Put([FromBody] WorkTag tag) => _workDatasource.UpdateTag(tag, CurrentMemberID);

        [Authorize]
        [HttpPost]
        [Route("")]
        public int Post([FromBody] WorkTag tag) => _workDatasource.InsertTag(tag, CurrentMemberID);
    }
}
