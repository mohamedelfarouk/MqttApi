using Microsoft.EntityFrameworkCore;
using MqttApi.Models;
namespace MqttApi.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Topic> Topics { get; set; }
    }
}
