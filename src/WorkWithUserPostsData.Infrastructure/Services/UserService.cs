using System.Text.Json;
using WorkWithUserPostsData.Application.Interfaces.Services;
using WorkWithUserPostsData.Domain.Models.Users;

namespace WorkWithUserPostsData.Infrastructure.Services;

public class UserService : IUserService
{
	private readonly string _sourcePath;
	private readonly bool _isUrl;
	private readonly HttpClient _httpClient;

	private List<User>? _cachedUsers;
	private int? _cachedUserCount;

	public UserService(string sourcePath = "https://jsonplaceholder.typicode.com/users")
	{
		_sourcePath = sourcePath;
		_isUrl = Uri.IsWellFormedUriString(sourcePath, UriKind.Absolute);
		_httpClient = new HttpClient();
	}

	private async Task<Stream> OpenStreamAsync()
	{
		return _isUrl
			? await _httpClient.GetStreamAsync(_sourcePath)
			: File.OpenRead(_sourcePath);
	}

	private async Task EnsureUsersLoadedAsync()
	{
		if (_cachedUsers != null)
			return;

		using var stream = await OpenStreamAsync();
		var users = await JsonSerializer.DeserializeAsync<List<User>>(stream, new JsonSerializerOptions
		{
			PropertyNameCaseInsensitive = true,
			ReadCommentHandling = JsonCommentHandling.Skip,
			AllowTrailingCommas = true
		});

		_cachedUsers = users ?? new List<User>();
		_cachedUserCount = _cachedUsers.Count;
	}

	private async Task EnsureUserCountLoadedAsync()
	{
		if (_cachedUserCount.HasValue)
			return;

		int count = 0;
		await using var stream = await OpenStreamAsync();
		var options = new JsonSerializerOptions
		{
			PropertyNameCaseInsensitive = true
		};

		await foreach (var user in JsonSerializer.DeserializeAsyncEnumerable<User>(stream, options))
		{
			if (user != null)
				count++;
		}

		_cachedUserCount = count;
	}

	public async Task<List<User>> GetUsersAsync(int take = 0, int skip = 0)
	{
		await EnsureUsersLoadedAsync();

		if (take <= 0)
			return _cachedUsers!.Skip(skip).ToList();

		return _cachedUsers!.Skip(skip).Take(take).ToList();
	}

	public async Task<int> GetUserCountAsync()
	{
		await EnsureUserCountLoadedAsync();
		return _cachedUserCount!.Value;
	}
}
