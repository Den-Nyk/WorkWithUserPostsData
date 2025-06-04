using WorkWithUserPostsData.Infrastructure.Services;

namespace WorkWithUserPostsData.UnitTests.Services;

public class PostServiceUnitTests
{
	private readonly string _testJson = """
    [
      {
        "userId": 1,
        "id": 1,
        "title": "Post Title 1",
        "body": "Body of post 1"
      },
      {
        "userId": 2,
        "id": 2,
        "title": "Post Title 2",
        "body": "Body of post 2"
      }
    ]
    """;

	private string CreateTempJsonFile(string content)
	{
		var path = Path.GetTempFileName();
		File.WriteAllText(path, content);
		return path;
	}

	[Fact]
	public async Task GetPostsAsync_FromLocalFile_ReturnsAllPosts_WhenTakeIsZero()
	{
		var filePath = CreateTempJsonFile(_testJson);
		var service = new PostService(filePath);

		var posts = await service.GetPostsAsync(take: 0);

		Assert.NotNull(posts);
		Assert.Equal(2, posts.Count);
	}

	[Fact]
	public async Task GetPostsAsync_TakeAndSkip_WorksCorrectly()
	{
		var filePath = CreateTempJsonFile(_testJson);
		var service = new PostService(filePath);

		var postsTake1 = await service.GetPostsAsync(take: 1);
		var postsSkip1Take1 = await service.GetPostsAsync(take: 1, skip: 1);

		Assert.Single(postsTake1);
		Assert.Equal("Post Title 1", postsTake1[0].Title);

		Assert.Single(postsSkip1Take1);
		Assert.Equal("Post Title 2", postsSkip1Take1[0].Title);
	}

	[Fact]
	public async Task GetPostsAsync_SkipOnly_ReturnsCorrectPosts()
	{
		var filePath = CreateTempJsonFile(_testJson);
		var service = new PostService(filePath);

		var posts = await service.GetPostsAsync(skip: 1);

		Assert.Single(posts);
		Assert.Equal("Post Title 2", posts[0].Title);
	}

	[Fact]
	public async Task GetPostCountAsync_FromLocalFile_ReturnsCorrectCount()
	{
		var filePath = CreateTempJsonFile(_testJson);
		var service = new PostService(filePath);

		var count = await service.GetPostCountAsync();

		Assert.Equal(2, count);
	}

	[Fact]
	public async Task GetPostsAsync_Twice_UsesCache()
	{
		var filePath = CreateTempJsonFile(_testJson);
		var service = new PostService(filePath);

		var posts1 = await service.GetPostsAsync();
		var posts2 = await service.GetPostsAsync();

		Assert.Equal(posts1.Count, posts2.Count);
	}

	[Fact]
	public async Task GetPostCountAsync_Twice_UsesCache()
	{
		var filePath = CreateTempJsonFile(_testJson);
		var service = new PostService(filePath);

		var count1 = await service.GetPostCountAsync();
		var count2 = await service.GetPostCountAsync();

		Assert.Equal(count1, count2);
	}

	[Fact]
	public async Task GetPostsAsync_ThenCount_ShouldReuseCachedPosts()
	{
		var filePath = CreateTempJsonFile(_testJson);
		var service = new PostService(filePath);

		var posts = await service.GetPostsAsync();
		var count = await service.GetPostCountAsync();

		Assert.Equal(posts.Count, count);
	}

	[Fact]
	public async Task GetPostCountAsync_ThenPosts_ShouldReuseCachedCount()
	{
		var filePath = CreateTempJsonFile(_testJson);
		var service = new PostService(filePath);

		var count = await service.GetPostCountAsync();
		var posts = await service.GetPostsAsync();

		Assert.Equal(count, posts.Count);
	}
}
