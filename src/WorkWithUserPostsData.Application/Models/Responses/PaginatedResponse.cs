using WorkWithUserPostsData.Application.Interfaces.Responses;
using WorkWithUserPostsData.Domain.Enums;

namespace WorkWithUserPostsData.Application.Models.Responses;

public class PaginatedResponse<T> : Response<T>, IPaginatedResponse<T>
{
	public int Total { get; set; }

	public PaginatedResponse() { }

	public PaginatedResponse(T data, ActionType action, int total)
			: base(data, action)
	{
		Total = total;
	}

	public PaginatedResponse(IError error, ActionType action)
		: base(error, action) { }
}
