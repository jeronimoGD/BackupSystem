using BackUpAgent;
using BackUpAgent.Common.Interfaces;
using BackUpAgent.Common.Interfaces.ApiRequests;
using BackUpAgent.Common.Interfaces.BackUpManaging;
using BackUpAgent.Common.Interfaces.DbServices;
using BackUpAgent.Common.Interfaces.Repository;
using BackUpAgent.Common.Interfaces.ScheduledTasks;
using BackUpAgent.Common.Interfaces.SignalR;
using BackUpAgent.Common.Interfaces.Utils;
using BackUpAgent.Common.Mappings;
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
using Microsoft.Extensions.Logging;

internal class Program
{
    private static async Task Main(string[] args)
    {
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
                        services.AddSingleton<IBackUpScheduler, BackUpScheduler>();

                        // Add Http services
                        services.AddHttpClient<IBackUpSystemApiRequestService, BackUpSystemApiRequestService>();
                        services.AddSingleton<IBackUpSystemApiRequestService, BackUpSystemApiRequestService>();

                        // SignalRServce
                        services.AddTransient<ISignalRService, SignalRService>();

                        //BackUpManager service
                        services.AddTransient<IBackUpManager, BackUpManager>();

                        // Inject Unit Of Work
                        services.AddScoped<IUnitOfWork, UnitOfWork>();
                        services.AddTransient<IBackUpConfigurationService, BackUpConfigurationService>();
                        services.AddTransient<IBackUpHistoryService, BackUpHistoryService>();

                        // Utils
                        services.AddTransient<IUtils, Utils>();

                        // Auto Mapper
                        services.AddAutoMapper(typeof(MappingProfile));
                    })
                    .ConfigureAppConfiguration((hostingContext, config) =>
                    {
                        var configPath = args.FirstOrDefault(arg => arg.StartsWith("--configFilePath="));
                        string test = hostingContext.Configuration["configFilePath"];
                        
                        if (!string.IsNullOrEmpty(configPath))
                        {
                            var path = configPath.Substring("--configFilePath=".Length);
                            config.AddJsonFile(path, optional: false, reloadOnChange: true);
                            config.AddEnvironmentVariables();
                        }
                        else
                        {
                            config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
                            config.AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}. json", optional: true);
                            config.AddEnvironmentVariables();
                        }
                    })
                    .ConfigureLogging((hostingContext, logging) =>
                    {
                        logging.AddConsole().
                                AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
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
}