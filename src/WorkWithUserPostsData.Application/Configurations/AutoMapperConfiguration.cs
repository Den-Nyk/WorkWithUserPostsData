using AutoMapper;
using WorkWithUserPostsData.Application.MapperProfile;

namespace WorkWithUserPostsData.Application.Configurations;

public static class AutoMapperConfiguration
{
	public static MapperConfiguration Configuration = new MapperConfiguration(cfg =>
	{
		cfg.AllowNullCollections = true;
		cfg.ShouldMapMethod = (m => false);
		cfg.AddProfile(new UserMapperProfile());
		cfg.AddProfile(new PostMapperProfile());
		cfg.AddProfile(new UserPostMappingProfile());
	});
}
