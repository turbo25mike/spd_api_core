using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using Api.DataStore;

namespace Api.DataContext
{
    public interface IMemberContext
    {
        Member CurrentMember { get; }
    }

    public class MemberContext: IMemberContext
    {
        public MemberContext(IDatabase db)
        {
            _db = db;
        }

        private readonly IDatabase _db;

        private Member _currentMember;
        public Member CurrentMember
        {
            get
            {
                if (_currentMember != null || ClaimsPrincipal.Current?.Claims == null) return _currentMember;
                var identity = ClaimsPrincipal.Current.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(identity)) return null;
                var result = _db.Select<Member>(where: new DBWhere { new DBWhereColumn(nameof(Member.LoginID), identity) }, limit: 1).FirstOrDefault();
                _currentMember = result;
                return _currentMember;
            }
        }
    }
}
