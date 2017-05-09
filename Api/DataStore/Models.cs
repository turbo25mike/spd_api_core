using System;
using System.Collections.Generic;

namespace Api.DataStore
{
    public interface IModel
    {
        int CreatedBy { get; set; }
        DateTime CreatedDate { get; set; }
        int UpdatedBy { get; set; }
        DateTime? UpdatedDate { get; set; }
        int? RemovedBy { get; set; }
        DateTime? RemovedDate { get; set; }
    }

    public class Model: IModel
    {
        public string CreatedByName { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string UpdatedByName { get; set; }
        public int UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string RemovedByName { get; set; }
        public int? RemovedBy { get; set; }
        public DateTime? RemovedDate { get; set; }
    }

    public class Member : Model
    {
        public int MemberID { get; set; }
        public string LoginID { get; set; }
        public string UserName { get; set; }
    }

    public class MemberForecast : Model
    {
        public int MemberForecastID { get; set; }
        public int WorkID { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }

    public class Org : Model
    {
        public int OrgID { get; set; }
        public string Name { get; set; }
        public int? BillingID { get; set; }
    }

    public class OrgBilling : Model
    {
        public int OrgBillingID { get; set; }
        public int OrgID { get; set; }
        public decimal AmountDue{ get; set; }
        public DateTime DateDue { get; set; }
        public int BillingMonth { get; set; }
        public int BillingYear { get; set; }
    }

    public class OrgCc : Model
    {
        public int OrgCCID { get; set; }
        public int OrgID { get; set; }
        public int CreditCardNumber { get; set; }
    }

    public class OrgGoal : Model
    {
        public int OrgGoalID { get; set; }
        public int OrgID { get; set; }
        public int ParentOrgGoalID { get; set; }
        public string Goal { get; set; }
        public bool BasedOnWork { get; set; }
        public int SuccessPercentCriteria { get; set; }
    }

    public class OrgMember : Model
    {
        public int OrgMemberID { get; set; }
        public int OrgID { get; set; }
        public int MemberID { get; set; }
    }

    public class OrgRole : Model
    {
        public int OrgRoleID { get; set; }
        public int OrgID { get; set; }
        public string Role { get; set; }
    }

    public class OrgWork : Model
    {
        public int OrgWorkID { get; set; }
        public int OrgID { get; set; }
        public int WorkID { get; set; }
    }

    public class Work : Model
    {
        public Work()
        {
            Children = new List<Work>();
        }

        public string OrgName { get; set; }
        public int WorkID { get; set; }
        public int? ParentWorkID { get; set; }
        public int? OrgID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int? Owner { get; set; }
        public string Status { get; set; }
        public int? Size { get; set; }
        public int? Priority { get; set; }
        public double? HoursWorked { get; set; }
        public DateTime? DueDate { get; set; }
        public DateTime? CompleteDate { get; set; }

        public List<Work> Children { get; set; }
        public List<WorkTag> Tags { get; set; }
    }

    public class WorkMember : Model
    {
        public int WorkMemberID { get; set; }
        public int WorkID { get; set; }
        public int OrgRoleID { get; set; }
        public int MemberID { get; set; }
    }

    public class WorkTag : Model
    {
        public int WorkTagID { get; set; }
        public int WorkID { get; set; }
        public string TagName { get; set; }
        public string TagValue { get; set; }
    }


    public class Ticket : Model
    {
        public int TicketID { get; set; }
        public int WorkID { get; set; }
    }

    public class TicketChat : Model
    {
        public int TicketChatID { get; set; }
        public int TicketID { get; set; }
        public string Message { get; set; }
    }

    public class WorkChat : Model
    {
        public int WorkChatID { get; set; }
        public int WorkID { get; set; }
        public string Message { get; set; }
    }
}
