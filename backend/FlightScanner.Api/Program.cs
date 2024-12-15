using FlightScanner.Api.Middleware;
using FlightScanner.Api.Middleware.ExceptionHandling;
using FlightScanner.Services.Api.Configuration;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

#region Configuration
builder.Configuration.AddJsonFile($"appsettings.Development.json", optional: true, reloadOnChange: true);
#endregion

#region CORS
builder.Services.AddCors
(
	options =>
	{
		options.AddPolicy
		(
			"WildcardPolicy",
			builder =>
			{
				builder
					.AllowAnyOrigin()
					.AllowAnyHeader()
					.AllowAnyMethod()
					.SetPreflightMaxAge(TimeSpan.FromHours(24));
			}
		);
	}
);
#endregion

#region Logging
Log.Logger = new LoggerConfiguration()
	.ReadFrom.Configuration(builder.Configuration)
	.CreateLogger();

builder.Host.UseSerilog();
#endregion

#region HttpClients
builder.Services.ConfigureHttpClients(builder.Configuration);
#endregion

#region Services
builder.Services.AddApiServices(builder.Configuration);
#endregion

#region Exception handling
builder.Services.AddExceptionHandler<TravelApiExceptionHandler>();
builder.Services.AddProblemDetails();
#endregion

#region Caching
builder.Services.AddDistributedMemoryCache();
#endregion

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

#region Middleware
app.UseMiddleware<RequestLoggingMiddleware>();
app.UseExceptionHandler();
#endregion

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("WildcardPolicy");
app.UseAuthorization();
app.MapControllers();
app.Run();

#region Dispose
Log.CloseAndFlush();
#endregion
