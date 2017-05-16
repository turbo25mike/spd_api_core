using Business;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models;

namespace Api.Controllers
{
    [Route("api/member")]
    public class MemberController : BaseController
    {
        private readonly IMemberDatasource _memberDatasource;

        public MemberController(IMemberDatasource memberDatasource, IAppSettings settings)
        {
            _memberDatasource = memberDatasource;
        }

        [Authorize]
        [HttpGet]
        [Route("")]
        public Member Get() => _memberDatasource.Get(CurrentMemberID);
    }
}
