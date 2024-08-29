using MqttApi.Models;

namespace MqttApi.Interfaces
{
    public interface ITopicRepository
    {
        ICollection<Topic> GetTopics();
        Client GetTopic(long id);
        Client GetTopicByName(string name);
    }
}