using System.Collections.Generic;
using Business;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models;

namespace Api.Controllers
{
    [Route("api/work")]
    public class WorkController : BaseController
    {
        private readonly IWorkDatasource _workDatasource;

        public WorkController(IWorkDatasource workDatasource)
        {
            _workDatasource = workDatasource;
        }

        [Authorize]
        [HttpGet]
        [Route("")]
        public IEnumerable<Work> GetWorkAtRootForMember() => _workDatasource.GetWorkAtRootForMember(CurrentMemberID);

        [Authorize]
        [HttpGet]
        [Route("{id}")]
        public Work GetWorkDetails(int id) => _workDatasource.GetWorkDetails(id, CurrentMemberID);

        [Authorize]
        [HttpGet]
        [Route("org")]
        public IEnumerable<Work> GetWorkAtRootForOrg() => _workDatasource.GetWorkAtRootForOrg(CurrentMemberID);


        [Authorize]
        [HttpPut]
        [Route("")]
        public void Put([FromBody] Work newItem) => _workDatasource.Update(newItem, CurrentMemberID);

        [Authorize]
        [HttpPost]
        [Route("")]
        public int Post([FromBody] Work newItem) => _workDatasource.Insert(newItem, CurrentMemberID);

        [Authorize]
        [HttpDelete]
        [Route("{id}")]
        public void Delete(int id) => _workDatasource.Delete(id, CurrentMemberID);
    }
}
