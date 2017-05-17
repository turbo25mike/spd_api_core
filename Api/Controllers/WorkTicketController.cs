using Business;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models;
using System.Collections.Generic;

namespace Api.Controllers
{
    [Route("api/work/{id}/ticket")]
    public class WorkTicketController: BaseController
    {
        private readonly ITicketDatasource _ticketDatasource;

        public WorkTicketController(ITicketDatasource ticketDatasource)
        {
            _ticketDatasource = ticketDatasource;
        }
        
        [Authorize]
        [HttpGet]
        [Route("open")]
        public IEnumerable<Ticket> GetOpenTickets(int id) => _ticketDatasource.GetTickets(id, CurrentMemberID);

        [Authorize]
        [HttpGet]
        [Route("{ticketID}/chat")]
        public IEnumerable<TicketChat> GetTicketChat(int id, int ticketID) => _ticketDatasource.GetTicketChat(ticketID, id, CurrentMemberID);
    }
}
