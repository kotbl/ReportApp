using API.Worker;
using BLL;
using BLL.Services;
using DAL;
using Domain.Options;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

IConfiguration configuration = builder.Configuration;
builder.Services.AddDbContext<ApiContext>(options =>
    options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")), ServiceLifetime.Transient);
builder.Services.Configure<ReportProcessingOption>(configuration.GetSection("ReportProcessingOption"));
builder.Services.AddHostedService<BackgroundWorker>();
builder.Services.AddSingleton<BackgroundWorkerQueue>();
builder.Services.AddBllServices();
builder.Services.Configure<RouteOptions>(x => x.LowercaseUrls = true);
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
