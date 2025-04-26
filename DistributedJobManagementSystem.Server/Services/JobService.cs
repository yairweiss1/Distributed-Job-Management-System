using DistributedJobManagementSystem.Server.Data;
using Microsoft.EntityFrameworkCore;
using DistributedJobManagementSystem.Server.Models;

namespace DistributedJobManagementSystem.Server.Services;

public class JobService(AppDbContext db) : IJobService
{
    public async Task<IEnumerable<Job>> GetJobsAsync() => await db.Jobs.ToListAsync();

    public async Task<Job?> GetJobByIdAsync(int id) => await db.Jobs.FindAsync(id);

    public async Task<Job> CreateJobAsync(Job job)
    {
        job.Status = JobStatus.Pending;
        job.Progress = 0;
        db.Jobs.Add(job);
        await db.SaveChangesAsync();
        return job;
    }

    public async Task<Job?> UpdateJobAsync(Job job)
    {
        db.Jobs.Update(job);
        await db.SaveChangesAsync();
        return job;
    }

    public async Task<bool> DeleteJobAsync(int id)
    {
        var job = await db.Jobs.FindAsync(id);
        if (job == null) return false;
        db.Jobs.Remove(job);
        await db.SaveChangesAsync();
        return true;
    }

    public async Task<bool> StopJobAsync(int id)
    {
        var job = await db.Jobs.FindAsync(id);
        if (job == null || job.Status != JobStatus.Running) return false;
        job.Status = JobStatus.Stopped;
        await db.SaveChangesAsync();
        return true;
    }

    public async Task<bool> RestartJobAsync(int id)
    {
        var job = await db.Jobs.FindAsync(id);
        if (job == null || (job.Status != JobStatus.Failed && job.Status != JobStatus.Stopped)) return false;
        job.Status = JobStatus.Pending;
        job.Progress = 0;
        await db.SaveChangesAsync();
        return true;
    }
} 