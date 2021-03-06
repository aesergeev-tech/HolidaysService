using System.Collections.Generic;
using System.Threading.Tasks;
using HolidaysService.Core;
using HolidaysService.Infrastructure.ExternalServiceProxies.PublicHolidaysServiceProxy.Contracts;

namespace HolidaysService.Infrastructure.ExternalServiceProxies.PublicHolidaysServiceProxy
{
    public interface IPublicHolidaysService
    {
        Task<Result<List<HolidayInfo>>> GetHolidays(string countryCode, int year);
    }
}