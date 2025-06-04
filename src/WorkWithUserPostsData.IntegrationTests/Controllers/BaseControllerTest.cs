using Microsoft.AspNetCore.Mvc.Testing;

namespace WorkWithUserPostsData.IntegrationTests.Controllers;

[CollectionDefinition("Integration Tests")]
public class BaseControllerTest : ICollectionFixture<WebApplicationFactory<Program>>
{
}
