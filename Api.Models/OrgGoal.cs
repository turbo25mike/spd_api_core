namespace Models
{
    public class OrgGoal : Model
    {
        public int OrgGoalID { get; set; }
        public int OrgID { get; set; }
        public int ParentOrgGoalID { get; set; }
        public string Goal { get; set; }
        public bool BasedOnWork { get; set; }
        public int SuccessPercentCriteria { get; set; }
    }
}
