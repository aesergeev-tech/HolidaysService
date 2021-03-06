using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using HolidaysService.Core;
using HolidaysService.Infrastructure.ExternalServiceProxies.PublicHolidaysServiceProxy.Contracts;
using JetBrains.Annotations;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using Serilog;

namespace HolidaysService.Infrastructure.ExternalServiceProxies.PublicHolidaysServiceProxy
{
    public class PublicHolidaysServiceProxy : IPublicHolidaysService
    {
        private readonly HttpClient _httpClient;
        private readonly IDistributedCache _cacheService;
        private readonly ServiceConfiguration _serviceConfiguration;

        public PublicHolidaysServiceProxy(HttpClient httpClient, IDistributedCache cacheService, ServiceConfiguration serviceConfiguration)
        {
            _httpClient = httpClient;
            _cacheService = cacheService;
            _serviceConfiguration = serviceConfiguration;
        }
        public async Task<Result<List<HolidayInfo>>> GetHolidays(string countryCode, int year)
        {
            var cacheKey = $"{countryCode}{year}";
            var encodedCountryHolidaysInfo = await _cacheService.GetAsync(cacheKey);
            if (encodedCountryHolidaysInfo == null)
            {
                var (url, method) = BuildGetCountryHolidays(countryCode, year);
                var (isSuccess, faultMessage, value) = GetResponseAsync<List<HolidayInfoResponse>>(url, method).Result;
                switch (isSuccess)
                {
                    case false:
                        return new Result<List<HolidayInfo>>
                        {
                            Value = null,
                            FaultMessage = faultMessage,
                            IsSuccess = false
                        };
                    default:
                    {
                        var holidayInfo = value.Select(v => Map(v)).ToList();
                        encodedCountryHolidaysInfo = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(holidayInfo));
                        var options = new DistributedCacheEntryOptions()
                            .SetSlidingExpiration(TimeSpan.FromMinutes(20))
                            .SetAbsoluteExpiration(DateTime.Now.AddHours(6));
                        await _cacheService.SetAsync(cacheKey, encodedCountryHolidaysInfo, options);
                        return new Result<List<HolidayInfo>>
                        {
                            Value = holidayInfo,
                            FaultMessage = null,
                            IsSuccess = true
                        };
                    }
                }
            }
            
            return null;
        }

        private static HolidayInfo Map(HolidayInfoResponse value)
        {
            return new()
            {
                Date = value.date,
                Fixed = value.Fixed,
                Global = value.Global,
                Name = value.name,
                LocalName = value.localName,
                CountryCode = value.countryCode,
                LaunchYear = value.launchYear,
                Type = value.type,
                Countries = value.countries.ToList()
            };
        }

        private (string Url, HttpMethod Method) BuildGetCountryHolidays(string countryCode, int year)
        {
            return ($"{_serviceConfiguration.Url}/publicholidays/{year}/{countryCode}", HttpMethod.Get);
        }
        
        private async Task<(bool IsSuccess, string FaultMessage, T Value)> GetResponseAsync<T>(string url, HttpMethod method, string content = null)
            where T : class
        {
            try
            {
                var httpRequest = new HttpRequestMessage
                {
                    Method = method,
                    RequestUri = new Uri(url),
                    Content = new StringContent(content ?? string.Empty, Encoding.UTF8, "application/json")
                };

                var response = await _httpClient.SendAsync(httpRequest);
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    return (true, null, JsonConvert.DeserializeObject<T>(json));
                }

                var body = await response.Content.ReadAsStringAsync();
                return (false, body, null);
            }
            catch (Exception e)
            {
                Log.Error("An error occured during service request, {@exception}", e);
                throw;
            }
        }
        
        [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
        [SuppressMessage("ReSharper", "InconsistentNaming")]
        private record HolidayInfoResponse
        {
            public DateTime date { get; set; }
            public string localName { get; set; }
            public string name { get; set; }
            public string countryCode { get; set; }
            public bool Fixed {get; set; }
            public bool Global { get; set; }
            public List<string> countries { get; set; }
            public int launchYear { get; set; }
            public string type { get; set; }
        }
    }
}