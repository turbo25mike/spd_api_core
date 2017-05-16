namespace Models
{
    public class WorkChat : Model
    {
        public int WorkChatID { get; set; }
        public int WorkID { get; set; }
        public string Message { get; set; }
    }
}
