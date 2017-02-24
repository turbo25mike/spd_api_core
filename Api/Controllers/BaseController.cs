using Api.DataContext;
using Api.DataStore;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    public class BaseController: Controller
    {
        public BaseController(IDatabase db, IMemberContext context)
        {
            DB = db;
            Context = context;
        }

        protected readonly IDatabase DB;
        protected readonly IMemberContext Context;
    }
}
