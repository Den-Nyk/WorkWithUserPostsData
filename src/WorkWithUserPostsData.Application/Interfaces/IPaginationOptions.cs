namespace WorkWithUserPostsData.Application.Interfaces;

public interface IPaginationOptions
{
	public int Skip { get; }
	public int Take { get; }
}