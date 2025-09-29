using AutoMapper;

namespace TimeTracker.API.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            // From database to DTO
            CreateMap<Entities.User, Models.UserDto>();
        }


    }
}
