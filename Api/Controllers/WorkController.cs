using System.Collections.Generic;
using System.Linq;
using Api.DataStore;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/work")]
    public class WorkController : BaseController
    {
        public WorkController(IDatabase db) : base(db) { }

        //[Authorize]
        [HttpGet]
        [Route("root")]
        public List<Work> GetWorkAtRootForMember()
        {
            var memberOrgIds = DB.Select<OrgMember>(where: new Where { new WhereColumn<OrgMember>(nameof(OrgMember.MemberID), GetCurrentMember().MemberID) }).Select(mo => mo.OrgID).ToArray();
            if (!memberOrgIds.Any()) return null;
            var columns = ModelHelper.GetColumns<Work>();
            columns.Add(new TableColumn<Org>(nameof(Org.Name)));
            return DB.Select<Work>(
                columns,
                new Joins
                    {
                        new Join<Work, Org>(nameof(Work.OrgID), nameof(Org.OrgID))
                    },
                new Where
                {
                    new WhereColumn<Work>(nameof(Work.ParentWorkID), cmp: WhereComparer.Is, op: WhereOperator.And),
                    new WhereColumns(
                        new WhereItem[]
                        {
                            new WhereColumn<Work>(nameof(Work.OrgID), memberOrgIds, WhereOperator.Or),
                            new WhereColumn<Work>(nameof(Work.Owner), GetCurrentMember().MemberID)
                        }
                        ,WhereOperator.And),
                    new WhereColumn<Work>(nameof(Work.RemovedBy), cmp: WhereComparer.Is, op: WhereOperator.And),
                    new WhereColumn<Org>(nameof(Org.RemovedBy), cmp: WhereComparer.Is)
                });
        }
    }
}
