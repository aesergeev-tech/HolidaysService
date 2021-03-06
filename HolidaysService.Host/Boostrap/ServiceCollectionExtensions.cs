using System.Net.Http;
using HolidaysService.Infrastructure.ExternalServiceProxies.PublicHolidaysServiceProxy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace HolidaysService.Host.Boostrap
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection ConfigureExternalServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<ServiceConfiguration>(configuration.GetSection("ConnectedServices:PublicHolidaysService"));
            services.AddSingleton(isp => isp.GetRequiredService<IOptions<ServiceConfiguration>>().Value);
            services.AddSingleton<IPublicHolidaysService, PublicHolidaysServiceProxy>();
            
            services.AddSingleton<HttpClient>();
            return services;
        }

        // public static IServiceCollection ConfigureServices(this IServiceCollection services)
        // {
        //     services.AddSingleton<IataCodeValidator>();
        //     services.AddSingleton<DistanceCalculationService>();
        //     return services;
        // }
    }
}