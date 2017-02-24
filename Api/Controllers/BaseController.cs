using System;
using System.Linq;
using System.Security.Claims;
using Api.DataStore;
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
            if (_currentMember != null || User.Claims == null)
                return _currentMember;
            var identity = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(identity))
                return null;
            var result = DB.Select<Member>(where: new DBWhere { new DBWhereColumn(nameof(Member.LoginID), identity) }, limit: 1).FirstOrDefault();
            _currentMember = result;
            return _currentMember;
        }
    }
}
