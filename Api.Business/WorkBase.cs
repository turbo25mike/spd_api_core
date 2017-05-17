using Models;
using Business.DataStore;

namespace Business
{
    public interface IWorkBase
    {
        bool IsMember(int id, int memberID);
    }

    public class WorkBase : Datasource, IWorkBase
    {
        public WorkBase(IDatabase db) : base(db) { }
        
        
        public bool IsMember(int id, int memberID)
        {
            var script = @"SELECT MemberID FROM work_member
              Where WorkID = @id AND MemberID = @memberID AND RemovedDate IS NULL";

            var isMember = DB.QuerySingle<WorkMember>(script, new { id, memberID });
            return isMember != null;
        }
    }
}
