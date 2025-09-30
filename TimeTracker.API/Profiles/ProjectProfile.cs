using AutoMapper;

namespace TimeTracker.API.Profiles
{
    public class ProjectProfile : Profile
    {
        public ProjectProfile()
        {
            // From database to DTO
            CreateMap<Entities.Project, Models.ProjectDto>();

            // From DTO to database
            CreateMap<Models.ProjectForCreationDto, Entities.Project>()
                  .ForMember(dest => dest.Team, opt => opt.Ignore())
                .ForMember(dest => dest.TimeEntries, opt => opt.Ignore())
                .ForMember(dest => dest.Id, opt => opt.Ignore());
        }


    }
}