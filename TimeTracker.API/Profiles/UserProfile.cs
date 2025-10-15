using AutoMapper;
using System.Collections.Generic;
using TimeTracker.API.Entities;

namespace TimeTracker.API.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            // From database to DTO
            CreateMap<Entities.User, Models.UserDto>();
            CreateMap<Entities.User, Models.UserWithTimeEntriesDto>();

            CreateMap<Entities.User, Models.UserWithSegmentTypesDto>()
                .ForMember(
                dest => dest.SegmentTypes,
                opt => opt.MapFrom(src => src.Team != null ? src.Team.SegmentTypes : new List<SegmentType>())
            );

            /*	•	The ForMember configuration tells AutoMapper to map the Projects property in the DTO from the user's team's projects.
                •	If Team is null, it maps an empty list.
                •	Ensure that you have a mapping from Project to ProjectDto(usually in ProjectProfile).
            Usage: When you fetch a user with their team and projects included(as shown in your repository method), AutoMapper will correctly map the projects to the DTO.

            */
            CreateMap<Entities.User, Models.UserWithProjectsDto>()
                .ForMember(
                dest => dest.Projects,
                opt => opt.MapFrom(src => src.Team != null ? src.Team.Projects : new List<Project>())
            );
        }
    }
}
