using AutoMapper;
using TimeTracker.API.Entities;
using TimeTracker.API.Models;

namespace TimeTracker.API.Profiles
{
    public class TeamProfile : Profile
    {
        public TeamProfile()
        {
            // From database to DTO
            CreateMap<Entities.Team, Models.TeamWithoutUsersDto>();
            CreateMap<Entities.Team, Models.TeamWithUsersDto>();
            CreateMap<Entities.Team, Models.TeamWithProjectsDto>();
            CreateMap<Entities.Team, Models.TeamWithUsersAndProjectsDto>();

            // From DTO to database
            CreateMap<Models.TeamForCreationDto, Entities.Team>()
                .ForMember(dest => dest.Users, opt => opt.Ignore())
                .ForMember(dest => dest.Projects, opt => opt.Ignore())
                .ForMember(dest => dest.Id, opt => opt.Ignore());
        }
    }
}
