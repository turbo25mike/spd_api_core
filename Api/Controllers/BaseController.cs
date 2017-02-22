using System.Linq;
using System.Threading.Tasks;
using Api.DataContext;
using Api.Extensions;
using Api.Models;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    public class BaseController : Controller
    {
        internal IDatabase _db { get; set; }
        internal IAppSettings _appsettings { get; set; }

        internal async Task<Member> ValidateMember()
        {
            var result = _db.Select<Member>(where: new DBWhere {new DBWhereColumn(nameof(Member.LoginID), User.Identity.Name) }, limit: 1).FirstOrDefault();
            if (result != null) return result;

            //creating new user
            var user = await WebService.Request<Auth0User>(RequestType.Get, $"{_appsettings.Auth0_Domain}userinfo", token: User.Identity.Name);
            
            var newMember = new Member
                {
                    LoginID = User.Identity.Name,
                    UserName = user.nickname
                };

            return (Member)_db.Insert(newMember, 0);
        }
    }
}
