using FilmsCRUDProject.Data;
using FilmsCRUDProject.Dto;
using FilmsCRUDProject.Handlers;
using FilmsCRUDProject.Services;
using FilmsCRUDProject.Validators;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

namespace FilmsCRUDProject;

public static class Program
{
	public static void Main(string[] args)
	{
		var builder = WebApplication.CreateBuilder(args);

		builder.Services.AddControllers();
		
		builder.Services.AddTransient<UsersService>();
		builder.Services.AddTransient<FilmsService>();

		builder.Services.AddScoped<IValidator<UserDto>, UserDtoValidator>();
		builder.Services.AddScoped<IValidator<FilmDto>, FilmDtoValidator>();
		
		builder.Services.AddProblemDetails(options =>
		{
			options.CustomizeProblemDetails = ctx =>
			{
				ctx.ProblemDetails.Extensions.Add("instance", $"{ctx.HttpContext.Request.Method} {ctx.HttpContext.Request.Path}");
			};
		});
		
		builder.Services.AddExceptionHandler<ValidationExceptionsHandler>();
		
		var connectionString = builder.Configuration.GetConnectionString("FilmsCollectionDatabase");

		builder.Services.AddDbContext<AppDbContext>(options =>
			options.UseNpgsql(connectionString));

		builder.Services.AddSwaggerGen(options =>
		{
			options.SwaggerDoc("v1", new OpenApiInfo
			{
				Version = "v1",
				Title = "Films CRUD Application",
				Description = "An ASP.NET Core CRUD Web API"
			});
			
		});

		var app = builder.Build();
		
		if (app.Environment.IsDevelopment())
		{
			app.UseSwagger();
			app.UseSwaggerUI(options =>
			{
				options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
				options.RoutePrefix = string.Empty;
			});
		}
		
		app.UseExceptionHandler();

		app.UseStatusCodePages();

		app.MapControllers();

		app.Run();
	}
}