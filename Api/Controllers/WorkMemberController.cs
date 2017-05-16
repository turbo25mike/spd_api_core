using System.Collections.Generic;
using Business;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models;

namespace Api.Controllers
{
    [Route("api/work/{id}/member")]
    public class WorkMemberController : BaseController
    {
        private readonly IWorkDatasource _workDatasource;

        public WorkMemberController(IWorkDatasource workDatasource)
        {
            _workDatasource = workDatasource;
        }

        [Authorize]
        [HttpGet]
        [Route("")]
        public IEnumerable<Member> Get(int id) => _workDatasource.GetMembers(id, CurrentMemberID);
    }
}
