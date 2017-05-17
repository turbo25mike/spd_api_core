using System;
using System.Collections.Generic;
using Models;
using Business.DataStore;

namespace Business
{
    public interface IWorkTagDatasource
    {
        IEnumerable<WorkTag> Get(int id, int memberID);

        int Insert(WorkTag tag, int memberID);

        void Update(WorkTag tag, int memberID);
    }

    public class WorkTagDatasource : WorkBase, IWorkTagDatasource
    {
        public WorkTagDatasource(IDatabase db) : base(db){}

        public IEnumerable<WorkTag> Get(int id, int memberID)
        {
            if (!IsMember(id, memberID))
                return null;
            var script = @"SELECT * FROM work_tag 
            WHERE WorkID = @id AND RemovedDate IS NULL";
            return DB.Query<WorkTag>(script, new { id });
        }

        public int Insert(WorkTag tag, int memberID)
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

        public void Update(WorkTag tag, int memberID)
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
    }
}
