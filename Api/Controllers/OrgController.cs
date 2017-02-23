using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.DataContext;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/org")]
    public class OrgController : BaseController
    {
        public OrgController(IDatabase db, IAppSettings settings): base(db, settings){}

        [Authorize]
        [HttpGet]
        [Route("")]
        public async Task<List<Org>> Get()
        {
            var member = await GetCurrentMember();
            var memberOrgs = DB.Select<OrgMember>(where: new DBWhere {new DBWhereColumn(nameof(OrgMember.MemberID), member.MemberID)}).Select(mo => mo.OrgID);
            
            return DB.Select<Org>(where: new DBWhere {new DBWhereColumn(nameof(Org.OrgID), memberOrgs)});
        }
        
    }
}
