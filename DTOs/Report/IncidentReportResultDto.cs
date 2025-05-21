public class IncidentReportResultDto
{
    public int ApplicationId { get; set; }
    public string ApplicationName { get; set; } = null!;
    public int TotalTickets { get; set; }
    public int OpenTickets { get; set; }
    public double AvgResolutionTime { get; set; }
}
