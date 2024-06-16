using FluentValidation;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;

namespace FanDuel.DepthChart.API.Filters
{
    public class FluentValidationFilter : IAsyncActionFilter
    {
        private readonly IValidatorFactory _validatorFactory;

        public FluentValidationFilter(IValidatorFactory validatorFactory)
        {
            _validatorFactory = validatorFactory;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (next == null)
            {
                throw new ArgumentNullException(nameof(next));
            }

            var bodyParam = context.ActionDescriptor.Parameters
                .OfType<ControllerParameterDescriptor>()
                .SingleOrDefault(x =>
                    x.ParameterInfo.CustomAttributes.Any(attr => attr.AttributeType == typeof(FromBodyAttribute)));

            //var modelValue = bodyParam != null ? context.ActionArguments[bodyParam.Name] : null;
            //if (modelValue != null)
            //{
            //    var validator = _validatorFactory?.GetValidator(modelValue.GetType());
            //    if (validator != null)
            //    {
            //        var validationContext = new ValidationContext<object>(modelValue);
            //        validationContext.SetServiceProvider(context.HttpContext.RequestServices);

            //        var validationResult =
            //            await validator.ValidateAsync(validationContext, context.HttpContext.RequestAborted);
            //        if (!validationResult.IsValid)
            //        {
            //            validationResult.AddToModelState(context.ModelState, null);
            //        }
            //        foreach (var prop in validator.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public))
            //        {
            //            ((Controller)context.Controller).ViewData.Add(prop.Name, prop.GetValue(validator));
            //        }
            //    }
            //}

            //if (!context.ModelState.IsValid)
            //{
            //    context.Result = new BadRequestObjectResult(context.ModelState);
            //}
            //else
            //{
            //    await next();
            //}

            await next();
        }
    }
}
