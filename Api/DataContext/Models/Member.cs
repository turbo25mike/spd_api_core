using System;

namespace Api.DataContext.Models
{
    public class Member: IModel
    {
        public long MemberID { get; set; }
        public long LoginID { get; set; }
        public string UserName { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public int RemovedBy { get; set; }
        public DateTime? RemovedDate { get; set; }
    }
}
