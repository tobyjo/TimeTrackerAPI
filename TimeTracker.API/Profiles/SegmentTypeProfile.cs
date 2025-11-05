using AutoMapper;

namespace TimeTracker.API.Profiles
{
    public class SegmentTypeProfile : Profile
    {

        public SegmentTypeProfile()
        {
            // From database to DTO
            CreateMap<Entities.SegmentType, Models.SegmentTypeDto>();

            // From DTO to database
            CreateMap<Models.SegmentTypeForCreationDto, Entities.SegmentType>()
                  .ForMember(dest => dest.Team, opt => opt.Ignore())
                .ForMember(dest => dest.TimeEntries, opt => opt.Ignore())
                .ForMember(dest => dest.Id, opt => opt.Ignore());

            CreateMap<Models.SegmentTypeForUpdateDto, Entities.SegmentType>()
                .ForMember(dest => dest.Team, opt => opt.Ignore())
                .ForMember(dest => dest.TimeEntries, opt => opt.Ignore())
                .ForMember(dest => dest.Id, opt => opt.Ignore());
        }
    }
}
