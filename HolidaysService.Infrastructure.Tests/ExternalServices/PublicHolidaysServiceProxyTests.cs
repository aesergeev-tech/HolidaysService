using System.Diagnostics;
using System.Threading.Tasks;
using DCS.Infrastructure.Tests.Bootstrap;
using FluentAssertions;
using HolidaysService.Infrastructure.ExternalServiceProxies.PublicHolidaysServiceProxy;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace HolidaysService.Infrastructure.Tests.ExternalServices
{
    [TestFixture]
    public class PublicHolidaysServiceProxyTests
    {
        [Test]
        public async Task ShouldGetHolidayInfo()
        {
            var holidaysService = GetService();
            var result = await holidaysService.GetHolidays("RU", 2021);
            result.IsSuccess.Should().BeTrue();
            result.FaultMessage.Should().BeNullOrWhiteSpace();
            result.Value.Should().NotBeNull();
        }

        [DebuggerStepThrough]
        private static IPublicHolidaysService GetService()
        {
            return CompositionRoot.ServiceProvider.GetRequiredService<IPublicHolidaysService>();
        }
    }
}