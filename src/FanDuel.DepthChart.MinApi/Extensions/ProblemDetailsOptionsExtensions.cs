using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;

namespace FanDuel.DepthChart.MinApi.Extensions
{
    /// <summary>
    /// Extension for the ProblemDetails.
    /// </summary>
    public static class ProblemDetailsOptionsExtensions
    {
        /// <summary>
        /// Configuration for the fluent validation.
        /// </summary>
        /// <param name="options"></param>
        public static void MapFluentValidationException(this Hellang.Middleware.ProblemDetails.ProblemDetailsOptions options) =>
            options.Map<ValidationException>((ctx, ex) =>
            {
                var factory = ctx.RequestServices.GetRequiredService<Hellang.Middleware.ProblemDetails.ProblemDetailsFactory>();

                var errors = ex.Errors
                    .GroupBy(x => x.PropertyName)
                    .ToDictionary(
                        x => x.Key,
                        x => x.Select(x => x.ErrorMessage).ToArray());

                return factory.CreateValidationProblemDetails(ctx, errors);
            });
    }
}
