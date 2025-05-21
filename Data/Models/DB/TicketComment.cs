public class TicketComment
{
    public int Id { get; set; }
    public int TicketId { get; set; }
    public int UserId { get; set; } // Es el autor
    public string Comment { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
    public bool Available { get; set; }
}
