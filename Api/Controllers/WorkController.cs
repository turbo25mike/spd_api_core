using System.Collections.Generic;
using Api.DataStore;
using Api.DataStore.SqlScripts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/work")]
    public class WorkController : BaseController
    {
        public WorkController(IDatabase db) : base(db) { }

        [Authorize]
        [HttpGet]
        [Route("")]
        public IEnumerable<Work> GetWorkAtRootForMember()
        {
            return DB.Query<Work>(WorkScripts.GetActiveRootItems, new { GetCurrentMember().MemberID});
        }

        [Authorize]
        [HttpPut]
        [Route("")]
        public void Put([FromBody] Work newItem)
        {
            newItem.UpdatedBy = GetCurrentMember().MemberID;
            DB.Execute(WorkScripts.Update, newItem);
        }

        [Authorize]
        [HttpPost]
        [Route("")]
        public int Post(Work newItem)
        {
            newItem.CreatedBy = GetCurrentMember().MemberID;
            newItem.UpdatedBy = GetCurrentMember().MemberID;
            return DB.QuerySingle<int>(WorkScripts.Insert, newItem);
        }

        [Authorize]
        [HttpDelete]
        [Route("{id}")]
        public void Delete(int id)
        {
            DB.Execute(WorkScripts.Insert, new {WorkID = id});
        }
    }
}
