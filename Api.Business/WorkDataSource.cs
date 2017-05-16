using System;
using System.Collections.Generic;
using System.Linq;
using Models;
using Business.DataStore;

namespace Business
{
    public interface IWorkDatasource
    {
        bool IsMember(int id, int memberID);

        IEnumerable<WorkChat> GetChat(int id, int memberID);
        IEnumerable<Work> GetWorkAtRootForMember(int memberID);
        IEnumerable<Work> GetWorkAtRootForOrg(int memberID);
        Work GetWorkDetails(int id, int memberID);
        IEnumerable<WorkTag> GetTags(int id, int memberID);
        IEnumerable<Member> GetMembers(int id, int memberID);
        IEnumerable<Ticket> GetTickets(int id, int memberID);

        int Insert(Work work, int memberID);
        void InsertChat(int workID, string newMessage, int memberID);
        int InsertTag(WorkTag tag, int memberID);

        void Update(Work newItem, int memberID);
        void UpdateTag(WorkTag tag, int memberID);

        void Delete(int id, int memberID);
    }

    public class WorkDatasource : Datasource, IWorkDatasource
    {
        public WorkDatasource(IDatabase db) : base(db) { }
        
        public IEnumerable<Work> GetWorkAtRootForMember(int memberID)
        {
            var script = @"SELECT WorkID, ParentWorkID, OrgID, Title FROM work 
                WHERE OwnerID = @memberID AND CompleteDate IS NULL AND RemovedDate IS NULL;";

            return ConvertToHierarchy(DB.Query<Work>(script, new { memberID }));
        }

        public Work GetWorkDetails(int id, int memberID)
        {
            var scripts = @"SELECT * FROM work w
                JOIN work_member wm on wm.WorkID = w.WorkID AND wm.RemovedDate IS NULL AND wm.MemberID = @memberID
                WHERE w.WorkID = @id;";
            var result = DB.QuerySingle<Work>(scripts, new { id, memberID });
            return result;
        }

        public IEnumerable<Work> GetWorkAtRootForOrg(int memberID)
        {
         var scripts = @"SELECT w.WorkID, w.ParentWorkID, w.OrgID, w.Title FROM org_member om
                JOIN org o ON o.OrgID = om.OrgID AND o.RemovedDate IS NULL
                LEFT JOIN work w ON w.OrgID = w.OrgID AND w.RemovedDate IS NULL AND w.CompleteDate IS NULL 
                JOIN work_member wm on wm.WorkID = w.WorkID AND wm.RemovedDate IS NULL AND wm.WorkID is not null
                WHERE om.MemberID = @memberID;";
            return DB.Query<Work>(scripts, new { memberID });
        }

        public IEnumerable<Member> GetMembers(int id, int memberID)
        {
            if (!IsMember(id, memberID)) return null;

            var script = @"SELECT m.MemberID, m.Picture, m.Nickname FROM work_member wm
              LEFT JOIN member m ON m.MemberID = wm.MemberID
              Where wm.WorkID = @id AND wm.RemovedDate IS NULL AND m.RemovedDate IS NULL";
            
            return DB.Query<Member>(script, new { id });
        }

        public IEnumerable<WorkChat> GetChat(int id, int memberID)
        {
            if (!IsMember(id, memberID)) return null;
            var script = @"SELECT * FROM `spd`.`work_chat` WHERE WorkID = @id;";
            return DB.Query<WorkChat>(script, new { id });
        }

        public IEnumerable<Ticket> GetTickets(int id, int memberID)
        {
            if (!IsMember(id, memberID)) return null;
            var script = @"SELECT * FROM Ticket WHERE WorkID = @id AND RemovedBy IS NULL AND Resolved = false;";
            return DB.Query<Ticket>(script, new { id });
        }


        public void InsertChat(int workID, string newMessage, int memberID)
        {
            if (!IsMember(workID, memberID))
                throw new ArgumentOutOfRangeException();

            var script = @"INSERT INTO `spd`.`work_chat`
            (
                `WorkID`,`Message`,
                `CompleteDate`,`CreatedBy`,`CreatedDate`,`UpdatedBy`,`UpdatedDate`
            )
            VALUES
            (
                @WorkID,@Message,
                @CreatedBy,NOW(),@UpdatedBy,NOW()
            );
            SELECT LAST_INSERT_ID();";

            var newItem = new WorkChat()
            {
                CreatedBy = memberID,
                UpdatedBy = memberID,
                Message = newMessage,
                WorkID = workID
            };
            DB.Execute(script, newItem);
        }

