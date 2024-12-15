using FlightScanner.Common.Api.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace FlightScanner.Api.Middleware.ExceptionHandling
{
	internal sealed class TravelApiExceptionHandler : IExceptionHandler
	{
		private readonly ILogger<TravelApiExceptionHandler> _logger;

		public TravelApiExceptionHandler(ILogger<TravelApiExceptionHandler> logger)
		{
			_logger = logger;
		}

		public async ValueTask<bool> TryHandleAsync(
			HttpContext httpContext,
			Exception exception,
			CancellationToken cancellationToken)
		{
			if (exception is not TravelApiException travelApiException)
			{
				return false;
			}

			_logger.LogError(
				travelApiException,
				"Exception occurred from the travelApi: {Message}",
				travelApiException.Message);

			var problemDetails = new ProblemDetails
			{
				Status = (int)travelApiException.StatusCode,
				Title = "Bad Request",
				Detail = travelApiException.Message
			};

			httpContext.Response.StatusCode = problemDetails.Status.Value;

			await httpContext.Response
				.WriteAsJsonAsync(problemDetails, cancellationToken);

			return true;
		}
	}
}
