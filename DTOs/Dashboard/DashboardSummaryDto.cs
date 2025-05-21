// DTOs/Dashboard/DashboardSummaryDto.cs
namespace JabilDevPortal.Api.DTOs.Dashboard
{
  public class DashboardSummaryDto
{
    public int TotalApps { get; set; }
    public int TotalTicketsOpen { get; set; }
    public List<DeptCount> TicketsByDept { get; set; } = new();
    public List<DeptCount> AppsByDept { get; set; } = new();
    public double AvgResolutionTime { get; set; }

    public class DeptCount
    {
        public string? Department { get; set; }
        public int Count { get; set; }
        public DeptCount(string? department, int count)
        {
            Department = department;
            Count = count;
        }
    }
}

}
