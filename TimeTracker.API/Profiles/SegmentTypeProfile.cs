using AutoMapper;

namespace TimeTracker.API.Profiles
{
    public class SegmentTypeProfile : Profile
    {

        public SegmentTypeProfile()
        {
            // From database to DTO
            CreateMap<Entities.SegmentType, Models.SegmentTypeDto>();
        }
    }
}
