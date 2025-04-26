using Microsoft.AspNetCore.Mvc;
using DistributedJobManagementSystem.Server.Models;
using DistributedJobManagementSystem.Server.Services;

namespace DistributedJobManagementSystem.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class JobsController(IJobService jobService) : ControllerBase
{
    private readonly IJobService _jobService = jobService;

    [HttpGet]
    public async Task<IActionResult> GetJobs() => Ok(await _jobService.GetJobsAsync());

    [HttpPost]
    public async Task<IActionResult> CreateJob([FromBody] Job job) => Ok(await _jobService.CreateJobAsync(job));

    [HttpPost("{id}/stop")]
    public async Task<IActionResult> StopJob(int id) => Ok(await _jobService.StopJobAsync(id));

    [HttpPost("{id}/restart")]
    public async Task<IActionResult> RestartJob(int id) => Ok(await _jobService.RestartJobAsync(id));

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteJob(int id) => Ok(await _jobService.DeleteJobAsync(id));
} 