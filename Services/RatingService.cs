using Newtonsoft.Json;
using shows_buzz_feed.Helpers;
using shows_buzz_feed.Mappings.Rating;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace shows_buzz_feed.Services
{
    public class RatingService
    {
        private readonly HttpClient client;
        private string baseUrl;

        public RatingService(HttpClient client)
        {
            this.client = client;
            baseUrl = "https://localhost:44347";
        }

        public async Task<RatingListViewModel> GetRatingsAsync()
        {
            try
            {
                var json = await client.GetStringAsync($"{baseUrl}/api/ratings/");
                return JsonConvert.DeserializeObject<RatingListViewModel>(json);
            }
            catch (Exception e)
            {
                var message = e.InnerException.Message;
                throw;
            }
        }

        public async Task<RatingListViewModel> GetAllRatingsAsync(int id)
        {
            try
            {
                var json = await client.GetStringAsync($"{baseUrl}/api/ratings/all/{id}");
                return JsonConvert.DeserializeObject<RatingListViewModel>(json);
            }
            catch (Exception e)
            {
                var message = e.InnerException.Message;
                throw;
            }
        }

        public async Task<RatingViewModel> GetRatingAsync(int id)
        {
            try
            {
                var json = await client.GetStringAsync($"{baseUrl}/api/ratings/{id}");
                return JsonConvert.DeserializeObject<RatingViewModel>(json);
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public async Task<int> InsertRatingAsync(CreateRatingCommand command)
        {
            var result = await client.PostAsync($"{baseUrl}/api/ratings/", RequestHelper.GetStringContentFromObject(command));
            return Convert.ToInt32(result.Content.ReadAsStringAsync().Result);
        }
    }
}
