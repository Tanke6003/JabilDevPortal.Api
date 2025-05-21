public class Ticket
{
    public int Id { get; set; }
    public int ApplicationId { get; set; }
    public string Title { get; set; } = null!;
    public string? Description { get; set; }
    public string Status { get; set; } = null!;
    public int CreatedById { get; set; }
    public DateTime CreatedDate { get; set; }
    public int? AssignedToId { get; set; }
}