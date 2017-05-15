using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Api.DataStore;
using Api.DataStore.SqlScripts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/work/{id}/chat")]
    public class WorkChatController : BaseController
    {
        public WorkChatController(IDatabase db, IAppSettings settings) : base(db, settings) { }

        [Authorize]
        [HttpGet]
        [Route("")]
        public async Task<IEnumerable<WorkChat>> GetWorkChat(int id)
        {
            var currentMember = await GetCurrentMember();
            var isMember = DB.QuerySingle<WorkMember>(WorkMemberScripts.IsMember, new { WorkID = id, currentMember.MemberID });
            return isMember == null ? null : DB.Query<WorkChat>(WorkChatScripts.GetChatByWorkID, new { WorkID = id });
        }

        [Authorize]
        [HttpPut]
        [Route("")]
        public async void Put(int id, [FromBody] string newMessage)
        {
            var currentMember = await GetCurrentMember();
            var isMember = DB.QuerySingle<WorkMember>(WorkMemberScripts.IsMember, new { WorkID = id, currentMember.MemberID });
            if (isMember == null)
                throw new ArgumentOutOfRangeException();
            var newItem = new WorkChat()
            {
                CreatedBy = currentMember.MemberID,
                UpdatedBy = currentMember.MemberID,
                Message = newMessage,
                WorkID = id
            };
            DB.Execute(WorkChatScripts.Insert, newItem);
        }
    }
}
