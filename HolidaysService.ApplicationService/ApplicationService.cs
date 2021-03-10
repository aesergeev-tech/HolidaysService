using System;
using System.Collections.Generic;
using System.Globalization;
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
            var monthNumberOfHolidaysDict = new Dictionary<int, int>();
            foreach (var countryCode in Constants.CountryCodes)
            {
                var holidaysResult = await _holidaysService.GetHolidays(countryCode, year);
                if (holidaysResult.IsSuccess)
                {
                    foreach (var date in holidaysResult.Value.Select(holidayInfo => holidayInfo.Date))
                    {
                        if (!monthNumberOfHolidaysDict.TryGetValue(date.Month, out var count))
                        {
                            var cnt = holidaysResult.Value.Count(h => h.Date.Month == date.Month);
                            monthNumberOfHolidaysDict[date.Month] = cnt;
                        }
                        else
                        {
                            count += holidaysResult.Value.Count(h => h.Date.Month == date.Month);
                            monthNumberOfHolidaysDict[date.Month] = count;    
                        }
                    }
                }
                else
                {
                    throw new ApplicationException($"Unable to get holidays for {countryCode}");
                }
            }
            var monthIndex = monthNumberOfHolidaysDict.Aggregate((l, r) => l.Value > r.Value ? l : r).Key + 1 ;
            return CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(monthIndex);
        }

        public async Task<string> GetCountryWithTheMostUniqueHolidays(int year)
        {
            var countryGlobalHolidaysCountDict = new Dictionary<string, int>();
            foreach (var countryCode in Constants.CountryCodes)
            {
                var holidaysResult = await _holidaysService.GetHolidays(countryCode, year);
                if (holidaysResult.IsSuccess)
                {
                    var globalHolidaysCount = holidaysResult.Value.Count(holidayInfo => holidayInfo.Global);
                    countryGlobalHolidaysCountDict[countryCode] = globalHolidaysCount;
                }
                else
                {
                    throw new ApplicationException($"Unable to get holidays for {countryCode}");
                }
            }
            //I've assumed that country with the smallest number of global holidays will be the country with the most unique holidays
            var monthIndex = countryGlobalHolidaysCountDict.Aggregate((l, r) => l.Value > r.Value ? r : l).Key;
            return monthIndex;
        }
    }
}