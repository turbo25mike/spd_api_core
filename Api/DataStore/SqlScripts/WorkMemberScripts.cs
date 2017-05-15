namespace Api.DataStore.SqlScripts
{
    public static class WorkMemberScripts
    {
        public static string GetWorkMembersByWorkID = 
            @"SELECT m.MemberID, m.Picture, m.Nickname FROM work_member wm
              LEFT JOIN member m ON m.MemberID = wm.MemberID
              Where wm.WorkID = @WorkID AND wm.RemovedDate IS NULL AND m.RemovedDate IS NULL";

        public static string IsMember =
            @"SELECT MemberID FROM work_member
              Where WorkID = @WorkID AND MemberID = @MemberID AND RemovedDate IS NULL";

    }
}
