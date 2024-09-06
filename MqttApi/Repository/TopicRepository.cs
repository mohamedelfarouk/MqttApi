using MqttApi.Data;
using MqttApi.Interfaces;
using MqttApi.Models;

namespace MqttApi.Repository
{
    public class TopicRepository : ITopicRepository
    {
        private readonly DataContext _context;

        public TopicRepository(DataContext context)
        {
            _context = context;
        }

        public void AddTopic(Topic topic)
        {
            _context.Topics.Add(topic);
        }

        public Topic GetTopic(long id)
        {
            return _context.Topics.FirstOrDefault(t => t.Id == id);
        }

        public Topic GetTopicByName(string name)
        {
            return _context.Topics.FirstOrDefault(t => t.Name == name);
        }

        public ICollection<Topic> GetTopics()
        {
            return _context.Topics.ToList();
        }

        public bool Save()
        {
            return _context.SaveChanges() > 0;
        }
    }
}