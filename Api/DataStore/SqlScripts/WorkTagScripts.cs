namespace Api.DataStore.SqlScripts
{
    public static class WorkTagScripts
    {
        public static string GetByWorkIDAndTagName =
            @"SELECT * FROM work_tag
            WHERE WorkID = @WorkID AND TagName = @TagName AND RemovedDate IS NULL";

        public static string GetByWorkID =
            @"SELECT * FROM work_tag 
            WHERE WorkID = @WorkID AND RemovedDate IS NULL";
        
        public static string Insert =
            @"INSERT INTO work_tag
            ( WorkID,TagName,TagValue,
            CreatedBy,CreatedDate,UpdatedBy,UpdatedDate )
            VALUES
            ( @WorkID, @TagName, @TagValue, @CreatedBy, NOW(), @UpdatedBy, NOW() );
            SELECT LAST_INSERT_ID();";
        
        public static string Update =
            @"UPDATE work_tag
            SET
            `TagName` = @TagName,
            `TagValue` = @TagValue,
            `UpdatedBy` = @UpdatedBy,
            `UpdatedDate` = NOW()
            WHERE `WorkTagID` = @WorkTagID;";

    }
}
