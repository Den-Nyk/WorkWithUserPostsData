using WorkWithUserPostsData.Application.Interfaces.Services;
using WorkWithUserPostsData.Domain.Models.Posts;
using WorkWithUserPostsData.Domain.Models.Users;

namespace WorkWithUserPostsData.Infrastructure.Services;

public class UserPostMappingService : IUserPostMappingService
{
	private readonly IUserService _userService;
	private readonly IPostService _postService;
	private List<(User User, List<Post> Posts)>? _mappedUsersWithPosts;
	private int? _mappedCount;

	public UserPostMappingService(IUserService userService, IPostService postService)
	{
		_userService = userService;
		_postService = postService;
	}

	private async Task EnsureMappedCountLoadedAsync()
	{
		if (_mappedCount.HasValue)
			return;

		await LoadAndMapAsync();
		_mappedCount = _mappedUsersWithPosts?.Count ?? 0;
	}

	private async Task LoadAndMapAsync()
	{
		if (_mappedUsersWithPosts != null)
			return;

		var users = await _userService.GetUsersAsync();
		var posts = await _postService.GetPostsAsync();

		_mappedUsersWithPosts = users
			.Select(user => (
				User: user,
				Posts: posts.Where(p => p.UserId == user.Id).ToList()
			)).ToList();

		_mappedCount = _mappedUsersWithPosts.Count;
	}

	public async Task<int> GetMappedCountAsync()
	{
		await EnsureMappedCountLoadedAsync();
		return _mappedCount ?? 0;
	}

	public async Task<(int totalCount, List<(User User, List<Post> Posts)> items)> GetFilteredAsync(Func<User, bool> userFilter, int take = 0, int skip = 0)
	{
		await LoadAndMapAsync();

		var filtered = _mappedUsersWithPosts!
			.Where(x => userFilter(x.User));

		var totalCount = filtered.Count();

		if (take <= 0)
			return (totalCount, filtered.Skip(skip).ToList());
		else
			return (totalCount, filtered.Skip(skip).Take(take).ToList());
	}

	public async Task<List<(User User, List<Post> Posts)>> GetMappedAsync(int take = 0, int skip = 0)
	{
		await LoadAndMapAsync();

		if (take <= 0)
			return _mappedUsersWithPosts!.Skip(skip).ToList();

		return _mappedUsersWithPosts!.Skip(skip).Take(take).ToList();
	}
}
