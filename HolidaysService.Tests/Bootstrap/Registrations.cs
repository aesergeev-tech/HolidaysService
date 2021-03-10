using HolidaysService.Infrastructure.ExternalServiceProxies.PublicHolidaysServiceProxy;
using HolidaysService.Tests.Mocks;
using Microsoft.Extensions.DependencyInjection;

namespace HolidaysService.Tests.Bootstrap
{
    public static class Registrations
    {
        public static IServiceCollection RegisterServices(this IServiceCollection services)
        {
            services.AddSingleton<IPublicHolidaysService, MockPublicHolidaysService>();
            services.AddSingleton<ApplicationService.ApplicationService>();
            return services;
        }
    }
}