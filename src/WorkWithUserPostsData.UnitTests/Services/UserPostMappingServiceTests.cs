using Moq;
using WorkWithUserPostsData.Application.Interfaces.Services;
using WorkWithUserPostsData.Domain.Models.Posts;
using WorkWithUserPostsData.Domain.Models.Users;
using WorkWithUserPostsData.Infrastructure.Services;

namespace WorkWithUserPostsData.UnitTests.Services;

public class UserPostMappingServiceTests
{
	private readonly Mock<IUserService> _mockUserService;
	private readonly Mock<IPostService> _mockPostService;

	public UserPostMappingServiceTests()
	{
		_mockUserService = new Mock<IUserService>();
		_mockPostService = new Mock<IPostService>();
	}

	private UserPostMappingService CreateService()
	{
		return new UserPostMappingService(_mockUserService.Object, _mockPostService.Object);
	}

	private void SetupMockData()
	{
		_mockUserService
			.Setup(u => u.GetUsersAsync(It.IsAny<int>(), It.IsAny<int>()))
			.ReturnsAsync(GetTestUsers());

		_mockPostService
			.Setup(p => p.GetPostsAsync(It.IsAny<int>(), It.IsAny<int>()))
			.ReturnsAsync(GetTestPosts());
	}

	private List<User> GetTestUsers()
	{
		return new List<User>
		{
			new User { Id = 1, Name = "Alice", Address = new Address { City = "New York" }, Company = new Company { Name = "AliceCo" } },
			new User { Id = 2, Name = "Bob", Address = new Address { City = "Los Angeles" }, Company = new Company { Name = "BobCorp" } },
			new User { Id = 3, Name = "Charlie", Address = new Address { City = "Chicago" }, Company = new Company { Name = "Charlie Ltd" } },
			new User { Id = 4, Name = "Dave", Address = new Address { City = "Newark" }, Company = new Company { Name = "DaveWorks" } },
		};
	}

	private List<Post> GetTestPosts()
	{
		return new List<Post>
		{
			new Post { Id = 1, UserId = 1, Title = "Post A" },
			new Post { Id = 2, UserId = 1, Title = "Post B" },
			new Post { Id = 3, UserId = 2, Title = "Post C" },
			new Post { Id = 4, UserId = 4, Title = "Post D" },
		};
	}

	[Fact]
	public async Task GetMappedCountAsync_ReturnsCorrectCount()
	{
		SetupMockData();
		var service = CreateService();

		var count = await service.GetMappedCountAsync();

		Assert.Equal(4, count);
	}

	[Fact]
	public async Task GetMappedAsync_ReturnsAllMapped_WhenTakeZero()
	{
		SetupMockData();
		var service = CreateService();

		var result = await service.GetMappedAsync();

		Assert.Equal(4, result.Count);
		Assert.Contains(result, x => x.User.Id == 1 && x.Posts.Count == 2);
		Assert.Contains(result, x => x.User.Id == 4 && x.Posts.Count == 1);
	}

	[Fact]
	public async Task GetMappedAsync_ReturnsCorrectPage()
	{
		SetupMockData();
		var service = CreateService();

		var result = await service.GetMappedAsync(take: 2, skip: 2);

		Assert.Equal(2, result.Count);
		Assert.Equal(3, result[0].User.Id);
		Assert.Equal(4, result[1].User.Id);
	}

	[Fact]
	public async Task GetMappedAsync_ReturnsEmpty_WhenSkipExceedsCount()
	{
		SetupMockData();
		var service = CreateService();

		var result = await service.GetMappedAsync(take: 2, skip: 10);

		Assert.Empty(result);
	}

	[Fact]
	public async Task GetFilteredAsync_ByCityStartWithN_ReturnsCorrectUsers()
	{
		SetupMockData();
		var service = CreateService();

		var (totalCount, items) = await service.GetFilteredAsync(
			user => user.Address?.City != null && user.Address.City.StartsWith("N", StringComparison.OrdinalIgnoreCase)
		);

		Assert.Equal(2, totalCount);
		Assert.All(items, x => Assert.StartsWith("N", x.User.Address.City, StringComparison.OrdinalIgnoreCase));
	}

	[Fact]
	public async Task GetFilteredAsync_ByCityStartWithZ_ReturnsEmpty()
	{
		SetupMockData();
		var service = CreateService();

		var (totalCount, items) = await service.GetFilteredAsync(
			user => user.Address?.City != null && user.Address.City.StartsWith("Z", StringComparison.OrdinalIgnoreCase)
		);

		Assert.Equal(0, totalCount);
		Assert.Empty(items);
	}

	[Fact]
	public async Task GetFilteredAsync_ByCompanyStartWithAlice_ReturnsAlice()
	{
		SetupMockData();
		var service = CreateService();

		var (totalCount, items) = await service.GetFilteredAsync(
			user => user.Company?.Name != null && user.Company.Name.StartsWith("Alice", StringComparison.OrdinalIgnoreCase)
		);

		Assert.Equal(1, totalCount);
		Assert.Single(items);
		Assert.Equal("AliceCo", items[0].User.Company.Name);
	}

	[Fact]
	public async Task GetFilteredAsync_ByCompanyStartWithX_ReturnsEmpty()
	{
		SetupMockData();
		var service = CreateService();

		var (totalCount, items) = await service.GetFilteredAsync(
			user => user.Company?.Name != null && user.Company.Name.StartsWith("X", StringComparison.OrdinalIgnoreCase)
		);

		Assert.Equal(0, totalCount);
		Assert.Empty(items);
	}
}
