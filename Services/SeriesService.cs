using Newtonsoft.Json;
using shows_buzz_feed.Helpers;
using shows_buzz_feed.Mappings.Series;
using shows_buzz_feed.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace shows_buzz_feed.Services
{
    public class SeriesService
    {
        private readonly HttpClient client;
        private string baseUrl;

        public SeriesService(HttpClient client)
        {
            this.client = client;
            baseUrl = "https://localhost:44347";
        }

        public async Task<SeriesListViewModel> GetSeriesAsync()
        {
            try
            {
                var json = await client.GetStringAsync($"{baseUrl}/api/series/");
                return JsonConvert.DeserializeObject<SeriesListViewModel>(json);
            }
            catch (Exception e)
            {
                var message = e.InnerException.Message;
                throw;
            }
        }

        public async Task<SeriesViewModel> GetSeriesAsync(int id)
        {
            try
            {
                var json = await client.GetStringAsync($"{baseUrl}/api/series/{id}");
                return JsonConvert.DeserializeObject<SeriesViewModel>(json);
            }
            catch (Exception e)
            {
                throw;
            }
        }
    }
}
