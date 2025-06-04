using WorkWithUserPostsData.Infrastructure.Services;

namespace WorkWithUserPostsData.IntegrationTests.Services;

public class UserServiceIntegrationTests
{
	private readonly int NumberOfUsersInJsonResource = 10;

	[Fact]
	public async Task GetUsersAsync_FromDefaultUrl_ReturnsValidUsers()
	{
		// Arrange
		var service = new UserService();

		// Act
		var users = await service.GetUsersAsync();

		// Assert
		Assert.NotNull(users);
		Assert.NotEmpty(users);
		Assert.All(users, user =>
		{
			Assert.True(user.Id > 0);
			Assert.False(string.IsNullOrWhiteSpace(user.Name));
			Assert.NotNull(user.Address);
			Assert.NotNull(user.Company);
		});
	}

	[Fact]
	public async Task GetUsersAsync_TakeAndSkip_WorksCorrectly_FromUrl()
	{
		var service = new UserService();

		var usersTake5 = await service.GetUsersAsync(take: 5);
		var usersSkip5 = await service.GetUsersAsync(skip: 5);

		Assert.Equal(5, usersTake5.Count);
		Assert.Equal(5, usersSkip5.Count);
		Assert.DoesNotContain(usersTake5[0], usersSkip5);
	}

	[Fact]
	public async Task GetUserCountAsync_FromDefaultUrl_ReturnsExpectedCount()
	{
		// Arrange
		var service = new UserService();

		// Act
		var count = await service.GetUserCountAsync();

		// Assert
		Assert.Equal(NumberOfUsersInJsonResource, count);
	}
}
