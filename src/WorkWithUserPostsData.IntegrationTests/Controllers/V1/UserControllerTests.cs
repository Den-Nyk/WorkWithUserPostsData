using System.Net.Http.Json;
using System.Net;
using WorkWithUserPostsData.Application.Dtos.User;
using WorkWithUserPostsData.Application.Models.Responses;
using FluentAssertions;
using WorkWithUserPostsData.Domain.Enums;
using Microsoft.AspNetCore.Mvc.Testing;

namespace WorkWithUserPostsData.IntegrationTests.Controllers.V1;

[Collection("Integration Tests")]
public class UserControllerTests
{
	private readonly HttpClient _client;

	public UserControllerTests(WebApplicationFactory<Program> factory)
	{
		_client = factory.CreateClient();
	}

	[Fact]
	public async Task GetUsers_ReturnsOkAndUsers()
	{
		var response = await _client.GetAsync("/api/v1/User");

		response.StatusCode.Should().Be(HttpStatusCode.OK);

		var responseBody = await response.Content.ReadFromJsonAsync<PaginatedResponse<List<UserDto>>>();
		responseBody.Should().NotBeNull();
		responseBody!.Data.Should().NotBeNullOrEmpty();
		responseBody.Total.Should().BeGreaterThan(0);
		responseBody.IsSuccess.Should().BeTrue();
		responseBody.ActionType.Should().Be(ActionType.getUsers.ToString());
	}

	[Theory]
	[InlineData(0, 5)]
	[InlineData(5, 5)]
	[InlineData(0, 1)]
	[InlineData(0, 100)]
	public async Task GetUsers_WithPagination_WorksCorrectly(int skip, int take)
	{
		var response = await _client.GetAsync($"/api/v1/User?skip={skip}&take={take}");

		response.StatusCode.Should().Be(HttpStatusCode.OK);

		var result = await response.Content.ReadFromJsonAsync<PaginatedResponse<List<UserDto>>>();

		result.Should().NotBeNull();
		result!.Data.Should().NotBeNull();
		result.Data.Count.Should().BeLessThanOrEqualTo(take);
		result.Total.Should().BeGreaterThan(0);
		result.IsSuccess.Should().BeTrue();
	}

	[Theory]
	[InlineData(-1, 5)]
	[InlineData(0, -10)]
	public async Task GetUsers_WithInvalidPagination_ReturnsOkAndIgnoresInvalidParams(int skip, int take)
	{
		var response = await _client.GetAsync($"/api/v1/User?skip={skip}&take={take}");

		response.StatusCode.Should().Be(HttpStatusCode.OK);

		var result = await response.Content.ReadFromJsonAsync<PaginatedResponse<List<UserDto>>>();

		result.Should().NotBeNull();
		result!.Data.Should().NotBeNullOrEmpty();
		result.Total.Should().BeGreaterThan(0);
	}

	[Fact]
	public async Task GetUsers_WithLargeSkip_ReturnsEmptyList()
	{
		var response = await _client.GetAsync("/api/v1/User?skip=10000&take=10");

		response.StatusCode.Should().Be(HttpStatusCode.OK);

		var result = await response.Content.ReadFromJsonAsync<PaginatedResponse<List<UserDto>>>();

		result.Should().NotBeNull();
		result!.Data.Should().BeEmpty();
		result.Total.Should().BeGreaterThan(0);
	}
}
