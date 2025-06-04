namespace WorkWithUserPostsData.Application.Interfaces.Responses;

public interface IResponse
{
	bool IsSuccess { get; set; }
	IError Error { get; set; }
}

public interface IResponse<T> : IResponse
{
	T Data { get; set; }
	string ActionType { get; set; }
}
