using System;
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
            return ConvertToHierarchy(DB.Query<Work>(WorkScripts.GetMyActiveItems, new { Owner = GetCurrentMember().MemberID }));
        }

        [Authorize]
        [HttpGet]
        [Route("{id}")]
        public Work GetWorkDetails(int id)
        {
            var result = DB.QuerySingle<Work>(WorkScripts.GetByMemberIDAndWorkID, new { WorkID = id, GetCurrentMember().MemberID });
            result.Tags = DB.Query<WorkTag>(WorkTagScripts.GetByWorkID, new { WorkID = id }).ToList();
            return result;
        }

        [Authorize]
        [HttpGet]
        [Route("{id}/chat")]
        public IEnumerable<WorkChat> GetWorkChat(int id)
        {
            return DB.Query<WorkChat>(WorkChatScripts.GetChatByMemberIDAndWorkID, new { WorkID = id, MemberID = GetCurrentMember().MemberID });
        }

        [Authorize]
        [HttpGet]
        [Route("org")]
        public IEnumerable<Work> GetWorkAtRootForOrg()
        {
            return DB.Query<Work>(WorkScripts.GetActiveOrgItems, new { GetCurrentMember().MemberID });
        }

        [Authorize]
        [HttpPut]
        [Route("{id}/chat")]
        public void Put(int id, [FromBody] string newMessage)
        {
            var newItem = new WorkChat()
            {
                CreatedBy = GetCurrentMember().MemberID,
                UpdatedBy = GetCurrentMember().MemberID,
                Message = newMessage,
                WorkID = id
            };
            DB.Execute(WorkChatScripts.Insert, newItem);
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
        [HttpPut]
        [Route("{id}/tag")]
        public int PutTag(int id, [FromBody] WorkTag tag)
        {
            if (tag == null)
                throw new ArgumentNullException(nameof(tag));

            tag.WorkID = id;
            tag.CreatedBy = GetCurrentMember().MemberID;
            tag.UpdatedBy = GetCurrentMember().MemberID;

            var serverTag = DB.QuerySingle<WorkTag>(WorkTagScripts.GetByWorkIDAndTagName, tag);
            if (serverTag == null)
                return DB.QuerySingle<int>(WorkTagScripts.Insert, tag);

            serverTag.TagValue = tag.TagValue;
            DB.Execute(WorkTagScripts.Update, serverTag);
            return serverTag.WorkTagID;
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
