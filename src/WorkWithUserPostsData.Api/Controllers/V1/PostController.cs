using Microsoft.AspNetCore.Mvc;
using WorkWithUserPostsData.Application.Queries.V1.Post;

namespace WorkWithUserPostsData.Api.Controllers.V1;

[Route("api/v1/[controller]")]
[ApiController]
public class PostController : BaseController
{
	[HttpGet]
	public async Task<IActionResult> GetPosts([FromQuery] GetPostsQuery query, CancellationToken cancellationToken)
	{
		return Ok(await Mediator.Send(query, cancellationToken));
	}
}
