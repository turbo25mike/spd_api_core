using System;
using System.Collections.Generic;
using Models;
using Business.DataStore;

namespace Business
{
    public interface IWorkChatDatasource
    {
        IEnumerable<WorkChat> Get(int id, int memberID);
        void Insert(int workID, string newMessage, int memberID);
    }

    public class WorkChatDatasource : WorkBase, IWorkChatDatasource
    {

        public WorkChatDatasource(IDatabase db) : base(db){}
        
        public IEnumerable<WorkChat> Get(int id, int memberID)
        {
            if (!IsMember(id, memberID)) return null;
            var script = @"SELECT * FROM `spd`.`work_chat` WHERE WorkID = @id;";
            return DB.Query<WorkChat>(script, new { id });
        }


        public void Insert(int workID, string newMessage, int memberID)
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
        
    }
}
