using AutoMapper;

namespace TimeTracker.API.Profiles
{
    public class TeamProfile : Profile
    {
        public TeamProfile()
        {
            // From database to DTO
            CreateMap<Entities.Team, Models.TeamWithoutUsersDto>();
            CreateMap<Entities.Team, Models.TeamWithUsersDto>();

            // From DTO to database
            CreateMap<Models.TeamForCreationDto, Entities.Team>()
                .ForMember(dest => dest.Users, opt => opt.Ignore())
                .ForMember(dest => dest.Id, opt => opt.Ignore());
        }
    }
}
