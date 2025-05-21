// DTOs/Search/ApplicationSearchDto.cs
namespace JabilDevPortal.Api.DTOs.Search
{
    public class ApplicationSearchDto
    {
        public string? Query      { get; set; }
        public string? Department { get; set; }
        public int?    OwnerId    { get; set; }
    }
}
