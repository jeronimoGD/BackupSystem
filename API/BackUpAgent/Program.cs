using BackUpAgent;
using BackUpAgent.Common.Interfaces;
using BackUpAgent.Common.Interfaces.ApiRequests;
using BackUpAgent.Common.Interfaces.BackUpManaging;
using BackUpAgent.Common.Interfaces.DbServices;
using BackUpAgent.Common.Interfaces.Repository;
using BackUpAgent.Common.Interfaces.ScheduledTasks;
using BackUpAgent.Common.Interfaces.SignalR;
using BackUpAgent.Common.Interfaces.Utils;
using BackUpAgent.Common.Services;
using BackUpAgent.Common.Services.BuckUpManaging;
using BackUpAgent.Common.Services.DbServices;
using BackUpAgent.Common.Services.ScheduledTasks;
using BackUpAgent.Common.Services.Utils;
using BackUpAgent.Data;
using BackUpAgent.Models.ApplicationSettings;
using BackupSystem.Common.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

internal class Program
{
    private static async Task Main(string[] args)
    {

        var configurationBuilder = new ConfigurationBuilder();
        BuildConfig(configurationBuilder);

        var host = Host.CreateDefaultBuilder()
                    .ConfigureServices((context, services)=>
                    {
                        // Add data base conexion string for migrations
                        services.AddDbContext<BackUpSystemDbContext>(option =>
                        {
                            option.UseSqlServer(context.Configuration.GetConnectionString("DbConexion"));
                        });

                        services.Configure<AppSettings>(context.Configuration.GetSection("AppSettings"));

                        // StartUp POint
                        services.AddTransient<IStartUp, StartUp>();

                        // Add Hosted Service
                        services.AddHostedService<BackUpScheduler>();

                        // Inject hosted service
                        services.AddTransient<IBackUpScheduler, BackUpScheduler>();

                        // Add Http services
                        services.AddHttpClient<IBackUpSystemApiRequestService, BackUpSystemApiRequestService>();
                        services.AddTransient<IBackUpSystemApiRequestService, BackUpSystemApiRequestService>();

                        // SignalRServce
                        services.AddTransient<ISignalRService, SignalRService>();

                        //BackUpManager service
                        services.AddTransient<IBackUpManager, BackUpManager>();

                        // Inject Unit Of Work
                        services.AddScoped<IUnitOfWork, UnitOfWork>();
                        services.AddTransient<IBackUpConfigurationService, BackUpConfigurationService>();

                        // Utils
                        services.AddTransient<IUtils, Utils>();

                    })
                    .Build();

        using (var scope = host.Services.CreateScope())
        {
            var dataContext = host.Services.GetRequiredService<BackUpSystemDbContext>();
            try
            {
                dataContext.Database.Migrate();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        var service = ActivatorUtilities.CreateInstance<StartUp>(host.Services);
        host.RunAsync();
        await service.StartAgentAsync();
    }
    static void BuildConfig(IConfigurationBuilder configBuilder)
    {
        configBuilder.SetBasePath(Directory.GetCurrentDirectory())
               .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
               .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}. json", optional: true)
               .AddEnvironmentVariables();
    }
}