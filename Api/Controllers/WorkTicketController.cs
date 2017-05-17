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
        private readonly IWorkDatasource _workDatasource;

        public WorkTicketController(IWorkDatasource workDatasource)
        {
            _workDatasource = workDatasource;
        }
        
        [Authorize]
        [HttpGet]
        [Route("open")]
        public IEnumerable<Ticket> GetOpenTickets(int id) => _workDatasource.GetTickets(id, CurrentMemberID);

        [Authorize]
        [HttpGet]
        [Route("{ticketID}/chat")]
        public IEnumerable<TicketChat> GetTicketChat(int id, int ticketID) => _workDatasource.GetTicketChat(ticketID, id, CurrentMemberID);
    }
}
