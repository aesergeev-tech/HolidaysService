using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using HolidaysService.Core;
using HolidaysService.Infrastructure.ExternalServiceProxies.PublicHolidaysServiceProxy.Contracts;
using HolidaysService.Tests.Bootstrap;
using HolidaysService.Tests.Mocks;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace HolidaysService.Tests
{
    [TestFixture]
    public class ApplicationServiceTests
    {

        [Test]
        public async Task ShouldGetCountryWithMostHolidaysThisYear()
        {
            MockPublicHolidaysService.HolidayInfosValueToReturn = new List<HolidayInfo>()
            {
                new()
                {
                    CountryCode = "RU",
                    Date = DateTime.Parse("2021-01-01"),
                    Fixed = true,
                    Global = false,
                    Name = "Holiday1",
                    Type = "Public",
                    LaunchYear = "2021",
                    LocalName = "Holiday1",
                    Countries = null
                },
                new()
                {
                    CountryCode = "RU",
                    Date = DateTime.Parse("2021-02-01"),
                    Fixed = true,
                    Global = false,
                    Name = "Holiday2",
                    Type = "Public",
                    LaunchYear = "2021",
                    LocalName = "Holiday2",
                    Countries = null
                },
                new()
                {
                    CountryCode = "AD",
                    Date = DateTime.Parse("2021-03-01"),
                    Fixed = true,
                    Global = false,
                    Name = "Holiday3",
                    Type = "Public",
                    LaunchYear = "2021",
                    LocalName = "Holiday3",
                    Countries = null
                }
            };
            Constants.CountryCodes = new List<string> {"RU", "AD"};
            var service = CompositionRoot.ServiceProvider.GetRequiredService<ApplicationService.ApplicationService>();
            var res = await service.GetCountryWithMostHolidaysThisYear();
            res.Should().Be("RU");
        }
    }
}