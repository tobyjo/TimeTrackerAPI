using AutoMapper;

namespace TimeTracker.API.Profiles
{
    public class TimeEntryProfile : Profile
    {

        public TimeEntryProfile()
        {
            // From database to DTO
            CreateMap<Entities.TimeEntry, Models.TimeEntryWithDetailsDto>();
            CreateMap<Entities.TimeEntry, Models.TimeEntryDto>();




            // CreateMap<Models.TimeEntryDto, Entities.TimeEntry>();
            // CreateMap<Models.ProjectDto, Entities.Project>();

            /*
            CreateMap<Entities.Project, Models.ProjectDto>()
                .ForMember(dest => dest.ID, opt => opt.MapFrom(src => src.Id));
            CreateMap<Models.ProjectDto, Entities.Project>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.ID));
            CreateMap<Entities.TimeEntry, Models.TimeEntryDto>()
                .ForMember(dest => dest.UserID, opt => opt.MapFrom(src => src.User.Id))
                .ForMember(dest => dest.ProjectID, opt => opt.MapFrom(src => src.Project.Id))
                .ForMember(dest => dest.SegmentTypeID, opt => opt.MapFrom(src => src.SegmentType.Id));
            CreateMap<Models.TimeEntryDto, Entities.TimeEntry>()
                .ForMember(dest => dest.User, opt => opt.Ignore())
                .ForMember(dest => dest.Project, opt => opt.Ignore())
                .ForMember(dest => dest.SegmentType, opt => opt.Ignore());
            */
        }
    }
}
