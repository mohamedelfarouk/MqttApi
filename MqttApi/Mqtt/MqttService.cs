using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace MqttApi.Mqtt
{
    public class MqttService : IMqttService
    {
        private readonly IMqttClient _mqttClient;
        private readonly ILogger<MqttService> _logger;
        private readonly IConfiguration _configuration;
        private readonly Action<string, string> _onMessageReceived;
        
        public MqttService(ILogger<MqttService> logger,IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;

            var mqttSettings = _configuration.GetSection("MQTT").Get<MqttSettings>();

            var factory = new MqttFactory();
            _mqttClient = factory.CreateMqttClient();

            // Set up the connection options
            var options = new MqttClientOptionsBuilder()
                .WithClientId(mqttSettings?.ClientId) // Replace with you ID
                .WithTcpServer(mqttSettings?.Host, mqttSettings?.Port) // Replace with your MQTT broker address and port
                .WithCleanSession()
                .Build();

            _mqttClient.ConnectAsync(options).Wait();

            _mqttClient.ApplicationMessageReceivedAsync += async e =>
            {
                var payload = Encoding.UTF8.GetString(e.ApplicationMessage.PayloadSegment);
                _logger.LogInformation($"Received message: {payload} from topic: {e.ApplicationMessage.Topic}");
                _onMessageReceived?.Invoke(e.ApplicationMessage.Topic, payload);
                await Task.CompletedTask;
            };
        }

        public async void Publish(string topic, string message)
        {
            var mqttMessage = new MqttApplicationMessageBuilder()
                .WithTopic(topic)
                .WithPayload(message)
                .WithQualityOfServiceLevel(MQTTnet.Protocol.MqttQualityOfServiceLevel.ExactlyOnce)
                .Build();

            if (_mqttClient.IsConnected)
            {
                await _mqttClient.PublishAsync(mqttMessage);
                _logger.LogInformation($"Published message: {message} to topic: {topic}");
            }
            else
            {
                _logger.LogWarning("MQTT client is not connected.");
            }
        }

        public async void Subscribe(string topic)
        {
            await _mqttClient.SubscribeAsync(new MqttTopicFilterBuilder().WithTopic(topic).Build());
            _logger.LogInformation($"Subscribed to topic: {topic}");
        }

        public void Unsubscribe(string topic)
        {
            _mqttClient.UnsubscribeAsync(topic);
            _logger.LogInformation($"Unsubscribed from topic: {topic}");
        }


    }
}
