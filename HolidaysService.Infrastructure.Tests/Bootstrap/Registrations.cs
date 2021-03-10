using System.Net.Http;
using HolidaysService.Infrastructure.ExternalServiceProxies.PublicHolidaysServiceProxy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace HolidaysService.Infrastructure.Tests.Bootstrap
{
    public static class Registrations
    {
        public static IServiceCollection RegisterServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<ServiceConfiguration>(serviceConfiguration =>
            {
                serviceConfiguration.Url = configuration.GetSection("ConnectedServices:PublicHolidaysService:Url").Value;
            });
            services.AddSingleton(isp => isp.GetRequiredService<IOptions<ServiceConfiguration>>().Value);
            services.AddTransient<IPublicHolidaysService, PublicHolidaysServiceProxy>();
            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = "localhost:6379";
            });
            services.AddSingleton<HttpClient>();
            return services;
        }
    }
}