namespace Api.DataStore.SqlScripts
{
    public static class WorkChatScripts
    {

        public static string GetChatByWorkID =
            @"SELECT * FROM `spd`.`work_chat` WHERE WorkID = @WorkID;";

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
