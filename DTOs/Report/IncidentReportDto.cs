    // DTOs/Report/IncidentReportDto.cs
namespace JabilDevPortal.Api.DTOs.Report
{
    public class IncidentReportDto
    {
        public DateTime From       { get; set; }
        public DateTime To         { get; set; }
        public string?   Department { get; set; }
    }
}
