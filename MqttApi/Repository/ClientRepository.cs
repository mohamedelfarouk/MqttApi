using MqttApi.Data;
using MqttApi.Interfaces;
using MqttApi.Models;

namespace MqttApi.Repository
{
    public class ClientRepository : IClientRepository
    {
        private readonly DataContext _context;

        public ClientRepository(DataContext context)
        {
            _context = context;
        }

        public void AddClient(Client client)
        {
            _context.Clients.Add(client);
        }

        public Client GetClient(long id)
        {
            return _context.Clients.FirstOrDefault(c => c.Id == id);
        }

        public Client GetClientByEmail(string email)
        {
            return _context.Clients.FirstOrDefault(c => c.email == email);
        }

        public Client GetClientByName(string name)
        {
            return _context.Clients.FirstOrDefault(c => c.Name == name);
        }

        public Client getClientByNumber(string number)
        {
            return _context.Clients.FirstOrDefault(c => c.number == number);
        }

    public ICollection<Client> GetClinets()
        {
            return _context.Clients.ToList();
        }

        public bool Save()
        {
            return _context.SaveChanges() > 0;
        }
    }
}