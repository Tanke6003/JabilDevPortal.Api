// Data/Entities/TicketComment.cs
using System;
using System.ComponentModel.DataAnnotations;

namespace JabilDevPortal.Api.Data.Entities
{
    public class TicketComment
    {
        public int      Id        { get; set; }

        [Required]
        public int      TicketId  { get; set; }
        public Ticket   Ticket    { get; set; } = null!;

        [Required]
        public int      AuthorId  { get; set; }
        public User     Author    { get; set; } = null!;

        [Required]
        public string   Comment   { get; set; } = null!;

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
