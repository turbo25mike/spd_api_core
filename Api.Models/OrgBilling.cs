using System;

namespace Models
{
    public class OrgBilling : Model
    {
        public int OrgBillingID { get; set; }
        public int OrgID { get; set; }
        public decimal AmountDue{ get; set; }
        public DateTime DateDue { get; set; }
        public int BillingMonth { get; set; }
        public int BillingYear { get; set; }
    }
}
