using System.Collections.Generic;
using Business;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models;

namespace Api.Controllers
{
    [Route("api/work/{id}/chat")]
    public class WorkChatController : BaseController
    {
        private readonly IWorkDatasource _workDatasource;

        public WorkChatController(IWorkDatasource workDatasource)
        {
            _workDatasource = workDatasource;
        }

        [Authorize]
        [HttpGet]
        [Route("")]
        public IEnumerable<WorkChat> GetWorkChat(int id) => _workDatasource.GetChat(id, CurrentMemberID);

        [Authorize]
        [HttpPut]
        [Route("")]
        public void Put(int id, [FromBody] string newMessage) => _workDatasource.InsertChat(id, newMessage, CurrentMemberID);
    }
}
