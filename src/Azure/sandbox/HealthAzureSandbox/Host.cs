// <copyright file="Host.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using App.Metrics.Health;
using Microsoft.Azure.Documents.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage;
using Serilog;

namespace HealthAzureSandbox
{
    public static class Host
    {
        public static IConfigurationRoot Configuration { get; set; }

        public static IHealthRoot Health { get; set; }

        public static async Task Main()
        {
            Init();

            var cancellationTokenSource = new CancellationTokenSource();

            await RunUntilEscAsync(
                TimeSpan.FromSeconds(20),
                cancellationTokenSource,
                async () =>
                {
                    Console.Clear();

                    var healthStatus = await Health.HealthCheckRunner.ReadAsync(cancellationTokenSource.Token);

                    foreach (var formatter in Health.OutputHealthFormatters)
                    {
                        Console.WriteLine($"Formatter: {formatter.GetType().FullName}");
                        Console.WriteLine("-------------------------------------------");

                        using (var stream = new MemoryStream())
                        {
                            await formatter.WriteAsync(stream, healthStatus, cancellationTokenSource.Token);

                            var result = Encoding.UTF8.GetString(stream.ToArray());

                            Console.WriteLine(result);
                        }
                    }
                });
        }

        private static void Init()
        {
            var builder = new ConfigurationBuilder()
                          .SetBasePath(Directory.GetCurrentDirectory())
                          .AddJsonFile("appsettings.json");

            Configuration = builder.Build();

            Log.Logger = new LoggerConfiguration()
                         .MinimumLevel.Verbose()
                         .WriteTo.LiterateConsole()
                         .WriteTo.Seq("http://localhost:5341")
                         .CreateLogger();

            var containerName = "test";
            var documentDbDatabaseUri = UriFactory.CreateDatabaseUri("test");
            var collectionUri = UriFactory.CreateDocumentCollectionUri("test", "testcollection");
            var doucmentDbKey = "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==";
            var documentDbUri = "https://localhost:8081";
            var storageAccount = CloudStorageAccount.Parse(@"UseDevelopmentStorage=true");

            var eventHubConnectionString = "todo event hub connection string";
            var eventHubName = "todo event hub name";
            var serviceBusConnectionString = "todo sb connection string";
            var queueName = "todo queue name";
            var topicName = "todo topic name";

            Health = AppMetricsHealth.CreateDefaultBuilder()
                                     .HealthChecks.AddAzureBlobStorageConnectivityCheck("Blob Storage Connectivity Check", storageAccount)
                                     .HealthChecks.AddAzureBlobStorageContainerCheck("Blob Storage Container Check", storageAccount, containerName)
                                     .HealthChecks.AddAzureQueueStorageConnectivityCheck("Queue Storage Connectivity Check", storageAccount)
                                     .HealthChecks.AddAzureQueueStorageCheck("Queue Storage Check", storageAccount, "test")
                                     .HealthChecks.AddAzureDocumentDBDatabaseCheck("DocumentDB Database Check", documentDbDatabaseUri, documentDbUri, doucmentDbKey)
                                     .HealthChecks.AddAzureDocumentDBCollectionCheck("DocumentDB Collection Check", collectionUri, documentDbUri, doucmentDbKey)
                                     .HealthChecks.AddAzureTableStorageConnectivityCheck("Table Storage Connectivity Check", storageAccount)
                                     .HealthChecks.AddAzureTableStorageTableCheck("Table Storage Table Exists Check", storageAccount, "test")
                                     .HealthChecks.AddAzureEventHubConnectivityCheck("Service EventHub Connectivity Check", eventHubConnectionString, eventHubName)
                                     .HealthChecks.AddAzureServiceBusQueueConnectivityCheck("Service Bus Queue Connectivity Check", serviceBusConnectionString, queueName)
                                     .HealthChecks.AddAzureServiceBusTopicConnectivityCheck("Service Bus Topic Connectivity Check", serviceBusConnectionString, topicName)
                                     .Build();
        }

        private static async Task RunUntilEscAsync(TimeSpan delayBetweenRun, CancellationTokenSource cancellationTokenSource, Func<Task> action)
        {
            Console.WriteLine("Press ESC to stop");

            while (true)
            {
                while (!Console.KeyAvailable)
                {
                    await action();
                    Thread.Sleep(delayBetweenRun);
                }

                while (Console.KeyAvailable)
                {
                    var key = Console.ReadKey(false).Key;

                    if (key == ConsoleKey.Escape)
                    {
                        cancellationTokenSource.Cancel();
                        return;
                    }
                }
            }
        }
    }
}