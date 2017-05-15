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
            ( WorkID,Title,`Value`,Color,
            CreatedBy,CreatedDate,UpdatedBy,UpdatedDate )
            VALUES
            ( @WorkID, @Title, @Value, @Color, @CreatedBy, NOW(), @UpdatedBy, NOW() );
            SELECT LAST_INSERT_ID();";
        
        public static string Update =
            @"UPDATE work_tag
            SET
            `Title` = @Title,
            `Value` = @Value,
            `Color` = @Color,
            `UpdatedBy` = @UpdatedBy,
            `UpdatedDate` = NOW()
            WHERE `WorkTagID` = @WorkTagID;";

    }
}
