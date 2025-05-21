// Data/Entities/Application.cs
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JabilDevPortal.Api.Data.Entities
{
    public class Application
    {
        public int      Id           { get; set; }

        [Required, MaxLength(100)]
        public string   Name         { get; set; } = null!;

        [Required, Url]
        public string   Url          { get; set; } = null!;

        [Required]
        public string   Description  { get; set; } = null!;  // Nueva propiedad para búsqueda

        [Required, MaxLength(100)]
        public string   DbServer     { get; set; } = null!;

        [Required, MaxLength(100)]
        public string   DbName       { get; set; } = null!;

        [Required, Url]
        public string   RepoUrl      { get; set; } = null!;

        [Required, MaxLength(20)]
        public string   Version      { get; set; } = null!;

        [Required]
        public int      OwnerUserId  { get; set; }
        public User     OwnerUser    { get; set; } = null!;

        [Required]
        public int      SmeUserId    { get; set; }
        public User     SmeUser      { get; set; } = null!;

        [Required, EmailAddress]
        public string   SupportEmail { get; set; } = null!;

        [Required, MaxLength(20)]
        public string   Department   { get; set; } = null!;

        [Required]
        public DateTime CreatedAt    { get; set; } = DateTime.UtcNow;

        [Required]
        public DateTime UpdatedAt    { get; set; } = DateTime.UtcNow;
    }
}
