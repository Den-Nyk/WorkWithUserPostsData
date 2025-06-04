using System.Text.Json;
using WorkWithUserPostsData.Application.Interfaces.Services;
using WorkWithUserPostsData.Domain.Models.Posts;

namespace WorkWithUserPostsData.Infrastructure.Services;

public class PostService : IPostService
{
	private readonly string _sourcePath;
	private readonly bool _isUrl;
	private readonly HttpClient _httpClient;

	private List<Post>? _cachedPosts;
	private int? _cachedPostCount;

	public PostService(string sourcePath = "https://jsonplaceholder.typicode.com/posts")
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

	private async Task EnsurePostsLoadedAsync()
	{
		if (_cachedPosts != null)
			return;

		using var stream = await OpenStreamAsync();
		var posts = await JsonSerializer.DeserializeAsync<List<Post>>(stream, new JsonSerializerOptions
		{
			PropertyNameCaseInsensitive = true,
			ReadCommentHandling = JsonCommentHandling.Skip,
			AllowTrailingCommas = true
		});

		_cachedPosts = posts ?? new List<Post>();
		_cachedPostCount = _cachedPosts.Count;
	}

	private async Task EnsurePostCountLoadedAsync()
	{
		if (_cachedPostCount.HasValue)
			return;

		int count = 0;
		await using var stream = await OpenStreamAsync();
		var options = new JsonSerializerOptions
		{
			PropertyNameCaseInsensitive = true
		};

		await foreach (var post in JsonSerializer.DeserializeAsyncEnumerable<Post>(stream, options))
		{
			if (post != null)
				count++;
		}

		_cachedPostCount = count;
	}

	public async Task<List<Post>> GetPostsAsync(int take = 0, int skip = 0)
	{
		await EnsurePostsLoadedAsync();

		if (take <= 0)
			return _cachedPosts!.Skip(skip).ToList();

		return _cachedPosts!.Skip(skip).Take(take).ToList();
	}

	public async Task<int> GetPostCountAsync()
	{
		await EnsurePostCountLoadedAsync();
		return _cachedPostCount!.Value;
	}
}
