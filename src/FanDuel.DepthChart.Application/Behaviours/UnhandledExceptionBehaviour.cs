using FanDuel.DepthChart.Application.Exceptions;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FanDuel.DepthChart.Application.Behaviours
{
    /// <summary>
	/// Exception Pipeline, All the application exception goes through this pipeline.
	/// </summary>
	/// <typeparam name="TRequest"></typeparam>
	/// <typeparam name="TResponse"></typeparam>
	public class UnhandledExceptionBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : MediatR.IRequest<TResponse>
    {
        private readonly ILogger<TRequest> _logger;

        public UnhandledExceptionBehaviour(ILogger<TRequest> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            var requestName = typeof(TRequest).Name;

            try
            {
                return await next();
            }
            catch (AppException ex)
            {
                _logger.LogError(ex, $"Application Request: Unhandled Exception for Request {requestName} {request}");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Application Request: Unhandled Exception for Request {requestName} {request}");
                throw new UnhandleException(ex.Message);
            }
        }
    }
}
