// Services/Implementations/DashboardService.cs
using System.Data;
using BackEnd.Infrastructure.Plugins;
using JabilDevPortal.Api.DTOs.Dashboard;

using JabilDevPortal.Api.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace JabilDevPortal.Api.Services.Implementations
{
public class DashboardService : IDashboardService
{
    private readonly PGSQLConnectionPlugin _db;
    public DashboardService(PGSQLConnectionPlugin db) => _db = db;

    public async Task<DashboardSummaryDto> GetSummaryAsync()
    {
        // Total de aplicaciones
        int totalApps = 0;
        var dtApps = await Task.Run(() => _db.ExecDataTable("SELECT COUNT(*) AS total FROM applications WHERE available = true"));
        if (dtApps.Rows.Count > 0)
            totalApps = Convert.ToInt32(dtApps.Rows[0]["total"]);

        // Total de tickets abiertos
        int totalTicketsOpen = 0;
        var dtOpenTickets = await Task.Run(() => _db.ExecDataTable("SELECT COUNT(*) AS total FROM tickets WHERE status = 'Abierto' AND available = true"));
        if (dtOpenTickets.Rows.Count > 0)
            totalTicketsOpen = Convert.ToInt32(dtOpenTickets.Rows[0]["total"]);

        // Tickets abiertos por departamento (según la aplicación asociada)
        // Ajusta el JOIN si tus tablas usan otro nombre o no tienen departamento
        var ticketsByDept = new List<DashboardSummaryDto.DeptCount>();
        var dtTicketsByDept = await Task.Run(() => _db.ExecDataTable(@"
            SELECT a.department AS department, COUNT(*) AS count
            FROM tickets t
            INNER JOIN applications a ON t.application_id = a.id
            WHERE t.status = 'Abierto' AND t.available = true AND a.available = true
            GROUP BY a.department
        "));
        foreach (DataRow row in dtTicketsByDept.Rows)
        {
            ticketsByDept.Add(new DashboardSummaryDto.DeptCount(
                row["department"] == DBNull.Value ? null : row["department"].ToString(),
                Convert.ToInt32(row["count"])
            ));
        }

        // Aplicaciones por departamento
        var appsByDept = new List<DashboardSummaryDto.DeptCount>();
        var dtAppsByDept = await Task.Run(() => _db.ExecDataTable(@"
            SELECT department, COUNT(*) AS count
            FROM applications
            WHERE available = true
            GROUP BY department
        "));
        foreach (DataRow row in dtAppsByDept.Rows)
        {
            appsByDept.Add(new DashboardSummaryDto.DeptCount(
                row["department"] == DBNull.Value ? null : row["department"].ToString(),
                Convert.ToInt32(row["count"])
            ));
        }

        // Tiempo promedio de resolución (horas) para tickets cerrados
        double avgResolutionTime = 0;
        var dtClosedTickets = await Task.Run(() => _db.ExecDataTable(@"
            SELECT created_at, updated_at
            FROM tickets
            WHERE status = 'Cerrado' AND available = true AND updated_at IS NOT NULL
        "));
        if (dtClosedTickets.Rows.Count > 0)
        {
            var totalHours = dtClosedTickets.Rows
                .Cast<DataRow>()
                .Select(row =>
                {
                    var created = Convert.ToDateTime(row["created_at"]);
                    var closed = Convert.ToDateTime(row["updated_at"]);
                    return (closed - created).TotalHours;
                }).Average();
            avgResolutionTime = totalHours;
        }

        return new DashboardSummaryDto
        {
            TotalApps = totalApps,
            TotalTicketsOpen = totalTicketsOpen,
            TicketsByDept = ticketsByDept,
            AppsByDept = appsByDept,
            AvgResolutionTime = avgResolutionTime
        };
    }
}
}
