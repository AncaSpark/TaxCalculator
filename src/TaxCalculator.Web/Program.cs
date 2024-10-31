using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Events;
using TaxCalculator.Domain.Interfaces;
using TaxCalculator.Infrastructure.Data.Repository;
using TaxCalculator.Infrastructure.Data;
using TaxCalculator.Application.Services;
using Microsoft.Extensions.Logging;

var builder = WebApplication.CreateBuilder(args);

// Configure Serilog
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.File("logs/tax-calculator-.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Host.UseSerilog();

// Add services to the container.
builder.Services.AddControllersWithViews();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
var migrationAssembly = typeof(TaxCalculatorDbContext).Assembly.FullName;

builder.Services.AddDbContext<TaxCalculatorDbContext>(options =>
    options.UseSqlServer(
        connectionString,
        sqlOptions =>
        {
            sqlOptions.EnableRetryOnFailure(
                maxRetryCount: 3,
                maxRetryDelay: TimeSpan.FromSeconds(30),
                errorNumbersToAdd: null);
            sqlOptions.MigrationsAssembly(migrationAssembly);
        }));
       

builder.Services.AddScoped<ITaxBandRepository, TaxBandRepository>();

builder.Services.AddScoped<ITaxCalculatorService, TaxCalculatorService>();


var app = builder.Build();


// Configure error handling middleware
app.UseExceptionHandler("/Error");

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=TaxCalculator}/{action=Index}/{id?}");

app.Run();