        public IEnumerable<WorkTag> GetTags(int id, int memberID)
        {
            if (!IsMember(id, memberID))
                return null;
            var script = @"SELECT * FROM work_tag 
            WHERE WorkID = @id AND RemovedDate IS NULL";
            return DB.Query<WorkTag>(script, new { id });
        }

        public int InsertTag(WorkTag tag, int memberID)
        {
            if (tag == null)
                throw new ArgumentNullException(nameof(tag));

            if (!IsMember(tag.WorkID, memberID))
                throw new ArgumentOutOfRangeException();

            tag.CreatedBy = memberID;
            tag.UpdatedBy = memberID;

            var script = @"INSERT INTO work_tag
                ( WorkID,Title,`Value`,Color,CreatedBy,CreatedDate,UpdatedBy,UpdatedDate )
                VALUES
                ( @WorkID, @Title, @Value, @Color, @CreatedBy, NOW(), @UpdatedBy, NOW() );
                SELECT LAST_INSERT_ID();";
            return DB.QuerySingle<int>(script, tag);
        }

        public void UpdateTag(WorkTag tag, int memberID)
        {
            if (!IsMember(tag.WorkID, memberID))
                throw new ArgumentOutOfRangeException();
            if (tag == null)
                throw new ArgumentNullException(nameof(tag));

            var scriptGet = @"SELECT * FROM work_tag WHERE WorkTagID = @WorkTagID AND RemovedDate IS NULL";

            if (DB.QuerySingle<WorkTag>(scriptGet, tag) == null)
                throw new ArgumentOutOfRangeException();

            var script = @"UPDATE work_tag
            SET
            `Title` = @Title,
            `Value` = @Value,
            `Color` = @Color,
            `UpdatedBy` = @UpdatedBy,
            `UpdatedDate` = NOW()
            WHERE `WorkTagID` = @WorkTagID;";

            tag.UpdatedBy = memberID;
            DB.Execute(script, tag);
        }

        public bool IsMember(int id, int memberID)
        {
            var script = @"SELECT MemberID FROM work_member
              Where WorkID = @id AND MemberID = @memberID AND RemovedDate IS NULL";

            var isMember = DB.QuerySingle<WorkMember>(script, new { id, memberID });
            return isMember != null;
        }

        public int Insert(Work work, int memberID)
        {
            work.OwnerID = memberID;
            work.CreatedBy = memberID;
            work.UpdatedBy = memberID;

            var script = @"INSERT INTO `spd`.`work`
            (
                `ParentWorkID`,`OrgID`,`Title`,`Description`,`OwnerID`,`Size`,`Priority`,`HoursWorked`,`DueDate`,
                `CompleteDate`,`CreatedBy`,`CreatedDate`,`UpdatedBy`,`UpdatedDate`
            )
            VALUES
            (
                @ParentWorkID,@OrgID,@Title,@Description,@OwnerID,@Size,@Priority,@HoursWorked,@DueDate,@CompleteDate,
                @CreatedBy,NOW(),@UpdatedBy,NOW()
            );
            SELECT LAST_INSERT_ID();";

            return DB.QuerySingle<int>(script, work);
        }

        public void Update(Work newItem, int memberID)
        {
            var script = @"UPDATE work
            SET
            `ParentWorkID` = @ParentWorkID,
            `Title` = @Title,
            `Description` = @Description,
            `Owner` = @Owner,
            `Size` = @Size,
            `Priority` = @Priority,
            `HoursWorked` = @HoursWorked,
            `DueDate` = @DueDate,
            `CompleteDate` = @CompleteDate,
            `UpdatedBy` = @UpdatedBy,
            `UpdatedDate` = NOW()
            WHERE `WorkID` = @WorkID;";

            newItem.UpdatedBy = memberID;
            DB.Execute(script, newItem);
        }

        public void Delete(int id, int memberID)
        {
            var script = @"UPDATE work
            SET
            `RemovedBy` = @RemovedBy,
            `RemovedDate` = NOW()
            WHERE `WorkID` = @id;";
            DB.Execute(script, new { id });
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
