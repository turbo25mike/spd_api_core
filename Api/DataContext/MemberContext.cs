using System.Linq;
using System.Security.Claims;
using Api.DataStore;
using Microsoft.AspNetCore.Http;

namespace Api.DataContext
{
    public interface IMemberContext
    {
        Member CurrentMember { get; }
    }

    public class MemberContext: IMemberContext
    {
        public MemberContext(IDatabase db, IHttpContextAccessor httpContextAccessor)
        {
            _db = db;
            _httpContextAccessor = httpContextAccessor;
        }

        private readonly IDatabase _db;

        private Member _currentMember;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public Member CurrentMember
        {
            get
            {
                if (_currentMember != null || _httpContextAccessor.HttpContext.User.Claims == null) return _currentMember;
                var identity = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(identity)) return null;
                var result = _db.Select<Member>(where: new DBWhere { new DBWhereColumn(nameof(Member.LoginID), identity) }, limit: 1).FirstOrDefault();
                _currentMember = result;
                return _currentMember;
            }
        }
    }
}
