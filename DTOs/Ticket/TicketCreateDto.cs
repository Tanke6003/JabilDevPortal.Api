
public class TicketCreateDto
{
    public int ApplicationId { get; set; }
    public string Title { get; set; } = null!;
    public string? Description { get; set; }
    public int CreatedById { get; set; }
}