using MqttApi.Data;
using MqttApi.Interfaces;
using MqttApi.Models;

namespace MqttApi.Repository
{
    public class TopicRepository : ITopicRepository
    {
        private readonly DataContext _context;
        public Client GetTopic(long id)
        {
            throw new NotImplementedException();
        }

        public Client GetTopicByName(string name)
        {
            throw new NotImplementedException();
        }

        public ICollection<Topic> GetTopics()
        {
            return _context.Topics.ToList();
        }
    }
}