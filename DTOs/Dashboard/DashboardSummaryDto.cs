// DTOs/Dashboard/DashboardSummaryDto.cs
namespace JabilDevPortal.Api.DTOs.Dashboard
{
    public class DashboardSummaryDto
    {
        public int    TotalApps        { get; set; }
        public int    TotalTicketsOpen { get; set; }
        public IEnumerable<DeptCount> TicketsByDept { get; set; } = null!;
        public IEnumerable<DeptCount> AppsByDept    { get; set; } = null!;
        public double AvgResolutionTime { get; set; }

        public record DeptCount(string Department, int Count);
    }
}
