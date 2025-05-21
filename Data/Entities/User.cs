// Data/Entities/User.cs
using System.ComponentModel.DataAnnotations;

namespace JabilDevPortal.Api.Data.Entities
{
    public class User
    {
        public int    Id           { get; set; }

        [Required, MaxLength(50)]
        public string Username     { get; set; } = null!;

        [Required]
        public string PasswordHash { get; set; } = null!;

        [Required, MaxLength(100)]
        public string FullName     { get; set; } = null!;

        [Required, EmailAddress]
        public string Email        { get; set; } = null!;

        [Required, MaxLength(20)]
        public string Department   { get; set; } = null!; // TE, IE, ME, AUT, Manufacturing

        [Required, MaxLength(20)]
        public string Role         { get; set; } = "Viewer"; // Admin, Developer, Support, Viewer
    }
}
