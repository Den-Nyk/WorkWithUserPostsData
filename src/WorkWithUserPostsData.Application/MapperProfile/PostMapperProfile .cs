using AutoMapper;
using WorkWithUserPostsData.Application.Dtos.Post;
using WorkWithUserPostsData.Domain.Models.Posts;

namespace WorkWithUserPostsData.Application.MapperProfile;

public class PostMapperProfile : Profile
{
	public PostMapperProfile()
	{
		CreateMap<Post, PostDto>();
	}
}