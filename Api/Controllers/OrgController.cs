using System.Collections.Generic;
using System.Linq;
using Api.DataStore;
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
        public List<Org> GetMemberOrgs()
        {
            var memberOrgs = DB.Select<OrgMember>(where: new DBWhere {new DBWhereColumn(nameof(OrgMember.MemberID), GetCurrentMember().MemberID)}).Select(mo => mo.OrgID).ToArray();
            return memberOrgs.Any() ? DB.Select<Org>(where: new DBWhere {new DBWhereColumn(nameof(Org.OrgID), memberOrgs)}) : new List<Org>();
        }
    }
}
