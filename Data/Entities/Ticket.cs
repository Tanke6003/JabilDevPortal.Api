// Data/Entities/Ticket.cs
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace JabilDevPortal.Api.Data.Entities
{
    public class Ticket
    {
        public int      Id             { get; set; }

        [Required]
        public int      ApplicationId  { get; set; }
        public Application Application  { get; set; } = null!;

        [Required]
        public int      CreatedById    { get; set; }
        public User     CreatedBy      { get; set; } = null!;

        [Required, MaxLength(150)]
        public string   Title          { get; set; } = null!;

        [Required]
        public string   Description    { get; set; } = null!;

        [Required]
        public DateTime CreatedDate    { get; set; } = DateTime.UtcNow;

        [Required, MaxLength(20)]
        public string   Status         { get; set; } = "Abierto"; // Abierto, EnProceso, Cerrado

        public int?     AssignedToId   { get; set; }
        public User?    AssignedTo     { get; set; }

        public ICollection<TicketComment> Comments { get; set; } = new List<TicketComment>();
    }
}
