using WorkWithUserPostsData.Application.Interfaces.Responses;
using WorkWithUserPostsData.Application.Models.Responses;
using WorkWithUserPostsData.Domain.Enums;

namespace WorkWithUserPostsData.Application.Factories;

public static class ResponseFactory
{
	public static IResponse Success(ActionType actionType) => new Response(actionType);
	public static IResponse<T> Success<T>(T data, ActionType actionType) => new Response<T>(data, actionType);
	public static IPaginatedResponse<T> Success<T>(T data, int total, ActionType actionType) => new PaginatedResponse<T>(data, actionType, total);

	public static IResponse Failed(IError error) => new Response(error);
	public static IResponse<T> Failed<T>(IError error) => new Response<T>(error);

	public static IResponse ServerError(ErrorCode code, ActionType actionType, string message = null) => new Response(Error.ServerError(code, message), actionType);
	public static IResponse ServerError(string message) => new Response(Error.ServerError(message));

	public static IResponse<T> ServerError<T>(ErrorCode code, ActionType actionType, string message = null) => new Response<T>(Error.ServerError(code, message), actionType);
	public static IResponse<T> ServerError<T>(string message) => new Response<T>(Error.ServerError(message));
}
