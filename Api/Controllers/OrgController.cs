using System.Collections.Generic;
using System.Threading.Tasks;
using Api.DataStore;
using Api.DataStore.SqlScripts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/org")]
    public class OrgController : BaseController
    {
        public OrgController(IDatabase db, IAppSettings settings) : base(db, settings) { }

        [Authorize]
        [HttpGet]
        [Route("")]
        public async Task<IEnumerable<Org>> GetMemberOrgs()
        {
            var currentMember = await GetCurrentMember();
            return DB.Query<Org>(OrgScripts.GetMemberOrgs, new { currentMember.MemberID});
        }

        [Authorize]
        [HttpPost]
        [Route("")]
        public async Task<int> Post([FromBody] Org org)
        {
            var currentMember = await GetCurrentMember();
            org.CreatedBy = currentMember.MemberID;
            org.UpdatedBy = currentMember.MemberID;
            var orgId = DB.QuerySingle<int>(OrgScripts.Insert, org);
            var orgMember = new OrgMember {MemberID = currentMember.MemberID, OrgID = orgId, CreatedBy = currentMember.MemberID, UpdatedBy = currentMember.MemberID};
            return DB.QuerySingle<int>(OrgMemberScripts.Insert, orgMember);
        }

        [Authorize]
        [HttpPut]
        [Route("")]
        public async void Put([FromBody] Org org)
        {
            var currentMember = await GetCurrentMember();
            org.UpdatedBy = currentMember.MemberID;
            DB.Execute(OrgScripts.Update, org);
        }
    }
}
