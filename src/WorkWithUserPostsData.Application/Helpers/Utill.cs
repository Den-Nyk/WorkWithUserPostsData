namespace WorkWithUserPostsData.Application.Helpers;

public static class Utill
{
	public static void ThrowIfNull<T>(T item, string message)
		where T : class
	{
		if (item == null)
			throw new ArgumentNullException(message);
	}

	public static void ThrowIfNullOrEmpty(this string text, string message)
	{
		if (string.IsNullOrEmpty(text))
			throw new ArgumentNullException(message);
	}

	public static void ThrowIfNonPositive(this long number, string message)
	{
		if (number <= 0)
			throw new ArgumentException(message);
	}
}
