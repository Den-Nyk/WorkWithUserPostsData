using Microsoft.AspNetCore.Mvc;
using WorkWithUserPostsData.Application.Queries.V1.UserPosts;

namespace WorkWithUserPostsData.Api.Controllers.V1;

[Route("api/v1/[controller]")]
[ApiController]
public class UserPostsController : BaseController
{
	[HttpGet("mapped")]
	public async Task<IActionResult> GetMappedUserPosts([FromQuery] GetMappedUserPostsQuery query, CancellationToken cancellationToken)
	{
		return Ok(await Mediator.Send(query, cancellationToken));
	}

	[HttpGet("mapped/filter-by-city")]
	public async Task<IActionResult> FilterByCityStartUserPosts([FromQuery] FilterByCityStartUserPostsQuery query, CancellationToken cancellationToken)
	{
		return Ok(await Mediator.Send(query, cancellationToken));
	}

	[HttpGet("mapped/filter-by-company-name")]
	public async Task<IActionResult> FilterByCompanyNameStartUserPosts([FromQuery] FilterByCompanyNameStartUserPostsQuery query, CancellationToken cancellationToken)
	{
		return Ok(await Mediator.Send(query, cancellationToken));
	}
}
