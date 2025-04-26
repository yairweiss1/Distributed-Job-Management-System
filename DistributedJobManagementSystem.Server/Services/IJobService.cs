using DistributedJobManagementSystem.Server.Models;

namespace DistributedJobManagementSystem.Server.Services;

public interface IJobService
{
    Task<IEnumerable<Job>> GetJobsAsync();
    Task<Job?> GetJobByIdAsync(int id);
    Task<Job> CreateJobAsync(Job job);
    Task<Job?> UpdateJobAsync(Job job);
    Task<bool> DeleteJobAsync(int id);
    Task<bool> StopJobAsync(int id);
    Task<bool> RestartJobAsync(int id);
} 