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
        public BaseController(IDatabase db, IAppSettings settings)
        {
            DB = db;
            Appsettings = settings;
        }

        internal IDatabase DB { get; set; }
        internal IAppSettings Appsettings { get; set; }
        private Member _currentMember;

        internal async Task<Member> GetCurrentMember()
        {
            if (_currentMember != null) return _currentMember;

            var result = DB.Select<Member>(where: new DBWhere { new DBWhereColumn(nameof(Member.LoginID), User.Identity.Name) }, limit: 1).FirstOrDefault();
            if (result != null)
            {
                _currentMember = result;
            }
            else
            {
                //creating new user
                var user = await WebService.Request<Auth0User>(RequestType.Get, $"{Appsettings.Auth0_Domain}userinfo", token: Request.Headers["Authorization"]);

                var newMember = new Member
                {
                    LoginID = User.Identity.Name,
                    UserName = user.nickname
                };

                _currentMember = (Member)DB.Insert(newMember, 0);
            }

            return _currentMember;
        }
    }
}
