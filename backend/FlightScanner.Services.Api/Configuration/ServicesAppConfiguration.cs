using FlightScanner.Services.Api.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FlightScanner.Services.Api.Configuration
{
    public static class ServicesAppConfiguration
    {
        public static IServiceCollection AddApiServices(this IServiceCollection services, IConfiguration config)
        {
			services.AddSingleton<ICacheService, CacheService>();
			services.AddTransient<IFlightService, FlightService>();
			services.AddTransient<IAirportService, AirportService>();
			services.AddTransient<IFlightHttpService, FlightHttpService>();
			services.AddTransient<IAirportHttpService, AirportHttpService>();

			return services;
        }

        public static void ConfigureHttpClients(this IServiceCollection services, IConfiguration config)
        {
            services.AddHttpClient("TravelAPI", client =>
            {
                client.BaseAddress = new Uri(config["AmadeusAPI:BaseUrl"]);
            });

            services.AddHttpClient("Wikipedia", client =>
			{
				client.BaseAddress = new Uri(config["Wikipedia:BaseUrl"]);
			});
		}
    }
}
