namespace DistributedJobManagementSystem.Server.Models;

public class Job
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public JobPriority Priority { get; set; }
    public JobStatus Status { get; set; }
    public DateTime? StartTime { get; set; }
    public DateTime? EndTime { get; set; }
    public int Progress { get; set; }
    public string? Error { get; set; }
} 