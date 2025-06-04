using AutoMapper;
using MediatR;
using WorkWithUserPostsData.Application.Dtos.Post;
using WorkWithUserPostsData.Application.Factories;
using WorkWithUserPostsData.Application.Interfaces.Responses;
using WorkWithUserPostsData.Application.Interfaces.Services;
using WorkWithUserPostsData.Application.Queries.V1.Post;
using WorkWithUserPostsData.Domain.Enums;

namespace WorkWithUserPostsData.Infrastructure.Handlers.V1.Post.Get;

public class GetPostsQueryHandler : IRequestHandler<GetPostsQuery, IPaginatedResponse<List<PostDto>>>
{
	private readonly IPostService _postService;
	private readonly IMapper _mapper;

	public GetPostsQueryHandler(IPostService postService,
								IMapper mapper)
	{
		_postService = postService;
		_mapper = mapper;
	}

	public async Task<IPaginatedResponse<List<PostDto>>> Handle(GetPostsQuery request, CancellationToken cancellationToken)
	{
		var posts = await _postService.GetPostsAsync(request.Take, request.Skip);
		var totalCount = await _postService.GetPostCountAsync();

		var mapped = _mapper.Map<List<PostDto>>(posts);

		return ResponseFactory.Success(mapped, totalCount, ActionType.getPosts);
	}
}
