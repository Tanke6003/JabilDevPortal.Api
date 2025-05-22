public class ApplicationCreateDto
{
    public string Name { get; set; } 
    public string? Description { get; set; }
    public string? Url { get; set; }
    public string? Server { get; set; }
    public string? Repository { get; set; }
    public int? OwnerId { get; set; }
    public string SupportEmail { get; set; } 
    public string? SmeEmail { get; set; }
    public string? DbServer { get; set; }
    public bool Available { get; set; } = true;
}
