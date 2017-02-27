using System;

namespace Api.DataStore
{
    public interface IModel
    {
        string PrimaryKey { get; }

        int CreatedBy { get; set; }
        DateTime CreatedDate { get; set; }
        int UpdatedBy { get; set; }
        DateTime? UpdatedDate { get; set; }
        int RemovedBy { get; set; }
        DateTime? RemovedDate { get; set; }
    }

    public class BaseModel
    {
        protected Action GetPrimaryKeyVal;
        public static string TableName;

        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public int RemovedBy { get; set; }
        public DateTime? RemovedDate { get; set; }
    }

    public class Member : BaseModel, IModel
    {
        public string PrimaryKey => nameof(MemberID);

        public int MemberID { get; set; }
        public string LoginID { get; set; }
        public string UserName { get; set; }
    }

    public class Org : BaseModel, IModel
    {
        public string PrimaryKey => nameof(OrgID);

        public int OrgID { get; set; }
        public string Name { get; set; }
        public int BillingID { get; set; }
    }

    public class OrgBilling : BaseModel, IModel
    {
        public string PrimaryKey => nameof(OrgBillingID);

        public int OrgBillingID { get; set; }
        public int OrgID { get; set; }
        public decimal AmountDue{ get; set; }
        public DateTime DateDue { get; set; }
        public int BillingMonth { get; set; }
        public int BillingYear { get; set; }
    }

    public class OrgCc : BaseModel, IModel
    {
        public string PrimaryKey => nameof(OrgCCID);

        public int OrgCCID { get; set; }
        public int OrgID { get; set; }
        public int CreditCardNumber { get; set; }
    }

    public class OrgMember : BaseModel, IModel
    {
        public string PrimaryKey => nameof(OrgMemberID);

        public int OrgMemberID { get; set; }
        public int OrgID { get; set; }
        public int MemberID { get; set; }
    }

    public class Tag : BaseModel, IModel
    {
        public string PrimaryKey => nameof(TagID);

        public int TagID { get; set; }
        public int Name { get; set; }
    }

    public class OrgWork : BaseModel, IModel
    {
        public string PrimaryKey => nameof(OrgWorkID);

        public int OrgWorkID { get; set; }
        public int OrgID { get; set; }
        public int WorkID { get; set; }
    }

    public class Work : BaseModel, IModel
    {
        public string PrimaryKey => nameof(WorkID);

        public int WorkID { get; set; }
        public int ParentWorkID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int Owner { get; set; }
        public int Size { get; set; }
        public int Priority { get; set; }
        public double HoursWorked { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime CompleteDate { get; set; }
    }

    public class WorkMember : BaseModel, IModel
    {
        public string PrimaryKey => nameof(WorkMemberID);

        public int WorkMemberID { get; set; }
        public int WorkID { get; set; }
    }

    public class WorkTag : BaseModel, IModel
    {
        public string PrimaryKey => nameof(WorkTagID);

        public int WorkTagID { get; set; }
        public int WorkID { get; set; }
        public int TagID { get; set; }
    }

    public class WorkStatus : BaseModel, IModel
    {
        public string PrimaryKey => nameof(WorkStatusID);

        public int WorkStatusID { get; set; }
        public int WorkID { get; set; }
        public string Description { get; set; }
    }
}
