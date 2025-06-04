using AutoMapper;
using WorkWithUserPostsData.Application.Dtos.User;
using WorkWithUserPostsData.Domain.Models.Users;

namespace WorkWithUserPostsData.Application.MapperProfile;

public class UserMapperProfile : Profile
{
	public UserMapperProfile()
	{
		CreateMap<User, UserDto>()
			.ForMember(dto => dto.City, e => e.MapFrom(e => e.Address.City));
	}
}
