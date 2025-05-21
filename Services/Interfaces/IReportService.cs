    // Services/Interfaces/IReportService.cs
using JabilDevPortal.Api.DTOs.Report;

namespace JabilDevPortal.Api.Services.Interfaces
{
    public interface IReportService
    {
        Task<List<IncidentReportResultDto>> GetIncidentsAsync(DateTime from, DateTime to, string? department);
    }
}