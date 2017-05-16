using System.Linq;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    public class BaseController : Controller
    {
        public int CurrentMemberID => int.Parse(User.Claims.FirstOrDefault(c => c.Type == "MemberID").Value);
    }
}
