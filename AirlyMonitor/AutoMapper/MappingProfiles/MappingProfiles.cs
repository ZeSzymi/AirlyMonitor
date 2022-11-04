using AirlyInfrastructure.Database;
using AirlyMonitor.Models.Dtos;
using AutoMapper;

namespace AirlyMonitor.AutoMapper.Profiles
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<AlertDefinitionDto, AlertDefinition>();
            CreateMap<AlertDefinition, AlertDefinitionDto>();
        }
    }
}
