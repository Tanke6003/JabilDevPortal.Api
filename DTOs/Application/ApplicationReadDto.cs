// DTOs/Application/ApplicationReadDto.cs
namespace JabilDevPortal.Api.DTOs.Application
{
    public class ApplicationReadDto
{
    public int    Id          { get; set; }
    public string Name        { get; set; } = null!;
    public string Url         { get; set; } = null!;
    public string Description { get; set; } = null!;   // ← nuevo
    public string Version     { get; set; } = null!;
    public string OwnerName   { get; set; } = null!;
    public string SmeName     { get; set; } = null!;
    public string Department  { get; set; } = null!;
}

}
