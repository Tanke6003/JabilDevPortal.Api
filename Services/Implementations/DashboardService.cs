// Services/Implementations/DashboardService.cs
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using JabilDevPortal.Api.DTOs.Dashboard;
using JabilDevPortal.Api.Data;
using JabilDevPortal.Api.Data.Entities;
using JabilDevPortal.Api.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace JabilDevPortal.Api.Services.Implementations
{
    public class DashboardService : IDashboardService
    {
        private readonly ApplicationDbContext _db;
        public DashboardService(ApplicationDbContext db) => _db = db;

        public async Task<DashboardSummaryDto> GetSummaryAsync()
        {
            // Total de aplicaciones
            var totalApps = await _db.Applications.CountAsync();

            // Total de tickets abiertos
            var totalTicketsOpen = await _db.Tickets
                .CountAsync(t => t.Status == "Abierto");

            // Tickets abiertos por departamento (según la aplicación asociada)
            var ticketsByDept = await _db.Tickets
                .Include(t => t.Application)
                .Where(t => t.Status == "Abierto")
                .GroupBy(t => t.Application.Department)
                .Select(g => new DashboardSummaryDto.DeptCount(g.Key, g.Count()))
                .ToListAsync();

            // Aplicaciones por departamento
            var appsByDept = await _db.Applications
                .GroupBy(a => a.Department)
                .Select(g => new DashboardSummaryDto.DeptCount(g.Key, g.Count()))
                .ToListAsync();

            // Tiempo promedio de resolución (horas) para tickets cerrados
            var closedTickets = await _db.Tickets
                .Where(t => t.Status == "Cerrado")
                .ToListAsync();

            double avgResolutionTime = 0;
            if (closedTickets.Any())
            {
                avgResolutionTime = closedTickets
                    .Average(t => (DateTime.UtcNow - t.CreatedDate).TotalHours);
            }

            return new DashboardSummaryDto
            {
                TotalApps         = totalApps,
                TotalTicketsOpen  = totalTicketsOpen,
                TicketsByDept     = ticketsByDept,
                AppsByDept        = appsByDept,
                AvgResolutionTime = avgResolutionTime
            };
        }
    }
}
