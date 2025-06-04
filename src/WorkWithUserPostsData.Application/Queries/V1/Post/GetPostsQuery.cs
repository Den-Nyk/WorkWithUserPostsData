using MediatR;
using WorkWithUserPostsData.Application.Interfaces.Responses;
using WorkWithUserPostsData.Application.Interfaces;
using WorkWithUserPostsData.Application.Dtos.Post;

namespace WorkWithUserPostsData.Application.Queries.V1.Post;

public class GetPostsQuery : IPaginationOptions, IRequest<IPaginatedResponse<List<PostDto>>>
{
	public int Skip { get; set; }
	public int Take { get; set; }
}
