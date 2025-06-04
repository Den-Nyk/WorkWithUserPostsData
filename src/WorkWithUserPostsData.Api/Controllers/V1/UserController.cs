using Microsoft.AspNetCore.Mvc;
using WorkWithUserPostsData.Application.Queries.V1.User;

namespace WorkWithUserPostsData.Api.Controllers.V1;

[Route("api/v1/[controller]")]
[ApiController]
public class UserController : BaseController
{
	[HttpGet]
	public async Task<IActionResult> GetUsers([FromQuery] GetUsersQuery query, CancellationToken cancellationToken)
	{
		return Ok(await Mediator.Send(query, cancellationToken));
	}
}
