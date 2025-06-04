using AutoMapper;
using MediatR;
using WorkWithUserPostsData.Application.Dtos.UserPost;
using WorkWithUserPostsData.Application.Factories;
using WorkWithUserPostsData.Application.Interfaces.Responses;
using WorkWithUserPostsData.Application.Interfaces.Services;
using WorkWithUserPostsData.Application.Queries.V1.UserPosts;
using WorkWithUserPostsData.Domain.Enums;

namespace WorkWithUserPostsData.Infrastructure.Handlers.V1.UserPosts.Get;

public class GetMappedUserPostsQueryHandler : IRequestHandler<GetMappedUserPostsQuery, IPaginatedResponse<List<UserPostsDto>>>
{
	private readonly IMapper _mapper;
	private readonly IUserPostMappingService _userPostMappingService;

	public GetMappedUserPostsQueryHandler(IUserPostMappingService service, IMapper mapper)
	{
		_userPostMappingService = service;
		_mapper = mapper;
	}

	public async Task<IPaginatedResponse<List<UserPostsDto>>> Handle(GetMappedUserPostsQuery request, CancellationToken cancellationToken)
	{
		var mappedResponse = await _userPostMappingService.GetMappedAsync(request.Take, request.Skip);
		var totalCount = await _userPostMappingService.GetMappedCountAsync();

		return ResponseFactory.Success(_mapper.Map<List<UserPostsDto>>(mappedResponse), totalCount, ActionType.getPosts);
	}
}
