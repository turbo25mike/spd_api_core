namespace Api.DataStore.SqlScripts
{
    public static class TagScripts
    {
        public static string Get =
            @"SELECT * FROM tag 
            WHERE Name = @Name";

        public static string Insert =
            @"INSERT INTO tag
            ( Name,CreatedBy,CreatedDate,UpdatedBy,UpdatedDate )
            VALUES
            ( @Name, @CreatedBy, NOW(), @UpdatedBy, NOW() );
            SELECT LAST_INSERT_ID();";
    }
}
