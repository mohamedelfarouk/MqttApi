using MqttApi.Mqtt;

namespace MqttApi.Interfaces
{
    public interface IMessageRepository
    {
        void AddMessage(Message message);
        Boolean Save();
    }
}
