using System;

namespace Models
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
}
