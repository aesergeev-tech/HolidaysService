using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HolidaysService.Core;
using HolidaysService.Infrastructure.ExternalServiceProxies.PublicHolidaysServiceProxy;
using HolidaysService.Infrastructure.ExternalServiceProxies.PublicHolidaysServiceProxy.Contracts;

namespace HolidaysService.Tests.Mocks
{
    public class MockPublicHolidaysService : IPublicHolidaysService
    {
        public static List<HolidayInfo> HolidayInfosValueToReturn = new();

        public Task<Result<List<HolidayInfo>>> GetHolidays(string countryCode, int year)
        {
            return Task.FromResult(new Result<List<HolidayInfo>>
            {
                Value = HolidayInfosValueToReturn.Where(h => h.CountryCode == countryCode).ToList(),
                FaultMessage = string.Empty,
                IsSuccess = true
            });
        }
    }
}