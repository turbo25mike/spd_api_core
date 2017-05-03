namespace Api.DataStore.SqlScripts
{
    public static class WorkTagScripts
    {
        public static string Get =
            @"SELECT * FROM work_tag 
            WHERE TagID = @TagID AND WorkID = @WorkID";

        public static string Insert =
            @"INSERT INTO work_tag
            ( WorkID,TagID,CreatedBy,CreatedDate,UpdatedBy,UpdatedDate )
            VALUES
            ( @WorkID, @TagID, @CreatedBy, NOW(), @UpdatedBy, NOW() );
            SELECT LAST_INSERT_ID();";
    }
}
