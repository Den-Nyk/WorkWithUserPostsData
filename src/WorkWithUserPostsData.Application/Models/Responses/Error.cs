using WorkWithUserPostsData.Application.Extensions;
using WorkWithUserPostsData.Application.Interfaces.Responses;
using WorkWithUserPostsData.Domain.Enums;

namespace WorkWithUserPostsData.Application.Models.Responses;

public class Error : IError
{
	public string Type { get; set; }

	public int Code { get; set; }

	public string Message { get; set; }

	public Error()
	{
	}

	public Error(ErrorCode code, ErrorType type, string message = null)
	{
		Code = (int)code;
		Type = type.ToString();
		Message = message ?? code.GetDescription();
	}

	public static Error ServerError(ErrorCode code, string message = null)
		=> new Error(code, ErrorType.InternalServerError, message);

	public static Error ServerError(string message)
		=> new Error(ErrorCode.Unknown, ErrorType.InternalServerError, message);

	public override string ToString()
	{
		return $"Error: Type - {Type}, Code - {Code}, Message {Message}";
	}
}
