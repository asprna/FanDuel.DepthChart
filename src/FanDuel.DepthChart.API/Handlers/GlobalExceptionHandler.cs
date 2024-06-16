using FanDuel.DepthChart.Application.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using FluentValidation;

namespace FanDuel.DepthChart.API.Handlers
{
    public class GlobalExceptionHandler : IExceptionHandler
    {
        private readonly ILogger<GlobalExceptionHandler> _logger;

        public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
        {
            _logger = logger;
        }

        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            _logger.LogError(exception, $"Exception occurred: {exception.Message}");

            var problemDetails = exception switch
            {
                AppException appException => new ProblemDetails
                {
                    Status = appException.StatusCode,
                    Title = appException.Title,
                    Detail = appException.Details,
                },
                ValidationException validationException => new ValidationProblemDetails(validationException.Errors
                    .ToDictionary(e => e.PropertyName, e => new string[] { e.ErrorMessage }))
                {
                    Status = StatusCodes.Status422UnprocessableEntity,
                    Title = "Validation Error",
                    Detail = "One or more validation errors occurred.",
                    Errors = validationException.Errors
                    .ToDictionary(e => e.PropertyName, e => new string[] { e.ErrorMessage })
                },
                _ => new ProblemDetails
                {
                    Status = StatusCodes.Status500InternalServerError,
                    Title = "Server Error",
                    Detail = "Unhandle Exception",
                },
            };

            httpContext.Response.StatusCode = (int)problemDetails.Status;

            if (exception is ValidationException)
            {
                await httpContext.Response.WriteAsJsonAsync<ValidationProblemDetails>((ValidationProblemDetails)problemDetails, cancellationToken);
            }
            else
            {
                await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);
            }

            return true;
        }
    }
}
