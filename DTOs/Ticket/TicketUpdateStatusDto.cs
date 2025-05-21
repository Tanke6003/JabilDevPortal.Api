// DTOs/Ticket/TicketUpdateStatusDto.cs
namespace JabilDevPortal.Api.DTOs.Ticket
{
    public class TicketUpdateStatusDto
    {
        public string Status { get; set; } = null!;  // “EnProceso” o “Cerrado”
    }
}
