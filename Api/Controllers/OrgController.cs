using System.Collections.Generic;
using Api.DataStore;
using Api.DataStore.SqlScripts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/org")]
    public class OrgController : BaseController
    {
        public OrgController(IDatabase db) : base(db) { }

        [Authorize]
        [HttpGet]
        [Route("")]
        public IEnumerable<Org> GetMemberOrgs()
        {
            return DB.Query<Org>(OrgScripts.GetMemberOrgs, new {GetCurrentMember().MemberID});
        }

        [Authorize]
        [HttpPost]
        [Route("")]
        public int Post([FromBody] Org org)
        {
            org.CreatedBy = GetCurrentMember().MemberID;
            org.UpdatedBy = GetCurrentMember().MemberID;
            var orgId = DB.QuerySingle<int>(OrgScripts.Insert, org);
            var orgMember = new OrgMember {MemberID = GetCurrentMember().MemberID, OrgID = orgId, CreatedBy = GetCurrentMember().MemberID, UpdatedBy = GetCurrentMember().MemberID};
            return DB.QuerySingle<int>(OrgMemberScripts.Insert, orgMember);
        }

        [Authorize]
        [HttpPut]
        [Route("")]
        public void Put([FromBody] Org org)
        {
            org.UpdatedBy = GetCurrentMember().MemberID;
            DB.Execute(OrgScripts.Update, org);
        }
    }
}
