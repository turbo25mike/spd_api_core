using System;

namespace Models
{
    public class MemberForecast : Model
    {
        public int MemberForecastID { get; set; }
        public int WorkID { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
