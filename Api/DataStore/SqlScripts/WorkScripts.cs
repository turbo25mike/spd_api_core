namespace Api.DataStore.SqlScripts
{
    public static class WorkScripts
    {

        public static string GetByMemberIDAndWorkID =
            @"SELECT * FROM work w
                JOIN work_member wm on wm.WorkID = w.WorkID AND wm.RemovedDate IS NULL AND wm.MemberID = @MemberID
                WHERE w.WorkID = @WorkID;";

        public static string GetActiveRootOrgItems = 
            @"SELECT w.WorkID, w.ParentWorkID, w.OrgID, w.Title FROM org_member om
                JOIN org o ON o.OrgID = om.OrgID AND o.RemovedDate IS NULL
                LEFT JOIN work w ON w.OrgID = w.OrgID AND w.RemovedDate IS NULL AND w.CompleteDate IS NULL 
                AND w.ParentWorkID is null 
                JOIN work_member wm on wm.WorkID = w.WorkID AND wm.RemovedDate IS NULL AND wm.WorkID is not null
                WHERE om.MemberID = @MemberID;";

        public static string GetMyActiveRootItems =
            @"SELECT WorkID, ParentWorkID, OrgID, Title FROM work 
                WHERE Owner = @Owner AND ParentWorkID IS NULL AND CompleteDate IS NULL AND RemovedDate IS NULL;";
        
        public static string Insert =
            @"INSERT INTO `spd`.`work`
            (
                `ParentWorkID`,`OrgID`,`Title`,`Description`,`Owner`,`Size`,`Priority`,`HoursWorked`,`DueDate`,
                `CompleteDate`,`CreatedBy`,`CreatedDate`,`UpdatedBy`,`UpdatedDate`
            )
            VALUES
            (
                @ParentWorkID,@OrgID,@Title,@Description,@Owner,@Size,@Priority,@HoursWorked,@DueDate,@CompleteDate,
                @CreatedBy,NOW(),@UpdatedBy,NOW()
            );
            SELECT LAST_INSERT_ID();";

        public static string Update =
            @"UPDATE work
            SET
            `ParentWorkID` = @ParentWorkID,
            `Title` = @Title,
            `Description` = @Description,
            `Owner` = @Owner,
            `Size` = @Size,
            `Priority` = @Priority,
            `HoursWorked` = @HoursWorked,
            `DueDate` = @DueDate,
            `CompleteDate` = @CompleteDate,
            `UpdatedBy` = @UpdatedBy,
            `UpdatedDate` = NOW()
            WHERE `WorkID` = @WorkID;";


        public static string Delete =
            @"UPDATE work
            SET
            `RemovedBy` = @RemovedBy,
            `RemovedDate` = NOW()
            WHERE `WorkID` = @WorkID;";
    }
}
