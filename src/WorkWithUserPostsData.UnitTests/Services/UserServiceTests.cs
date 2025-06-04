using WorkWithUserPostsData.Infrastructure.Services;

namespace WorkWithUserPostsData.UnitTests.Services;

public class UserServiceUnitTests
{
	private readonly string _testJson = """
        [
          {
            "id": 1,
            "name": "John Doe",
            "username": "johndoe",
            "email": "john@example.com",
            "address": {
              "street": "Main St",
              "suite": "Apt. 1",
              "city": "Metropolis",
              "zipcode": "12345",
              "geo": { "lat": "40.0", "lng": "-70.0" }
            },
            "phone": "123-456-7890",
            "website": "example.com",
            "company": {
              "name": "Example Inc",
              "catchPhrase": "We do stuff",
              "bs": "business solutions"
            }
          },
          {
            "id": 2,
            "name": "Jane Smith",
            "username": "janesmith",
            "email": "jane@example.com",
            "address": {
              "street": "Second St",
              "suite": "Apt. 2",
              "city": "Gotham",
              "zipcode": "54321",
              "geo": { "lat": "42.0", "lng": "-72.0" }
            },
            "phone": "987-654-3210",
            "website": "example.net",
            "company": {
              "name": "Example LLC",
              "catchPhrase": "We do other stuff",
              "bs": "business ideas"
            }
          }
        ]
    """;

	private string CreateTempJsonFile(string content)
	{
		string path = Path.GetTempFileName();
		File.WriteAllText(path, content);
		return path;
	}

	[Fact]
	public async Task GetUsersAsync_FromLocalFile_ReturnsAllUsers_WhenTakeIsZero()
	{
		// Arrange
		var filePath = CreateTempJsonFile(_testJson);
		var service = new UserService(filePath);

		// Act
		var users = await service.GetUsersAsync(take: 0);

		// Assert
		Assert.NotNull(users);
		Assert.Equal(2, users.Count);
	}

	[Fact]
	public async Task GetUsersAsync_TakeAndSkip_WorksCorrectly()
	{
		// Arrange
		var filePath = CreateTempJsonFile(_testJson);
		var service = new UserService(filePath);

		// Act
		var usersTake1 = await service.GetUsersAsync(take: 1);
		var usersSkip1Take1 = await service.GetUsersAsync(take: 1, skip: 1);

		// Assert
		Assert.Single(usersTake1);
		Assert.Equal("John Doe", usersTake1[0].Name);

		Assert.Single(usersSkip1Take1);
		Assert.Equal("Jane Smith", usersSkip1Take1[0].Name);
	}

	[Fact]
	public async Task GetUsersAsync_SkipOnly_ReturnsCorrectUsers()
	{
		// Arrange
		var filePath = CreateTempJsonFile(_testJson);
		var service = new UserService(filePath);

		// Act
		var users = await service.GetUsersAsync(skip: 1);

		// Assert
		Assert.Single(users);
		Assert.Equal("Jane Smith", users[0].Name);
	}

	[Fact]
	public async Task GetUserCountAsync_FromLocalFile_ReturnsCorrectCount()
	{
		// Arrange
		var filePath = CreateTempJsonFile(_testJson);
		var service = new UserService(filePath);

		// Act
		var count = await service.GetUserCountAsync();

		// Assert
		Assert.Equal(2, count);
	}

	[Fact]
	public async Task GetUsersAsync_Twice_UsesCache()
	{
		// Arrange
		var filePath = CreateTempJsonFile(_testJson);
		var service = new UserService(filePath);

		// Act
		var users1 = await service.GetUsersAsync();
		var users2 = await service.GetUsersAsync();

		// Assert
		Assert.Equal(users1.Count, users2.Count);
	}

	[Fact]
	public async Task GetUserCountAsync_Twice_UsesCache()
	{
		// Arrange
		var filePath = CreateTempJsonFile(_testJson);
		var service = new UserService(filePath);

		// Act
		var count1 = await service.GetUserCountAsync();
		var count2 = await service.GetUserCountAsync();

		// Assert
		Assert.Equal(count1, count2);
	}

	[Fact]
	public async Task GetUsersAsync_ThenCount_ShouldReuseCachedUsers()
	{
		// Arrange
		var filePath = CreateTempJsonFile(_testJson);
		var service = new UserService(filePath);

		// Act
		var users = await service.GetUsersAsync();
		var count = await service.GetUserCountAsync();

		// Assert
		Assert.Equal(users.Count, count);
	}

	[Fact]
	public async Task GetUserCountAsync_ThenUsers_ShouldReuseCachedCount()
	{
		// Arrange
		var filePath = CreateTempJsonFile(_testJson);
		var service = new UserService(filePath);

		// Act
		var count = await service.GetUserCountAsync();
		var users = await service.GetUsersAsync();

		// Assert
		Assert.Equal(count, users.Count);
	}
}
