using ERDM.Credit.Application.Mappings;
using ERDM.Credit.Application.Services;
using ERDM.Credit.Domain.Interfaces;
using ERDM.Credit.Infrastructure.Repositories;
using ERDMCore.Infrastructure.MongoDB.Settings;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using Serilog;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .MinimumLevel.Override("Microsoft", Serilog.Events.LogEventLevel.Warning)
    .MinimumLevel.Override("Microsoft.AspNetCore", Serilog.Events.LogEventLevel.Warning)
    .MinimumLevel.Override("MongoDB.Driver", Serilog.Events.LogEventLevel.Warning)
    .Enrich.WithProperty("Application", "ERDM.Credit.API")
    .Enrich.WithProperty("Environment", builder.Environment.EnvironmentName)
    .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}")
    .WriteTo.File($"logs/erdm-credit-{DateTime.Now:yyyy-MM-dd}.log",
        rollingInterval: RollingInterval.Day,
        retainedFileCountLimit: 30)
    .CreateLogger();

builder.Host.UseSerilog();

var mongoDbSettings = builder.Configuration.GetSection("MongoDbSettings").Get<MongoDbSettings>();
if (mongoDbSettings == null)
{
    throw new InvalidOperationException("MongoDB settings are not configured in appsettings.json");
}

builder.Services.Configure<MongoDbSettings>(builder.Configuration.GetSection("MongoDbSettings"));
builder.Services.AddSingleton(sp => sp.GetRequiredService<IOptions<MongoDbSettings>>().Value);

builder.Services.AddSingleton<IMongoClient>(sp =>
{
    var settings = sp.GetRequiredService<MongoDbSettings>();
    var clientSettings = MongoClientSettings.FromConnectionString(settings.ConnectionString);
    clientSettings.MinConnectionPoolSize = settings.MinPoolSize;
    clientSettings.MaxConnectionPoolSize = settings.MaxPoolSize;
    clientSettings.ConnectTimeout = TimeSpan.FromSeconds(settings.ConnectionTimeoutSeconds);
    clientSettings.SocketTimeout = TimeSpan.FromSeconds(settings.SocketTimeoutSeconds);
    clientSettings.ServerSelectionTimeout = TimeSpan.FromSeconds(settings.ConnectionTimeoutSeconds);
    clientSettings.RetryWrites = settings.RetryWrites;
    clientSettings.RetryReads = settings.RetryReads;

    if (settings.UseSsl)
    {
        clientSettings.SslSettings = new SslSettings
        {
            EnabledSslProtocols = System.Security.Authentication.SslProtocols.Tls12
        };
    }

    return new MongoClient(clientSettings);
});

builder.Services.AddScoped<IMongoDatabase>(sp =>
{
    var client = sp.GetRequiredService<IMongoClient>();
    var settings = sp.GetRequiredService<MongoDbSettings>();
    return client.GetDatabase(settings.DatabaseName);
});

builder.Services.AddScoped<ICreditApplicationRepository, CreditApplicationRepository>();
builder.Services.AddScoped<ICreditApplicationService, CreditApplicationService>();

builder.Services.AddAutoMapper(cfg =>
{
    cfg.AddProfile<CreditApplicationMappingProfile>();
    cfg.AddProfile<AddressMappingProfile>();
    cfg.AddProfile<ApplicationDataMappingProfile>();
    cfg.AddProfile<ApplicationMetadataMappingProfile>();
    cfg.AddProfile<CreditApplicationProfile>();
    cfg.AddProfile<CreditCardMappingProfile>();
    cfg.AddProfile<CustomerProfileMappingProfile>();
    cfg.AddProfile<DecisionMappingProfile>();
    cfg.AddProfile<EmploymentMappingProfile>();
    cfg.AddProfile<FinancialProfileMappingProfile>();
    cfg.AddProfile<LoanMappingProfile>();
    cfg.AddProfile<PaginationMappingProfile>();
});

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = null;
        options.JsonSerializerOptions.WriteIndented = true;
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    });

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "ERDM Credit Management API",
        Description = "API for managing credit applications and underwriting",
        Contact = new OpenApiContact
        {
            Name = "ERDM Support",
            Email = "support@erdm.com"
        },
        License = new OpenApiLicense
        {
            Name = "Use under LICX",
            Url = new Uri("https://example.com/license")
        }
    });

    c.CustomSchemaIds(type => type.FullName);
    c.UseAllOfForInheritance();
    c.UseOneOfForPolymorphism();
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

builder.Services.AddHealthChecks();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "ERDM Credit API V1");
        c.RoutePrefix = "swagger";
    });
}

app.UseCors("AllowAll");
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.MapHealthChecks("/health");

using (var scope = app.Services.CreateScope())
{
    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
    var settings = scope.ServiceProvider.GetRequiredService<MongoDbSettings>();
    var database = scope.ServiceProvider.GetRequiredService<IMongoDatabase>();

    try
    {
        await database.RunCommandAsync<BsonDocument>(new BsonDocument("ping", 1));
        logger.LogInformation("Successfully connected to MongoDB Atlas");
        logger.LogInformation("Database: {DatabaseName}", settings.DatabaseName);

        var collections = await (await database.ListCollectionNamesAsync()).ToListAsync();
        logger.LogInformation("Existing collections: {Count}", collections.Count);
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "Failed to connect to MongoDB");
        if (app.Environment.IsDevelopment())
        {
            throw;
        }
    }
}

app.Run();