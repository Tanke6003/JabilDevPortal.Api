// DTOs/Application/ApplicationCreateDto.cs
namespace JabilDevPortal.Api.DTOs.Application
{
    public class ApplicationCreateDto
    {
        public string Name        { get; set; } = null!;
        public string Url         { get; set; } = null!;
        public string Description { get; set; } = null!;   // ← nuevo
        
        public string DbServer { get; set; } = null!;
        public string DbName      { get; set; } = null!;
        public string RepoUrl     { get; set; } = null!;
        public string Version     { get; set; } = null!;
        public int    OwnerUserId { get; set; }
        public int    SmeUserId   { get; set; }
        public string SupportEmail{ get; set; } = null!;
        public string Department  { get; set; } = null!;
    }
}
