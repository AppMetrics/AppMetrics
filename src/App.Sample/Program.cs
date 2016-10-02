using System;
using App.Metrics;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.PlatformAbstractions;

namespace App.Sample
{
    public class Host
    {
        public static void Main()
        {
            IServiceCollection serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);

            var application = new Application(serviceCollection);


            // Run
            // ...

            Console.ReadKey();
        }

        private static void ConfigureServices(IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<ILoggerFactory>(provider =>
            {
                var logFactory = new LoggerFactory();
                logFactory.AddConsole();
                return logFactory;
                ;
            });
        }
    }

    public class Application
    {
        public Application(IServiceCollection serviceCollection)
        {
            ConfigureServices(serviceCollection);
            Services = serviceCollection.BuildServiceProvider();
            Logger = Services.GetRequiredService<ILoggerFactory>().CreateLogger<Application>();
            Logger.LogInformation("Application created successfully.");

            var app = Services.GetRequiredService<ApplicationEnvironment>();

        }

        public ILogger Logger { get; set; }

        public IServiceProvider Services { get; set; }        


        private void ConfigureServices(IServiceCollection serviceCollection)
        {
        }
    }
}