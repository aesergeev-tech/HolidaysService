using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HolidaysService.Core;
using HolidaysService.Infrastructure.ExternalServiceProxies.PublicHolidaysServiceProxy;

namespace HolidaysService.ApplicationService
{
    public class ApplicationService
    {
        private readonly IPublicHolidaysService _holidaysService;

        public ApplicationService(IPublicHolidaysService holidaysService)
        {
            _holidaysService = holidaysService;
        }
        public async Task<string> GetCountryWithMostHolidaysThisYear()
        {
            var year = DateTime.Now.Year;
            var countryHolidaysNumberDict = new Dictionary<string, int>();
            foreach (var countryCode in Constants.CountryCodes)
            {
                var holidaysResult = await _holidaysService.GetHolidays(countryCode, year);
                if (holidaysResult.IsSuccess)
                {
                    countryHolidaysNumberDict[countryCode] = holidaysResult.Value.Count;
                }
                else
                {
                    throw new ApplicationException($"Unable to get holidays for {countryCode}");
                }
            }

            return countryHolidaysNumberDict.Aggregate((l, r) => l.Value > r.Value ? l : r).Key;
        }

        public async Task<string> GetMonthWithMostNumberOfHolidaysGlobally(int year)
        {
            throw new System.NotImplementedException();
        }

        public async Task<string> GetCountryWithTheMostUniqueHolidays(int year)
        {
            throw new System.NotImplementedException();
        }
    }
}