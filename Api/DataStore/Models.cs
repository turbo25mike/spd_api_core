using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

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

        object GetValue(string propertyName);
        void SetValue(string propertyName, object val);
        Dictionary<string, object> CreateSet(string[] setProps = null);
    }

    public class BaseModel : IModel
    {
        protected Action GetPrimaryKeyVal;
        public static string TableName;

        public string PrimaryKey
        {
            get { throw new NotImplementedException(); }
        }

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

        public void SetValue(string propertyName, object val)
        {
            GetType().GetProperty(propertyName).SetValue(this, val);
        }

        public Dictionary<string, object> CreateSet(string[] setProps = null)
        {
            if (setProps == null)
            {
                setProps = (from prop in GetType().GetProperties()
                           where
                             prop.CanWrite &&
                             prop.Name != "CreatedBy" && prop.Name != "CreatedDate" &&
                             prop.Name != "UpdatedBy" && prop.Name != "UpdatedDate" &&
                             prop.Name != "RemovedBy" && prop.Name != "RemovedDate"
                           select prop.Name).ToArray();
            }
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

        public int MemberID { get; set; }
        public string LoginID { get; set; }
        public string UserName { get; set; }
    }

    public class Org : BaseModel
    {
        public new string PrimaryKey => nameof(OrgID);

        public int OrgID { get; set; }
        public string Name { get; set; }
        public int BillingID { get; set; }
    }

    public class OrgBilling : BaseModel
    {
        public new string PrimaryKey => nameof(OrgBillingID);

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

        public int OrgCCID { get; set; }
        public int OrgID { get; set; }
        public int CreditCardNumber { get; set; }
    }

    public class OrgMember : BaseModel
    {
        public new string PrimaryKey => nameof(OrgMemberID);

        public int OrgMemberID { get; set; }
        public int OrgID { get; set; }
        public int MemberID { get; set; }
    }

    public class Tag : BaseModel
    {
        public new string PrimaryKey => nameof(TagID);

        public int TagID { get; set; }
        public int Name { get; set; }
    }

    public class OrgWork : BaseModel
    {
        public new string PrimaryKey => nameof(OrgWorkID);

        public int OrgWorkID { get; set; }
        public int OrgID { get; set; }
        public int WorkID { get; set; }
    }

    public class Work : BaseModel
    {
        public new string PrimaryKey => nameof(WorkID);

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

        public int WorkMemberID { get; set; }
        public int WorkID { get; set; }
    }

    public class WorkTag : BaseModel
    {
        public new string PrimaryKey => nameof(WorkTagID);

        public int WorkTagID { get; set; }
        public int WorkID { get; set; }
        public int TagID { get; set; }
    }

    public class WorkStatus : BaseModel
    {
        public new string PrimaryKey => nameof(WorkStatusID);

        public int WorkStatusID { get; set; }
        public int WorkID { get; set; }
        public string Description { get; set; }
    }
}
