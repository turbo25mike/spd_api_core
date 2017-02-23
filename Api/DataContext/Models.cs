using System;
using System.Collections.Generic;
using System.Reflection;

namespace Api.DataContext
{
    public interface IModel
    {
        string PrimaryKey { get;}
        void SetPrimaryKey(int id);

        int CreatedBy { get; set; }
        DateTime CreatedDate { get; set; }
        int UpdatedBy { get; set; }
        DateTime? UpdatedDate { get; set; }
        int RemovedBy { get; set; }
        DateTime? RemovedDate { get; set; }

        object GetValue(string propertyName);
        Dictionary<string, object> CreateSet(string[] setProps);
    }

    public class BaseModel : IModel
    {
        public static string TableName;

        public string PrimaryKey => "Set_In_Model"; 
        public void SetPrimaryKey(int id){ throw new NotImplementedException(); }

        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public int RemovedBy { get; set; }
        public DateTime? RemovedDate { get; set; }

        public object GetValue(string propertyName)
        {
            return GetType().GetProperty(propertyName).GetValue(this);
        }

        public Dictionary<string, object> CreateSet(string[] setProps)
        {
            var set = new Dictionary<string, object>();
            if (setProps == null) return null;
            foreach (var prop in setProps)
                set.Add(prop, GetValue(prop));
            return set;
        }
    }

    public class Member : BaseModel
    {
        public new string PrimaryKey => nameof(MemberID);
        internal new void SetPrimaryKey(int id) { MemberID = id; }

        public int MemberID { get; set; }
        public string LoginID { get; set; }
        public string UserName { get; set; }
    }

    public class Org : BaseModel
    {
        public new string PrimaryKey => nameof(OrgID);
        internal new void SetPrimaryKey(int id) { OrgID = id; }
        
        public int OrgID { get; set; }
        public string Name { get; set; }
        public int BillingID { get; set; }
    }

    public class OrgBilling : BaseModel
    {
        public new string PrimaryKey => nameof(OrgBillingID);
        internal new void SetPrimaryKey(int id) { OrgBillingID = id; }

        public int OrgBillingID { get; set; }
        public int OrgID { get; set; }
        public decimal AmountDue{ get; set; }
        public DateTime DateDue { get; set; }
        public int BillingMonth { get; set; }
        public int BillingYear { get; set; }
    }

    public class OrgCc : BaseModel
    {
        public new string PrimaryKey => nameof(OrgCCID);
        internal new void SetPrimaryKey(int id) { OrgCCID = id; }

        public int OrgCCID { get; set; }
        public int OrgID { get; set; }
        public int CreditCardNumber { get; set; }
    }

    public class OrgMember : BaseModel
    {
        public new string PrimaryKey => nameof(OrgMemberID);
        internal new void SetPrimaryKey(int id) { OrgMemberID = id; }

        public int OrgMemberID { get; set; }
        public int OrgID { get; set; }
        public int MemberID { get; set; }
    }

    public class Tag : BaseModel
    {
        public new string PrimaryKey => nameof(TagID);
        internal new void SetPrimaryKey(int id) { TagID = id; }

        public int TagID { get; set; }
        public int Name { get; set; }
    }

    public class OrgWork : BaseModel
    {
        public new string PrimaryKey => nameof(OrgID);
        internal new void SetPrimaryKey(int id) { OrgID = id; }

        public int OrgID { get; set; }
        public int WorkID { get; set; }
    }

    public class Work : BaseModel
    {
        public new string PrimaryKey => nameof(WorkID);
        internal new void SetPrimaryKey(int id) { WorkID = id; }

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

    public class WorkMember : BaseModel
    {
        public new string PrimaryKey => nameof(WorkMemberID);
        internal new void SetPrimaryKey(int id) { WorkMemberID = id; }

        public int WorkMemberID { get; set; }
        public int WorkID { get; set; }
    }

    public class WorkTag : BaseModel
    {
        public new string PrimaryKey => nameof(WorkTagID);
        internal new void SetPrimaryKey(int id) { WorkTagID = id; }

        public int WorkTagID { get; set; }
        public int WorkID { get; set; }
        public int TagID { get; set; }
    }

    public class WorkStatus : BaseModel
    {
        public new string PrimaryKey => nameof(WorkStatusID);
        internal new void SetPrimaryKey(int id) { WorkStatusID = id; }

        public int WorkStatusID { get; set; }
        public int WorkID { get; set; }
        public string Description { get; set; }
    }
}
