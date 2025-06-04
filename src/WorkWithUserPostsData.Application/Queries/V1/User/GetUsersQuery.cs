using MediatR;
using WorkWithUserPostsData.Application.Dtos.User;
using WorkWithUserPostsData.Application.Interfaces;
using WorkWithUserPostsData.Application.Interfaces.Responses;

namespace WorkWithUserPostsData.Application.Queries.V1.User;

public class GetUsersQuery : IPaginationOptions, IRequest<IPaginatedResponse<List<UserDto>>>
{
	public int Skip { get; set; }
	public int Take { get; set; }
}
