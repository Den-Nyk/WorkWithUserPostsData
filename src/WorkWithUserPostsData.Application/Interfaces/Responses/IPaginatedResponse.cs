namespace WorkWithUserPostsData.Application.Interfaces.Responses;

public interface IPaginatedResponse<T> : IResponse<T>
{
	int Total { get; set; }
}
