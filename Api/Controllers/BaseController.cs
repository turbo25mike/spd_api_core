using System.Linq;
using System.Security.Claims;
using Api.DataStore;
using Api.DataStore.SqlScripts;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    public class BaseController : Controller
    {
        public BaseController(IDatabase db) { DB = db; }

        protected readonly IDatabase DB;

        private Member _currentMember;
        public Member GetCurrentMember()
        {
#if DEBUG
            _currentMember = DB.QuerySingle<Member>(MemberScripts.GetMember, new { LoginID = "google-oauth2|113205226902327356570" });
            return _currentMember;
#endif

            if (_currentMember != null || User.Claims == null)
                return _currentMember;
            var identity = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(identity))
                return null;
            _currentMember = DB.QuerySingle<Member>(MemberScripts.GetMember, new { LoginID = identity });
            return _currentMember;
        }
    }
}
