// Helpers/JwtSettings.cs
namespace JabilDevPortal.Api.Helpers
{
    public class JwtSettings
    {
        public string Secret       { get; set; } = null!;
        public int    ExpiryMinutes { get; set; }
        public string Issuer       { get; set; } = null!;
        public string Audience     { get; set; } = null!;
    }
}