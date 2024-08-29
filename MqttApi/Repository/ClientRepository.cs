using MqttApi.Data;
using MqttApi.Interfaces;
using MqttApi.Models;

namespace MqttApi.Repository
{
    public class ClientRepository : IClientRepository
    {
        private readonly DataContext _context;
        public Client GetClient(long id)
        {
            throw new NotImplementedException();
        }

        public Client GetClientByEmail(string email)
        {
            throw new NotImplementedException();
        }

        public Client getClientByNumber(string number)
        {
            throw new NotImplementedException();
        }

        public ICollection<Client> GetClinets()
        {
            return _context.Clients.ToList();
        }
    }
}