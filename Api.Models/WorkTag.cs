namespace Models
{
    public class WorkTag : Model
    {
        public int WorkTagID { get; set; }
        public int WorkID { get; set; }
        public string Title { get; set; }
        public string Value { get; set; }
        public string Color { get; set; }
    }
}
