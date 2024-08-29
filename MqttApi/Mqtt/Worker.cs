using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MqttApi.Data;
namespace MqttApi.Mqtt
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IMqttService _mqttService;
        private readonly IServiceProvider _serviceProvider;

        public Worker(ILogger<Worker> logger, IMqttService mqttService,IServiceProvider serviceProvider)
        {

            _serviceProvider = serviceProvider;
            _logger = logger;
            _mqttService = mqttService;
        }

        public void onMessageReceived(string topic,string message)
        {
            var scope = _serviceProvider.CreateScope();
            var dataContext = scope.ServiceProvider.GetRequiredService<DataContext>();




            Console.WriteLine($"[Dynamic] Message received on topic {topic}: {message}");
        }

        protected async Task StartAsync()
        {
            // connect
        }



        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            // Subscribe to a topic, providing the message handler callback
            _mqttService.Subscribe("my/topic");

            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);

                // Publish a message to the topic every second
                _mqttService.Publish("my/topic", "Hello, MQTT!");

                _mqttService.Unsubscribe("my/topic");

                _logger.LogInformation("Finished subscribing, publishing, and unsubscribing.");

                await Task.Delay(1000, stoppingToken);
            }
        }

        protected async Task StopAsync()
        {
            // undubscribe & disconnect
        }
    }
}
