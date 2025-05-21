// DTOs/Ticket/TicketCreateDto.cs
namespace JabilDevPortal.Api.DTOs.Ticket
{
    public class TicketCreateDto
    {
        public int ApplicationId { get; set; }
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        // Nuevo: indicamos quién crea el ticket
        public int    CreatedById   { get; set; }
    }
}
