using WorkWithUserPostsData.Application.Interfaces.Responses;
using WorkWithUserPostsData.Domain.Enums;

namespace WorkWithUserPostsData.Application.Models.Responses;

public class Response<T> : Response, IResponse<T>
{
	public T Data { get; set; }
	public Response() { }

	public Response(IError error, ActionType actionType = Domain.Enums.ActionType.unknown)
		: base(error, actionType)
	{
	}

	public Response(T data, ActionType actionType)
		: base(actionType)
	{
		if (data == null)
			throw new ArgumentNullException(nameof(data));

		Data = data;
		ActionType = actionType.ToString();
	}
}

public class Response : IResponse
{
	public static readonly Response Success = new Response();

	public static readonly Task<Response> Task = System.Threading.Tasks.Task.FromResult(Success);

	public bool IsSuccess { get; set; }

	public IError? Error { get; set; }

	public string ActionType { get; set; }

	public Response(ActionType actionType)
	{
		IsSuccess = true;
		ActionType = actionType.ToString();
	}

	public Response(IError error = null, ActionType actionType = Domain.Enums.ActionType.unknown)
	{
		ActionType = actionType.ToString();
		IsSuccess = false;
		Error = error;
	}
}
