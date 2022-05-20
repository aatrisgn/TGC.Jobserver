using Hangfire;
using Hangfire.SqlServer;
using Hangfire.Storage;
using Serilog;
using Serilog.Events;
using TGC.JobServer.Abstractions.Infrastructure;
using TGC.JobServer.Abstractions.Jobs;
using TGC.JobServer.Abstractions.Services;
using TGC.JobServer.Infrastructure;
using TGC.JobServer.Jobs;
using TGC.JobServer.Jobs.JobExecutionStrategies;
using TGC.JobServer.Jobs.JobTypes;
using TGC.JobServer.Services;

namespace TGC.JobServer.WebAPI
{
    public class CustomApplicationBuilder
    {
        public static WebApplicationBuilder Build(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            var hangfireConnectionstring = builder.Configuration.GetConnectionString("HangfireDB");

            builder.Services.AddHealthChecks();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddHangfire(x => {

                x.SetDataCompatibilityLevel(CompatibilityLevel.Version_170);
                x.UseColouredConsoleLogProvider();
                x.UseSimpleAssemblyNameTypeSerializer();
                x.UseRecommendedSerializerSettings();
                x.UseSqlServerStorage(hangfireConnectionstring, new SqlServerStorageOptions
                {
                    CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
                    SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
                    QueuePollInterval = TimeSpan.Zero,
                    UseRecommendedIsolationLevel = true
                });
            });

            builder.Services.AddHangfireServer();

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var logger = new LoggerConfiguration()
            #if DEBUG
                .MinimumLevel.Override("Microsoft", LogEventLevel.Debug)
            #else
                .MinimumLevel.Override("Microsoft", LogEventLevel.Informatin)
            #endif
                .Enrich.FromLogContext()
                            .WriteTo.Console()
                            .CreateLogger();

            builder.Services.AddSingleton<Serilog.ILogger>(logger);
            builder.Services.AddSingleton<ICustomMonitoringApi, MonitoringApi>();

            builder.Services.AddScoped<IJobService, JobService>();
            builder.Services.AddScoped<IJobTypeResolver, JobTypeResolver>();
            builder.Services.AddScoped<IJobExecutionTypeResolver, JobExecutionTypeResolver>();
            builder.Services.AddTransient<IStandardHttpClient, StandardHttpClient>();
            builder.Services.AddScoped<IJobInitializeService, JobInitializeService>();

            AddExecutionStrategies(builder.Services);
            AddJobTypes(builder.Services);

            return builder;
        }

        public static void AddExecutionStrategies(IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<IExecutionService, BatchExecutionService>();
            serviceCollection.AddScoped<IExecutionService, DelayedExecutionService>();
            serviceCollection.AddScoped<IExecutionService, FireAndForgetExecutionService>();
            serviceCollection.AddScoped<IExecutionService, RecurringExecutionService>();
        }

        public static void AddJobTypes(IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<IInvokeableJob, HttpJob>();
            serviceCollection.AddScoped<IInvokeableJob, IsAliveJob>();
            serviceCollection.AddScoped<IInvokeableJob, OrderBioBagsJob>();
        }
    }
}
