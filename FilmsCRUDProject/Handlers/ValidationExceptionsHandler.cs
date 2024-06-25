using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
namespace FilmsCRUDProject.Handlers
{
	public class ValidationExceptionsHandler : IExceptionHandler
	{
		public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
		{
			if (exception is ValidationException validationException)
			{
				Console.WriteLine("test");
				var problemDetails = new ProblemDetails()
				{
					Status = StatusCodes.Status400BadRequest,
					Title = "Validation error",
					Instance = $"{httpContext.Request.Method} {httpContext.Request.Path}",
					Extensions =
					{
						["errors"] = validationException.Errors.Select(error => new
						{
							error.PropertyName,
							error.ErrorMessage,
							error.AttemptedValue
						}).ToArray()
					}
				};
				
				await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);
                
				return true;
			}
			return false;
		}
	}
}