﻿namespace Api.DataStore.SqlScripts
{
    public static class WorkChatScripts
    {

        public static string GetChatByMemberIDAndWorkID =
            @"SELECT wm.*, m.UserName as 'UpdatedByName' FROM `spd`.`work_chat` wc
                LEFT JOIN work_member wm on wm.WorkID = wc.WorkID AND wm.RemovedDate IS NULL
                LEFT JOIN member m on m.MemberID = wc.UpdatedBy
                WHERE wc.WorkID = @WorkID AND wm.MemberID = @MemberID;";

        public static string Insert =
            @"INSERT INTO `spd`.`work_chat`
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
    }
}
