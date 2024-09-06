using MqttApi.Interfaces;
using MqttApi.Models;
using MqttApi.Repository;
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
        private readonly IServiceProvider _serviceProvider;

        public MqttService(
        ILogger<MqttService> logger,
        IConfiguration configuration,
        IServiceProvider serviceProvider)
        {
            _logger = logger;
            _configuration = configuration;
            _serviceProvider = serviceProvider;

            var mqttSettings = _configuration.GetSection("MQTT").Get<MqttSettings>();

            var factory = new MqttFactory();
            _mqttClient = factory.CreateMqttClient();

            var options = new MqttClientOptionsBuilder()
                .WithClientId(mqttSettings?.ClientId)
                .WithTcpServer(mqttSettings?.Host, mqttSettings?.Port)
                .WithCleanSession()
                .Build();

            _mqttClient.ConnectAsync(options).Wait();

            // Message received handler
            _mqttClient.ApplicationMessageReceivedAsync += async e =>
            {
                var scope = _serviceProvider.CreateScope();
                var _topicRepository = scope.ServiceProvider.GetRequiredService<ITopicRepository>();
                var _clientRepository = scope.ServiceProvider.GetRequiredService<IClientRepository>();
                var _messageRepository = scope.ServiceProvider.GetRequiredService<IMessageRepository>();


                var payload = Encoding.UTF8.GetString(e.ApplicationMessage.PayloadSegment);
                _logger.LogInformation($"Received message: {payload} from topic: {e.ApplicationMessage.Topic}");

                // Deserialize the message using the existing method
                var messageData = Message.FromJson(payload);

                // Handle the topic
                var topic = _topicRepository.GetTopicByName(messageData.Topic);
                if (topic == null)
                {
                    topic = new Topic { Name = messageData.Topic };
                    _topicRepository.AddTopic(topic);
                    _topicRepository.Save();
                }

                // Handle the client (message sender)
                var client = _clientRepository.GetClientByName(messageData.From);
                if (client == null)
                {
                    client = new Client { Name = messageData.From };
                    _clientRepository.AddClient(client);
                    _clientRepository.Save();
                }

                // Save the message
                var mqttMessage = new Message
                {
                    MessageBody = messageData.MessageBody,
                    From = messageData.From,
                    Topic = messageData.Topic,
                };
                _messageRepository.AddMessage(mqttMessage);
                _messageRepository.Save();

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
