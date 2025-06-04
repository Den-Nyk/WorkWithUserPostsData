using WorkWithUserPostsData.Domain.Models.Posts;

namespace WorkWithUserPostsData.Application.Interfaces.Services;

public interface IPostService
{
	Task<List<Post>> GetPostsAsync(int take = 0, int skip = 0);
	Task<int> GetPostCountAsync();
}
