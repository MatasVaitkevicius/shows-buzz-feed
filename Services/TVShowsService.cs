using Newtonsoft.Json;
using shows_buzz_feed.Helpers;
using shows_buzz_feed.Mappings.TVShows;
using shows_buzz_feed.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace shows_buzz_feed.Services
{
    public class TVShowsService
    {
        private readonly HttpClient client;
        private string baseUrl;

        public TVShowsService(HttpClient client)
        {
            this.client = client;
            baseUrl = "https://localhost:44347";
        }

        public async Task<TVShowsListViewModel> GetTVShowsAsync()
        {
            try
            {
                var json = await client.GetStringAsync($"{baseUrl}/api/tvshows/");
                return JsonConvert.DeserializeObject<TVShowsListViewModel>(json);
            }
            catch (Exception e)
            {
                var message = e.InnerException.Message;
                throw;
            }
        }

        public async Task<TVShowsViewModel> GetTVShowsAsync(int id)
        {
            try
            {
                var json = await client.GetStringAsync($"{baseUrl}/api/tvshows/{id}");
                return JsonConvert.DeserializeObject<TVShowsViewModel>(json);
            }
            catch (Exception e)
            {
                throw;
            }
        }
    }
}
