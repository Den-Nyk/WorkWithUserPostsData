using WorkWithUserPostsData.Infrastructure.Services;

namespace WorkWithUserPostsData.IntegrationTests.Services;

public class PostServiceIntegrationTests
{
	private readonly int NumberOfPostsInJsonResource = 100;

	[Fact]
	public async Task GetPostsAsync_FromDefaultUrl_ReturnsValidPosts()
	{
		var service = new PostService();

		var posts = await service.GetPostsAsync();

		Assert.NotNull(posts);
		Assert.NotEmpty(posts);
		Assert.All(posts, post =>
		{
			Assert.True(post.Id > 0);
			Assert.False(string.IsNullOrWhiteSpace(post.Title));
			Assert.False(string.IsNullOrWhiteSpace(post.Body));
			Assert.True(post.UserId > 0);
		});
	}

	[Fact]
	public async Task GetPostsAsync_TakeAndSkip_WorksCorrectly_FromUrl()
	{
		var service = new PostService();

		var postsTake5 = await service.GetPostsAsync(take: 5);
		var postsSkip5 = await service.GetPostsAsync(skip: 5);

		Assert.Equal(5, postsTake5.Count);
		Assert.Equal(NumberOfPostsInJsonResource - 5, postsSkip5.Count);
		Assert.DoesNotContain(postsTake5[0], postsSkip5);
	}

	[Fact]
	public async Task GetPostCountAsync_FromDefaultUrl_ReturnsExpectedCount()
	{
		var service = new PostService();

		var count = await service.GetPostCountAsync();

		Assert.Equal(NumberOfPostsInJsonResource, count);
	}
}
