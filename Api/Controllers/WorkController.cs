using System.Collections.Generic;
using System.Linq;
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
            return ConvertToHierarchy(DB.Query<Work>(WorkScripts.GetActiveRootUserItems, new { Owner = GetCurrentMember().MemberID }));
        }

        [Authorize]
        [HttpGet]
        [Route("org")]
        public IEnumerable<Work> GetWorkAtRootForOrg()
        {
            return DB.Query<Work>(WorkScripts.GetActiveRootOrgItems, new { GetCurrentMember().MemberID });
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
        public int Post([FromBody] Work newItem)
        {
            newItem.Owner = GetCurrentMember().MemberID;
            newItem.CreatedBy = GetCurrentMember().MemberID;
            newItem.UpdatedBy = GetCurrentMember().MemberID;
            return DB.QuerySingle<int>(WorkScripts.Insert, newItem);
        }

        [Authorize]
        [HttpDelete]
        [Route("{id}")]
        public void Delete(int id)
        {
            DB.Execute(WorkScripts.Insert, new { WorkID = id });
        }

        private IEnumerable<Work> ConvertToHierarchy(IEnumerable<Work> list)
        {
            var hierarchy = new List<Work>();

            var enumerable = list as Work[] ?? list.ToArray();
            hierarchy.AddRange(enumerable.Where(e => e.ParentWorkID is null));
            foreach (var work in enumerable)
            {
                if (!work.ParentWorkID.HasValue)
                    continue;

                foreach (var root in hierarchy)
                {
                    var parent = FindParentByID(root, work.ParentWorkID.Value);
                    parent?.Children.Add(work);
                }
            }

            return hierarchy;
        }

        private Work FindParentByID(Work work, int id)
        {
            if (work.WorkID == id)
                return work;

            if (!work.Children.Any())
                return null;

            foreach (var child in work.Children)
            {
                if (child.WorkID == id)
                    return child;

                var found = FindParentByID(child, id);
                if (found != null)
                    return found;
            }
            return null;
        }
    }
}
