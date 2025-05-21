using System.Data;
using BackEnd.Infrastructure.Plugins;
using JabilDevPortal.Api.DTOs.Report;
using JabilDevPortal.Api.Services.Interfaces;

public class ReportService : IReportService
{
    private readonly PGSQLConnectionPlugin _db;
    public ReportService(PGSQLConnectionPlugin db) => _db = db;

    public async Task<List<IncidentReportResultDto>> GetIncidentsAsync(DateTime from, DateTime to, string? department)
    {
        // Construir filtro de fechas y departamento
        var sql = $@"
            SELECT 
                t.application_id AS application_id,
                a.name AS application_name,
                COUNT(*) AS total_tickets,
                COUNT(*) FILTER (WHERE t.status != 'Cerrado') AS open_tickets,
                COALESCE(AVG(EXTRACT(EPOCH FROM (t.updated_at - t.created_at))/3600) FILTER (WHERE t.status = 'Cerrado' AND t.updated_at IS NOT NULL), 0) AS avg_resolution_time
            FROM tickets t
            INNER JOIN applications a ON t.application_id = a.id
            WHERE t.created_at >= '{from:yyyy-MM-dd HH:mm:ss}' AND t.created_at <= '{to:yyyy-MM-dd HH:mm:ss}' 
                  AND t.available = true AND a.available = true
        ";

        if (!string.IsNullOrEmpty(department))
            sql += $" AND a.department = '{department.Replace("'", "''")}' ";

        sql += " GROUP BY t.application_id, a.name ORDER BY a.name;";

        var dt = await Task.Run(() => _db.ExecDataTable(sql));

        var list = new List<IncidentReportResultDto>();
        foreach (DataRow row in dt.Rows)
        {
            list.Add(new IncidentReportResultDto
            {
                ApplicationId     = Convert.ToInt32(row["application_id"]),
                ApplicationName   = row["application_name"].ToString()!,
                TotalTickets      = Convert.ToInt32(row["total_tickets"]),
                OpenTickets       = Convert.ToInt32(row["open_tickets"]),
                AvgResolutionTime = Convert.ToDouble(row["avg_resolution_time"])
            });
        }
        return list;
    }
}
