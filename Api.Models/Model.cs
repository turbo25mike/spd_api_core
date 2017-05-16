using System;

namespace Models
{
    public class Model: IModel
    {
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public int? RemovedBy { get; set; }
        public DateTime? RemovedDate { get; set; }
    }
}
