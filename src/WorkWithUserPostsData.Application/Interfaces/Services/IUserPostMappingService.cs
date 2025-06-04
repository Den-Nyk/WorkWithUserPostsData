using WorkWithUserPostsData.Domain.Models.Posts;
using WorkWithUserPostsData.Domain.Models.Users;

namespace WorkWithUserPostsData.Application.Interfaces.Services;

public interface IUserPostMappingService
{
	Task<int> GetMappedCountAsync();
	Task<List<(User User, List<Post> Posts)>> GetMappedAsync(int take = 0, int skip = 0);
	Task<(int totalCount, List<(User User, List<Post> Posts)> items)> GetFilteredAsync(Func<User, bool> userFilter, int take = 0, int skip = 0);
}
