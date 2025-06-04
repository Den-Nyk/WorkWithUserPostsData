using AutoMapper;
using MediatR;
using WorkWithUserPostsData.Application.Dtos.UserPost;
using WorkWithUserPostsData.Application.Factories;
using WorkWithUserPostsData.Application.Interfaces.Responses;
using WorkWithUserPostsData.Application.Interfaces.Services;
using WorkWithUserPostsData.Application.Queries.V1.UserPosts;
using WorkWithUserPostsData.Domain.Enums;

namespace WorkWithUserPostsData.Infrastructure.Handlers.V1.UserPosts.Get;

public class FilterByCompanyNameStartUserPostsQueryHandler : IRequestHandler<FilterByCompanyNameStartUserPostsQuery, IPaginatedResponse<List<UserPostsDto>>>
{
	private readonly IMapper _mapper;
	private readonly IUserPostMappingService _userPostMappingService;

	public FilterByCompanyNameStartUserPostsQueryHandler(IUserPostMappingService service, IMapper mapper)
	{
		_userPostMappingService = service;
		_mapper = mapper;
	}

	public async Task<IPaginatedResponse<List<UserPostsDto>>> Handle(FilterByCompanyNameStartUserPostsQuery request, CancellationToken cancellationToken)
	{
		var filteredMapRequestsAndCount = await _userPostMappingService.GetFilteredAsync(user => user.Company?.Name?
			.StartsWith(request.StartWith, StringComparison.OrdinalIgnoreCase) == true, request.Take, request.Skip);

		var filteredMapRequests = filteredMapRequestsAndCount.items;
		var totalCount = filteredMapRequestsAndCount.totalCount;

		return ResponseFactory.Success(_mapper.Map<List<UserPostsDto>>(filteredMapRequests), totalCount, ActionType.getPostsFilterByCompanyNameStart);
	}
}
