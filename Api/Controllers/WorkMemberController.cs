using System.Collections.Generic;
using System.Threading.Tasks;
using Api.DataStore;
using Api.DataStore.SqlScripts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/work/{id}/member")]
    public class WorkMemberController : BaseController
    {
        public WorkMemberController(IDatabase db, IAppSettings settings) : base(db, settings) { }

        [Authorize]
        [HttpGet]
        [Route("")]
        public async Task<IEnumerable<Member>> GetWorkMembers(int id)
        {
            var currentMember = await GetCurrentMember();
            var isMember = DB.QuerySingle<WorkMember>(WorkMemberScripts.IsMember, new {WorkID = id, currentMember.MemberID});
            return isMember == null ? null : DB.Query<Member>(WorkMemberScripts.GetWorkMembersByWorkID, new { WorkID = id, currentMember.MemberID });
        }
    }
}
