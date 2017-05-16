namespace Models
{
    public class TicketChat : Model
    {
        public int TicketChatID { get; set; }
        public int TicketID { get; set; }
        public string Message { get; set; }
    }
}
