using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace HolidaysService.Host.Controllers
{
    [Microsoft.AspNetCore.Components.Route("api/holidays")]
    public class HolidaysController
    {
        private readonly ApplicationService.ApplicationService _applicationService;

        public HolidaysController(ApplicationService.ApplicationService applicationService)
        {
            _applicationService = applicationService;
        }
        
        [HttpGet("most-holidays-country")]
        public async Task<string> GetCountryWithMostHolidaysThisYear()
        {
            return await _applicationService.GetCountryWithMostHolidaysThisYear();
        }

        [HttpGet("month-with-most-holidays")]
        public async Task<string> GetMonthWithMostNumberOfHolidaysGlobally(int year = 2021)
        {
            return await _applicationService.GetMonthWithMostNumberOfHolidaysGlobally(year);
        }

        [HttpGet("country-with-most-unique-holidays")]
        public async Task<string> GetCountryWithTheMostUniqueHolidays(int year = 2021)
        {
            return await _applicationService.GetCountryWithTheMostUniqueHolidays(year);
        }
    }
}