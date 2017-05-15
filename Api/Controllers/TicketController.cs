using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Api.DataStore;
using Api.DataStore.SqlScripts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/ticket")]
    public class TicketController : BaseController
    {
        public TicketController(IDatabase db, IAppSettings settings) : base(db, settings) { }

        [Authorize]
        [HttpGet]
        [Route("work/{id}/open")]
        public async Task<IEnumerable<Ticket>> GetWorkTickets(int id)
        {
            var currentMember = await GetCurrentMember();
            var isMember = DB.QuerySingle<WorkMember>(WorkMemberScripts.IsMember, new { WorkID = id, currentMember.MemberID });
            return isMember == null ? null : DB.Query<Ticket>(TicketScripts.GetOpenByWorkID, new { WorkID = id });
        }

        [Authorize]
        [HttpGet]
        [Route("org/{id}/open")]
        public async Task<IEnumerable<Ticket>> GetOrgTickets(int id)
        {
            var currentMember = await GetCurrentMember();
            var isMember = DB.QuerySingle<OrgMember>(OrgMemberScripts.IsMember, new { OrgID = id, currentMember.MemberID });
            return isMember == null ? null : DB.Query<Ticket>(TicketScripts.GetOpenByOrgID, new { OrgID = id });
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
