using DistributedJobManagementSystem.Server.Data;
using DistributedJobManagementSystem.Server.SignalR;
using Microsoft.EntityFrameworkCore;
using DotNetEnv;
using DistributedJobManagementSystem.Server.Services;
using DistributedJobManagementSystem.Server;

// Load environment variables from .env file
Env.Load();
var dbServer = Environment.GetEnvironmentVariable("DB_SERVER");

var builder = WebApplication.CreateBuilder(args);

// Configure environment variable substitution
builder.Configuration.AddEnvironmentVariables();

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register EF Core with SQL Server
builder.Services.AddDbContext<AppDbContext>(options =>
{
    var connectionString = string.Format(Constants.ConnectionStringTemplate, dbServer);
    options.UseSqlServer(connectionString);
});

// Register SignalR
builder.Services.AddSignalR();

// Register custom services
builder.Services.AddHostedService<JobWorkerService>();
builder.Services.AddScoped<IJobService, JobService>();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins("http://localhost:3000")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors();

app.UseAuthorization();

app.MapControllers();
app.MapHub<JobHub>("/jobHub");

app.Run();
