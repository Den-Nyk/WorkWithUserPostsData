using MediatR;
using System.Reflection;
using WorkWithUserPostsData.Api.Extensions;
using WorkWithUserPostsData.Application.Queries.V1.UserPosts;

var builder = WebApplication.CreateBuilder(args);

var dllPatterns = new[] { "*Application.dll", "*Infrastructure.dll" };

var paths = dllPatterns
	.SelectMany(pattern => Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, pattern))
	.Distinct()
	.ToList();

var assemblies = paths
	.Select(path => Assembly.LoadFrom(path))
	.ToArray();

builder.Services.AddMediatR(cfg =>
{
	cfg.RegisterServicesFromAssemblies(assemblies);
});

builder.Services.AddServices();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
	var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

	var result = await mediator.Send(new FilterByCityStartUserPostsQuery
	{
		StartWith = "S",
		Skip = 0,
		Take = 10
	});

	Console.WriteLine(" === Filtered UserPosts by City Starting with 'S' ===");
	Console.WriteLine($" === Total count: {result.Total} ===\n");
	foreach (var item in result.Data)
	{
		Console.WriteLine($" Name: {item?.UserName}\n City: {item?.City}\n Posts count: {item?.CountOfPosts}");
		Console.WriteLine("_______________________________________________________\n");
	}
}

if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();

public partial class Program { }
