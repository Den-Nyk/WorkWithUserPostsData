using WorkWithUserPostsData.Application.Configurations;
using WorkWithUserPostsData.Application.Interfaces.Services;
using WorkWithUserPostsData.Infrastructure.Services;

namespace WorkWithUserPostsData.Api.Extensions;

public static class ServiceCollectionExtensions
{
	public static void AddServices(this IServiceCollection services)
	{
		services.AddSingleton(sp => AutoMapperConfiguration.Configuration.CreateMapper());
		services.AddSingleton<IUserService, UserService>();
		services.AddSingleton<IPostService, PostService>();
		services.AddSingleton<IUserPostMappingService, UserPostMappingService>();
	}
}
