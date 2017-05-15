using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.DataStore;
using Api.DataStore.SqlScripts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/work")]
    public class WorkController : BaseController
    {
        public WorkController(IDatabase db, IAppSettings settings) : base(db, settings) { }

        [Authorize]
        [HttpGet]
        [Route("")]
        public async Task<IEnumerable<Work>> GetWorkAtRootForMember()
        {
            var currentMember = await GetCurrentMember();
            return ConvertToHierarchy(DB.Query<Work>(WorkScripts.GetMyActiveItems, new { Owner = currentMember.MemberID }));
        }

        [Authorize]
        [HttpGet]
        [Route("{id}")]
        public async Task<Work> GetWorkDetails(int id)
        {
            var currentMember = await GetCurrentMember();
            var result = DB.QuerySingle<Work>(WorkScripts.GetByMemberIDAndWorkID, new { WorkID = id, currentMember.MemberID });
            return result;
        }

        [Authorize]
        [HttpGet]
        [Route("org")]
        public async Task<IEnumerable<Work>> GetWorkAtRootForOrg()
        {
            var currentMember = await GetCurrentMember();
            return DB.Query<Work>(WorkScripts.GetActiveOrgItems, new { currentMember.MemberID });
        }

        [Authorize]
        [HttpPut]
        [Route("")]
        public async void Put([FromBody] Work newItem)
        {
            var currentMember = await GetCurrentMember();
            newItem.UpdatedBy = currentMember.MemberID;
            DB.Execute(WorkScripts.Update, newItem);
        }

        [Authorize]
        [HttpPost]
        [Route("")]
        public async Task<int> Post([FromBody] Work newItem)
        {
            var currentMember = await GetCurrentMember();
            newItem.OwnerID = currentMember.MemberID;
            newItem.CreatedBy = currentMember.MemberID;
            newItem.UpdatedBy = currentMember.MemberID;
            return DB.QuerySingle<int>(WorkScripts.Insert, newItem);
        }

        [Authorize]
        [HttpDelete]
        [Route("{id}")]
        public void Delete(int id)
        {
            DB.Execute(WorkScripts.Delete, new { WorkID = id });
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
