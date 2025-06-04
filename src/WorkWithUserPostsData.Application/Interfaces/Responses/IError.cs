namespace WorkWithUserPostsData.Application.Interfaces.Responses;

public interface IError
{
	int Code { get; }

	string Type { get; }

	string Message { get; }
}
