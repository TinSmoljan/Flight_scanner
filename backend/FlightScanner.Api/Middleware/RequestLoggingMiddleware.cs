using Serilog;
using System.Diagnostics;

namespace FlightScanner.Api.Middleware
{
	public class RequestLoggingMiddleware
	{
		private readonly RequestDelegate _next;

		public RequestLoggingMiddleware(RequestDelegate next)
		{
			_next = next;
		}

		public async Task InvokeAsync(HttpContext context)
		{
			var stopwatch = Stopwatch.StartNew();

			try
			{
				await _next(context);
			}
			finally
			{
				stopwatch.Stop();

				var logData = new
				{
					context.Request.Method,
					context.Request.Path,
					QueryString = context.Request.QueryString.ToString(),
					context.Response.StatusCode,
					Duration = stopwatch.ElapsedMilliseconds
				};

				Log.Information(
					"HTTP {Method} {Path}{QueryString} responded {StatusCode} in {Duration} ms",
					logData.Method, logData.Path, logData.QueryString, logData.StatusCode, logData.Duration
				);
			}
		}
	}
}
