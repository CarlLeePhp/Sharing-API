using AutoMapper;
using Sharing_API.DTOs;
using Sharing_API.Models;

namespace Sharing_API.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<AppUser, MemberDto>();
            CreateMap<MemberDto, AppUser>();
            CreateMap<AppUser, MemberUpdateDto>();
            CreateMap<Sharing, SharingDto>();
            CreateMap<SharingDto, Sharing>();
            CreateMap<Interest, InterestDto>();
            CreateMap<Category, CategoryDto>();
        }
    }
}
