using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Api.DataStore;
using Api.DataStore.SqlScripts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/work/{id}/tag")]
    public class WorkTagController : BaseController
    {
        public WorkTagController(IDatabase db, IAppSettings settings) : base(db, settings) { }

        [Authorize]
        [HttpGet]
        [Route("")]
        public async Task<IEnumerable<WorkTag>> Get(int id)
        {
            var currentMember = await GetCurrentMember();
            var isMember = DB.QuerySingle<WorkMember>(WorkMemberScripts.IsMember, new { WorkID = id, currentMember.MemberID });
            return isMember == null ? null : DB.Query<WorkTag>(WorkTagScripts.GetByWorkID, new { WorkID = id, currentMember.MemberID });
        }


        [Authorize]
        [HttpPut]
        [Route("")]
        public async Task<int> PutTag(int id, [FromBody] WorkTag tag)
        {
            var currentMember = await GetCurrentMember();
            var isMember = DB.QuerySingle<WorkMember>(WorkMemberScripts.IsMember, new { WorkID = id, currentMember.MemberID });
            if (isMember == null)
                throw new ArgumentOutOfRangeException();
            if (tag == null)
                throw new ArgumentNullException(nameof(tag));

            tag.WorkID = id;
            tag.CreatedBy = currentMember.MemberID;
            tag.UpdatedBy = currentMember.MemberID;

            var serverTag = DB.QuerySingle<WorkTag>(WorkTagScripts.GetByWorkIDAndTagName, tag);
            if (serverTag == null)
                return DB.QuerySingle<int>(WorkTagScripts.Insert, tag);

            serverTag.Value = tag.Value;
            serverTag.Color = tag.Color;
            DB.Execute(WorkTagScripts.Update, serverTag);
            return serverTag.WorkTagID;
        }
    }
}
