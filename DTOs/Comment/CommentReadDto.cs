namespace JabilDevPortal.Api.DTOs.Comment
{
    public class CommentReadDto
    {
        public int      Id        { get; set; }
        public int      TicketId  { get; set; }
        public int      AuthorId  { get; set; }
        public string   AuthorName{ get; set; } = null!;
        public string   Comment   { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
    }
}
