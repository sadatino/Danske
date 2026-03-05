using Danske.Application.Interfaces;
using Danske.Application.Services;
using Danske.Application.Services.TaxStrategy;
using Danske.Domain.Interfaces.Repositories;
using Danske.Infrastructure.Persistence;
using Danske.Infrastructure.Repositories;
using Danske.Middleware;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// services
builder.Services.AddScoped<ITaxService, TaxService>();
builder.Services.AddScoped<IMunicipalityService, MunicipalityService>();
builder.Services.AddScoped<ITaxRateService, TaxRateService>();

builder.Services.AddScoped<ITaxStrategy, DailyTaxStrategy>();
builder.Services.AddScoped<ITaxStrategy, WeeklyTaxStrategy>();
builder.Services.AddScoped<ITaxStrategy, MonthlyTaxStrategy>();
builder.Services.AddScoped<ITaxStrategy, YearlyTaxStrategy>();

builder.Services.AddScoped<TaxStrategyResolver>();

builder.Services.AddScoped<IMunicipalityRepository, MunicipalityRepository>();
builder.Services.AddScoped<ITaxRepository, TaxRepository>();

// Add services to the container.
builder.Services.AddControllers();

// Add Swagger services
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.EnableAnnotations();
});

var connectionString = builder.Configuration.GetConnectionString("DanskeDb");

var dbPath = connectionString?.Replace("Data Source=", "");
var dbDir = Path.GetDirectoryName(Path.GetFullPath(dbPath!));
if (!string.IsNullOrEmpty(dbDir))
{
    Directory.CreateDirectory(dbDir);
}

Console.WriteLine($"connectionString:{connectionString},\n dbPath: {dbPath}, \n dbDir: {dbDir}");

builder.Services.AddDbContext<DanskeDbContext>(options =>
    options.UseSqlite(connectionString));

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<DanskeDbContext>();
    db.Database.EnsureCreated();
}

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseMiddleware<ExceptionMiddleware>();

app.UseAuthorization();

app.MapControllers();

app.Run();