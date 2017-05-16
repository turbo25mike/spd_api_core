using System;
using System.Collections.Generic;

namespace Models
{
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
        public int? OwnerID { get; set; }
        public string Status { get; set; }
        public int? Size { get; set; }
        public int? Priority { get; set; }
        public double? HoursWorked { get; set; }
        public DateTime? DueDate { get; set; }
        public DateTime? CompleteDate { get; set; }

        public List<Work> Children { get; set; }
        public List<WorkTag> Tags { get; set; }
    }
}
