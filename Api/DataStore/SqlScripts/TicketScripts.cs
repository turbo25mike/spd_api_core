namespace Api.DataStore.SqlScripts
{
    public static class TicketScripts
    {
        public static string GetOpenByOrgID =
            @"SELECT * FROM Org o
              LEFT JOIN work w ON w.OrgID = o.OrgID
              LEFT JOIN Ticket t ON t.WorkID = w.WorkID
              WHERE o.OrgID = @OrgID AND w.RemovedBy IS NULL AND t.RemovedBy IS NULL AND Resolved = false;";

        public static string GetOpenByWorkID =
            @"SELECT * FROM Ticket WHERE WorkID = @WorkID AND RemovedBy IS NULL AND Resolved = false;";
    }
}
