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
        private readonly IWorkChatDatasource _workChatDatasource;

        public WorkChatController(IWorkChatDatasource workChatDatasource)
        {
            _workChatDatasource = workChatDatasource;
        }

        [Authorize]
        [HttpGet]
        [Route("")]
        public IEnumerable<WorkChat> Get(int id) => _workChatDatasource.Get(id, CurrentMemberID);

        [Authorize]
        [HttpPut]
        [Route("")]
        public void Put(int id, [FromBody] string newMessage) => _workChatDatasource.Insert(id, newMessage, CurrentMemberID);
    }
}
