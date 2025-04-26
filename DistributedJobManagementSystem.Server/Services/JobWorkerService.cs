using DistributedJobManagementSystem.Server.SignalR;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using DistributedJobManagementSystem.Server.Models;

namespace DistributedJobManagementSystem.Server.Services;

public class JobWorkerService(
    IServiceProvider serviceProvider,
    IHubContext<JobHub> hubContext,
    ILogger<JobWorkerService> logger) : BackgroundService
{
    // Main background worker loop
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using var scope = serviceProvider.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<DistributedJobManagementSystem.Server.Data.AppDbContext>();

            // Atomically claim a pending job (highest priority first)
            var job = await db.Jobs
                .Where(j => j.Status == JobStatus.Pending)
                .OrderByDescending(j => j.Priority)
                .ThenBy(j => j.Id)
                .FirstOrDefaultAsync(stoppingToken);

            if (job != null)
            {
                try
                {
                    // Mark job as running and clear previous errors
                    job.Status = JobStatus.Running;
                    job.StartTime = DateTime.UtcNow;
                    job.Error = null;
                    db.Jobs.Update(job);
                    await db.SaveChangesAsync(stoppingToken);
                    await hubContext.Clients.All.SendAsync("JobUpdated", job, stoppingToken);

                    bool stopped = false;
                    // Simulate job progress in increments
                    for (int i = 0; i <= 100; i += 5)
                    {
                        // Check for stop request by reloading job from DB
                        var freshJob = await db.Jobs.AsNoTracking().FirstOrDefaultAsync(j => j.Id == job.Id, stoppingToken);
                        if (freshJob != null && freshJob.Status == JobStatus.Stopped)
                        {
                            // Mark as stopped and notify clients
                            freshJob.EndTime = DateTime.UtcNow;
                            db.Jobs.Update(freshJob);
                            await db.SaveChangesAsync(stoppingToken);
                            await hubContext.Clients.All.SendAsync("JobUpdated", freshJob, stoppingToken);
                            stopped = true;
                            break;
                        }

                        // Continue progress and notify clients
                        job.Progress = i;
                        db.Jobs.Update(job);
                        await db.SaveChangesAsync(stoppingToken);
                        await hubContext.Clients.All.SendAsync("JobUpdated", job, stoppingToken);
                        await Task.Delay(400, stoppingToken);
                    }

                    if (!stopped)
                    {
                        // Mark job as completed and notify clients
                        var completedJob = await db.Jobs.FirstOrDefaultAsync(j => j.Id == job.Id, stoppingToken);
                        if (completedJob != null && completedJob.Status != JobStatus.Stopped)
                        {
                            completedJob.Status = JobStatus.Completed;
                            completedJob.EndTime = DateTime.UtcNow;
                            completedJob.Progress = 100;
                            await db.SaveChangesAsync(stoppingToken);
                            await hubContext.Clients.All.SendAsync("JobUpdated", completedJob, stoppingToken);
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Log error and mark job as failed with error details
                    logger.LogError(ex, "Job {JobId} failed", job.Id);
                    job.Status = JobStatus.Failed;
                    job.Error = ex.ToString();
                    job.EndTime = DateTime.UtcNow;
                    db.Jobs.Update(job);
                    await db.SaveChangesAsync(stoppingToken);
                    await hubContext.Clients.All.SendAsync("JobUpdated", job, stoppingToken);
                }
            }
            else
            {
                // No pending jobs, wait before polling again
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
} 