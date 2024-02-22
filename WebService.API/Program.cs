using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;
using WebService.API.Extensions;
using WebService.Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args);

// Add Configuration
var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true)
    .AddEnvironmentVariables()
    .Build();

// Register DBContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"), 
    b => b.MigrationsAssembly("WebService.Infrastructure")));

// Register Services
builder.Services.RegisterService();
builder.Services.RegisterMapperService();

builder.Logging.AddConsole();
builder.Logging.AddDebug();

builder.Logging.SetMinimumLevel(LogLevel.Information); // Set the minimum level for logging

// Configure EF Core logging
builder.Logging.AddFilter(DbLoggerCategory.Database.Command.Name, LogLevel.Information); // Set the log level for database commands


// Register ILogger service
builder.Services.AddLogging(loggingBuilder =>
{
    loggingBuilder.AddSeq(builder.Configuration.GetSection("SeqConfig"));
});


// API Versioning
builder.Services
    .AddApiVersioning()
    .AddVersionedApiExplorer(options =>
    {
        options.GroupNameFormat = "'v'VVV";
        options.SubstituteApiVersionInUrl = true;
        options.DefaultApiVersion = new ApiVersion(1, 0);
    });


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Swagger Settings
builder.Services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
builder.Services.AddSwaggerGen(options =>
{
    options.OperationFilter<SwaggerDefaultValues>();
});

builder.Host.ConfigureLogging(logging =>
{
    logging.ClearProviders(); // Clear default logging providers
    logging.AddConsole(); // Add Console logging provider
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
