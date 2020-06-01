using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace eShopSolution.WebApp.Services.Location
{
    public class LocationService : ILocationService
    {
        private readonly IHttpClientFactory _httpClientFactor;
        private readonly IConfiguration _configuration;
        private HttpClient _client;
        public LocationService(IHttpClientFactory httpClientFactory,
            IConfiguration configuration)
        {
            _configuration = configuration;
            _httpClientFactor = httpClientFactory;
            _client = _httpClientFactor.CreateClient();
            _client.BaseAddress = new Uri("https://maps.googleapis.com");
        }
        public async Task<string> GetAll(string keyword)
        {
            var apiKey = _configuration.GetSection("Authentication:APIKEY").Value;
            var response = await _client.GetAsync($"/maps/api/place/autocomplete/json?input={keyword}&language=vi&key={apiKey}");
            using (HttpContent content = response.Content)
            {
                //convert data content to string using await
                return  await content.ReadAsStringAsync();

            }
        }
    }
}
