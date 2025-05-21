// Services/Implementations/ReportService.cs
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JabilDevPortal.Api.DTOs.Report;
using JabilDevPortal.Api.Data;
using JabilDevPortal.Api.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace JabilDevPortal.Api.Services.Implementations
{
    public class ReportService : IReportService
    {
        private readonly ApplicationDbContext _db;
        public ReportService(ApplicationDbContext db) => _db = db;

        public async Task<List<IncidentReportResultDto>> GetIncidentsAsync(DateTime from, DateTime to, string? department)
        {
            // Base query: tickets created in the specified date range
            var query = _db.Tickets
                .Include(t => t.Application)
                .Where(t => t.CreatedDate >= from && t.CreatedDate <= to)
                .AsQueryable();

            // Optional filter by application department
            if (!string.IsNullOrEmpty(department))
            {
                query = query.Where(t => t.Application.Department == department);
            }

            // Group by application and project into the DTO
            var results = await query
                .GroupBy(t => new { t.ApplicationId, t.Application.Name })
                .Select(g => new IncidentReportResultDto
                {
                    ApplicationId     = g.Key.ApplicationId,
                    ApplicationName   = g.Key.Name,
                    TotalTickets      = g.Count(),
                    OpenTickets       = g.Count(t => t.Status != "Cerrado"),
                    AvgResolutionTime = g
                        .Where(t => t.Status == "Cerrado")
                        .Select(t => (DateTime.UtcNow - t.CreatedDate).TotalHours)
                        .DefaultIfEmpty(0)
                        .Average()
                })
                .ToListAsync();

            return results;
        }
    }
}
