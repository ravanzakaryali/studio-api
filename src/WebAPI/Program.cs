using AspNetCoreRateLimit;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Microsoft.AspNetCore.HttpLogging;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.MSSqlServer;
using Space.Application.Helper;
using Space.WebAPI.Filters;
using System.Text.Json.Serialization;


var builder = WebApplication.CreateBuilder(args);

AppOptions options = new()
{
    Credential = GoogleCredential.FromFile("studio-firebase.json"),
};

FirebaseApp.Create(options);

ColumnOptions columnOpts = new ColumnOptions();
columnOpts.Store.Remove(StandardColumn.Properties);
columnOpts.Store.Add(StandardColumn.LogEvent);
columnOpts.PrimaryKey = columnOpts.TimeStamp;
columnOpts.TimeStamp.NonClusteredIndex = true;

Serilog.Core.Logger log = new LoggerConfiguration()
    .WriteTo.Console()
    .AuditTo.MSSqlServer(builder.Configuration.GetConnectionString("SqlServer"), new MSSqlServerSinkOptions()
    {
        TableName = "Logs",
        AutoCreateSqlTable = true,
    }, columnOptions: columnOpts, restrictedToMinimumLevel: LogEventLevel.Error)
    .Enrich.FromLogContext()
    .CreateLogger();

Log.Logger = log;

builder.Host.UseSerilog(log);
builder.Logging.AddSerilog(log);

builder.Services.AddApplicationServices();
builder.Services.AddHostedService<NotificationBackgroundService>();
builder.Services.AddInfrastructureServices(builder.Configuration);




builder.Services.AddHttpLogging(logging =>
{
    logging.LoggingFields = HttpLoggingFields.All;
    logging.RequestHeaders.Add("sec-ch-ua");
    logging.MediaTypeOptions.AddText("application/javascript");
    logging.RequestBodyLogLimit = 4096;
    logging.ResponseBodyLogLimit = 4096;
});


builder.Services.AddControllers(opt =>
{
    opt.Filters.Add<PermissionEndpointFilter>();
}).AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new DateOnlyConverter());
    options.JsonSerializerOptions.Converters.Add(new TimeOnlyConverter());
    options.JsonSerializerOptions.Converters.Add(new TimeOnlyNullableConverter());
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(

                      builder =>
                      {
                          builder.WithOrigins("http://localhost:3000", "http://localhost:3001", "https://localhost:5002", "https://studio.code.az", "https://dev-studio.code.az")
                                                .AllowAnyHeader()
                                                .AllowAnyMethod()
                                                .AllowCredentials();
                      });
});

builder.Services.AddMemoryCache();
builder.Services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
builder.Services.AddInMemoryRateLimiting();

builder.Services.Configure<ClientRateLimitOptions>(options =>
{
    options.EnableEndpointRateLimiting = true;
    options.StackBlockedRequests = false;
    options.HttpStatusCode = 429;
    options.GeneralRules = new List<RateLimitRule>
        {
            new RateLimitRule
            {
                Endpoint = "*",
                Period = "1s",
                Limit = 10
            },
            new RateLimitRule
            {
                Endpoint = "*",
                Period = "1m",
                Limit = 80
            },
             new RateLimitRule
            {
                Endpoint = "*",
                Period = "1h",
                Limit = 600
            },
            new RateLimitRule
            {
                Endpoint = "POST:/api/supports",
                Period= "5s",
                Limit=1,
            },
             new RateLimitRule
            {
                Endpoint = "POST:/api/supports",
                Period= "1m",
                Limit=5,
            },
               new RateLimitRule
            {
                Endpoint = "POST:/api/supports",
                Period= "1d",
                Limit=30,
            },
        };
});

builder.Services.AddSwaggerGen(config =>
{
    config.OperationFilter<AuthenticationRequirementOperationFilter>();
    // config.IncludeXmlComments(Path.ChangeExtension(Assembly.GetExecutingAssembly().Location, ".xml"));
    config.UseInlineDefinitionsForEnums();
});

var app = builder.Build();

app.UseCors();
app.UseTokenAuthetication();

app.UseChangeTokenAuthetication();
app.UseEndpointScanner();
app.UseRateLimit();

app.UseSerilogRequestLogging();

app.UseHttpLogging();

app.UseStaticFiles();
app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();


app.UseExceptionMiddleware();

app.Run();
