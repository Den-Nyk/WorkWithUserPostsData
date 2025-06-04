using WorkWithUserPostsData.Domain.Models.Users;

namespace WorkWithUserPostsData.Application.Interfaces.Services;

public interface IUserService
{
	Task<List<User>> GetUsersAsync(int take = 0, int skip = 0);
	Task<int> GetUserCountAsync();
}
