using AutoMapper;
using MediatR;
using WorkWithUserPostsData.Application.Dtos.User;
using WorkWithUserPostsData.Application.Factories;
using WorkWithUserPostsData.Application.Interfaces.Responses;
using WorkWithUserPostsData.Application.Interfaces.Services;
using WorkWithUserPostsData.Application.Queries.V1.User;
using WorkWithUserPostsData.Domain.Enums;

namespace WorkWithUserPostsData.Infrastructure.Handlers.V1.User.Get;

public class GetUsersQueryHandler : IRequestHandler<GetUsersQuery, IPaginatedResponse<List<UserDto>>>
{
	private readonly IUserService _userService;
	private readonly IMapper _mapper;

	public GetUsersQueryHandler(IUserService userService,
								IMapper mapper)
	{
		_userService = userService;
		_mapper = mapper;
	}

	public async Task<IPaginatedResponse<List<UserDto>>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
	{
		return ResponseFactory.Success(_mapper.Map<List<UserDto>>(await _userService.GetUsersAsync(request.Take, request.Skip)), await _userService.GetUserCountAsync(), ActionType.getUsers);
	}
}
