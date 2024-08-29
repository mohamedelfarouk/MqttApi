using AutoMapper;
using MqttApi.Dto;
using MqttApi.Models;
namespace MqttApi.Helper
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Client, ClientDto>();
            CreateMap<ClientDto, Client>();
        }
    }
}
