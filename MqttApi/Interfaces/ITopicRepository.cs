using MqttApi.Models;

namespace MqttApi.Interfaces
{
    public interface ITopicRepository
    {
        ICollection<Topic> GetTopics();
        Topic GetTopic(long id);
        Topic GetTopicByName(string name);
        void AddTopic(Topic topic);
        bool Save();
    }
}