using MediatR;
using WorkWithUserPostsData.Application.Dtos.UserPost;
using WorkWithUserPostsData.Application.Interfaces.Responses;

namespace WorkWithUserPostsData.Application.Queries.V1.UserPosts;

public class GetMappedUserPostsQuery : IRequest<IPaginatedResponse<List<UserPostsDto>>>
{
	public int Skip { get; set; }
	public int Take { get; set; }
}
