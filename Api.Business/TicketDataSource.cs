using System;
using System.Collections.Generic;
using Models;
using Business.DataStore;

namespace Business
{
    public interface ITicketDatasource
    {
        bool IsMember(int id, int memberID);
        
        IEnumerable<Ticket> GetTicketsByWorkID(int id, int memberID);
        IEnumerable<TicketChat> GetTicketChat(int ticketID, int workID, int memberID);
    }

    public class TicketDatasource : WorkBase, ITicketDatasource
    {
        public TicketDatasource(IDatabase db) : base(db) { }

        public IEnumerable<Ticket> GetTickets(int memberID)
        {
            var script = @"SELECT * FROM ticket WHERE CreatedBy = @memberID AND RemovedBy IS NULL;";
            return DB.Query<Ticket>(script, new { memberID });
        }

        public IEnumerable<Ticket> GetTicketsByWorkID(int workID, int memberID)
        {
            if (!IsMember(workID, memberID)) return null;
            var script = $@"SELECT * FROM ticket WHERE WorkID = @{nameof(workID)} AND RemovedBy IS NULL AND Resolved = false;";
            return DB.Query<Ticket>(script, new { workID });
        }

        public IEnumerable<TicketChat> GetTicketChat(int ticketID, int workID, int memberID)
        {
            if (GetTicket(ticketID, memberID) == null)
                return null;

            var script = $@"SELECT * FROM `spd`.`ticket_chat` WHERE TicketID = @{nameof(ticketID)} AND RemovedBy IS NULL;";
            return DB.Query<TicketChat>(script, new { ticketID });
        }

        public void Insert(int ticketID, string newMessage, int memberID)
        {
            if (GetTicket(ticketID, memberID) == null)
                throw new ArgumentOutOfRangeException();

            var script = $@"INSERT INTO `spd`.`ticket_chat`
            (
                `TicketID`,`Message`,
                `CompleteDate`,`CreatedBy`,`CreatedDate`,`UpdatedBy`,`UpdatedDate`
            )
            VALUES
            (
                @{nameof(ticketID)},@{nameof(newMessage)},
                @{nameof(memberID)},NOW(),@{nameof(memberID)},NOW()
            );
            SELECT LAST_INSERT_ID();";

            DB.Execute(script, new {ticketID, newMessage, memberID });
        }

        public Ticket GetTicket(int ticketID, int memberID)
        {
            var ticketScript = $@"SELECT * FROM ticket WHERE TicketID = @{ticketID} AND RemovedBy IS NULL;";
            var ticket = DB.QuerySingle<Ticket>(ticketScript, new { ticketID });

            return ticket == null || !IsMember(ticket.WorkID, memberID) && ticket.CreatedBy != memberID ? null : ticket;
        }
    }
}
