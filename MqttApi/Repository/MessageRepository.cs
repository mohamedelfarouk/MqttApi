using MqttApi.Data;
using MqttApi.Interfaces;
using MqttApi.Mqtt;

namespace MqttApi.Repository
{
    public class MessageRepository : IMessageRepository
    {
        private readonly DataContext _context;

        public MessageRepository(DataContext context)
        {
            _context = context;
        }

        public void AddMessage(Message message)
        {
            _context.Add(message);
        }

        public bool Save()
        {
            return _context.SaveChanges() > 0;
        }
    }
}
