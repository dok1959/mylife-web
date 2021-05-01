using AutoMapper;

namespace MyLife.Profiles
{
    public class UsersProfile : Profile
    {
        public UsersProfile()
        {
            CreateMap<Models.User, ViewModels.ProfileViewModel>();
            CreateMap<Models.User, ViewModels.MyProfileViewModel>();
        }
    }
}
