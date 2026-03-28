The error indicates that the connection string is not being parsed correctly. This is likely because the connection string contains special characters (like `@` in the password) or is being modified by your configuration. Here's how to fix it:

## Fix 1: URL Encode Special Characters in Password

Your password `DubaiDutyFree@2026` contains `@` which is a special character in URLs. You need to URL encode it:

Replace `@` with `%40` in the password:

```json
{
  "MongoDbSettings": {
    "ConnectionString": "dummy",
    "DatabaseName": "ERDM_Credit",
    "CollectionPrefix": "credit_",
    "MinPoolSize": 10,
    "MaxPoolSize": 100,
    "ConnectionTimeoutSeconds": 30,
    "SocketTimeoutSeconds": 60,
    "WriteConcern": "majority",
    "JournalEnabled": true,
    "ReadPreferenceMode": "Primary",
    "RetryWrites": true,
    "RetryReads": true,
    "UseSsl": true
  }
}
```

## Fix 2: Use Connection String Builder Programmatically

Update your Program.cs to build the connection string programmatically:

```csharp
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

// Get raw connection string from configuration
var rawConnectionString = builder.Configuration.GetConnectionString("MongoDB") ?? 
                         builder.Configuration["MongoDbSettings:ConnectionString"];

if (string.IsNullOrEmpty(rawConnectionString))
{
    throw new InvalidOperationException("MongoDB connection string is not configured");
}

// Build connection string with proper encoding
var connectionString = rawConnectionString;

// If the connection string contains @ in password, encode it
if (connectionString.Contains(":DubaiDutyFree@2026@"))
{
    connectionString = connectionString.Replace(":DubaiDutyFree@2026@", ":DubaiDutyFree%402026@");
}

var mongoDbSettings = new MongoDbSettings
{
    ConnectionString = connectionString,
    DatabaseName = builder.Configuration["MongoDbSettings:DatabaseName"] ?? "ERDM_Credit",
    CollectionPrefix = builder.Configuration["MongoDbSettings:CollectionPrefix"] ?? "credit_",
    MinPoolSize = int.TryParse(builder.Configuration["MongoDbSettings:MinPoolSize"], out var minPool) ? minPool : 10,
    MaxPoolSize = int.TryParse(builder.Configuration["MongoDbSettings:MaxPoolSize"], out var maxPool) ? maxPool : 100,
    ConnectionTimeoutSeconds = int.TryParse(builder.Configuration["MongoDbSettings:ConnectionTimeoutSeconds"], out var connTimeout) ? connTimeout : 30,
    SocketTimeoutSeconds = int.TryParse(builder.Configuration["MongoDbSettings:SocketTimeoutSeconds"], out var sockTimeout) ? sockTimeout : 60,
    WriteConcern = builder.Configuration["MongoDbSettings:WriteConcern"] ?? "majority",
    JournalEnabled = bool.TryParse(builder.Configuration["MongoDbSettings:JournalEnabled"], out var journal) ? journal : true,
    ReadPreferenceMode = builder.Configuration["MongoDbSettings:ReadPreferenceMode"] ?? "Primary",
    RetryWrites = bool.TryParse(builder.Configuration["MongoDbSettings:RetryWrites"], out var retryWrites) ? retryWrites : true,
    RetryReads = bool.TryParse(builder.Configuration["MongoDbSettings:RetryReads"], out var retryReads) ? retryReads : true,
    UseSsl = bool.TryParse(builder.Configuration["MongoDbSettings:UseSsl"], out var useSsl) ? useSsl : true
};

builder.Services.AddSingleton(mongoDbSettings);

builder.Services.AddSingleton<IMongoClient>(sp =>
{
    var settings = sp.GetRequiredService<MongoDbSettings>();
    
    try
    {
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
    }
    catch (Exception ex)
    {
        var logger = sp.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Failed to create MongoDB client with connection string: {ConnectionString}", 
            MaskConnectionString(settings.ConnectionString));
        throw;
    }
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
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "ERDM Credit Management API",
        Version = "v1",
        Description = "API for managing credit applications and underwriting"
    });
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

static string MaskConnectionString(string connectionString)
{
    if (string.IsNullOrEmpty(connectionString)) return "Not configured";
    
    try
    {
        return System.Text.RegularExpressions.Regex.Replace(
            connectionString,
            ":(.*?)@",
            ":****@");
    }
    catch
    {
        return "[Invalid connection string format]";
    }
}
```

## Fix 3: Use appsettings.json with Correct Format

Update your `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "MongoDB": "dummy"
  },
  "MongoDbSettings": {
    "DatabaseName": "ERDM_Credit",
    "CollectionPrefix": "credit_",
    "MinPoolSize": 10,
    "MaxPoolSize": 100,
    "ConnectionTimeoutSeconds": 30,
    "SocketTimeoutSeconds": 60,
    "WriteConcern": "majority",
    "JournalEnabled": true,
    "ReadPreferenceMode": "Primary",
    "RetryWrites": true,
    "RetryReads": true,
    "UseSsl": true
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning",
      "Microsoft.AspNetCore.HttpLogging.HttpLoggingMiddleware": "Information"
    }
  },
  "AllowedHosts": "*"
}
```

## Key Fixes:

1. **URL Encode the password** - Replace `@` with `%40` in the password
2. **Use ConnectionStrings section** for connection string
3. **Add error handling** to see the actual connection string being used
4. **Mask the connection string in logs** for security

The main issue is the `@` symbol in your password `DubaiDutyFree@2026`. This needs to be URL encoded as `DubaiDutyFree%402026`.
