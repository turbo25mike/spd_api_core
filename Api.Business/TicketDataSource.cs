using System.Collections.Generic;
using Models;
using Business.DataStore;

namespace Business
{
    public interface ITicketDatasource
    {
        bool IsMember(int id, int memberID);
        
        IEnumerable<Ticket> GetTickets(int id, int memberID);
        IEnumerable<TicketChat> GetTicketChat(int ticketID, int workID, int memberID);
    }

    public class TicketDatasource : Datasource, ITicketDatasource
    {
        public TicketDatasource(IDatabase db) : base(db) { }
        
        public IEnumerable<Ticket> GetTickets(int id, int memberID)
        {
            if (!IsMember(id, memberID)) return null;
            var script = @"SELECT * FROM ticket WHERE WorkID = @id AND RemovedBy IS NULL AND Resolved = false;";
            return DB.Query<Ticket>(script, new { id });
        }

        public IEnumerable<TicketChat> GetTicketChat(int ticketID, int workID, int memberID)
        {
            if (!IsMember(workID, memberID)) return null;
            var script = @"SELECT * FROM `spd`.`ticket_chat` WHERE TicketID = @ticketID;";
            return DB.Query<TicketChat>(script, new { ticketID });
        }

        public bool IsMember(int id, int memberID)
        {
            var script = @"SELECT MemberID FROM work_member
              Where WorkID = @id AND MemberID = @memberID AND RemovedDate IS NULL";

            var isMember = DB.QuerySingle<WorkMember>(script, new { id, memberID });
            return isMember != null;
        }
    }
}
