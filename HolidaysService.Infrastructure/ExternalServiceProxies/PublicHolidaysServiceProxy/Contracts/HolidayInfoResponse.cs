using System;
using System.Collections.Generic;

namespace HolidaysService.Infrastructure.ExternalServiceProxies.PublicHolidaysServiceProxy.Contracts
{
    public class HolidayInfo
    {
        public DateTime Date { get; set; }
        public string LocalName { get; set; }
        public string Name { get; set; }
        public string CountryCode { get; set; }
        public bool Fixed {get; set; }
        public bool Global { get; set; }
        public List<string> Countries { get; set; }
        public string LaunchYear { get; set; }
        public string Type { get; set; }
    }
}