using AutoMapper;
using WorkWithUserPostsData.Application.Dtos.UserPost;
using WorkWithUserPostsData.Domain.Models.Posts;
using WorkWithUserPostsData.Domain.Models.Users;

namespace WorkWithUserPostsData.Application.MapperProfile;

public class UserPostMappingProfile : Profile
{
	public UserPostMappingProfile()
	{
		CreateMap<(User User, List<Post> Posts), UserPostsDto>()
			.ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.Name))
			.ForMember(dest => dest.CountOfPosts, opt => opt.MapFrom(src => src.Posts.Count()))
			.ForMember(dest => dest.City, opt => opt.MapFrom(src => src.User.Address.City));
	}
}
