using MqttApi.Models;

namespace MqttApi.Interfaces
{
    public interface IClientRepository
    {
        ICollection<Client> GetClinets();
        Client GetClient(long id);
        Client GetClientByName(string name);
        Client GetClientByEmail(string email);
        Client getClientByNumber(string number);
        void AddClient(Client client);
        bool Save();
    }
}