using Newtonsoft.Json;
using shows_buzz_feed.Helpers;
using shows_buzz_feed.Mappings.UserSeenTvShow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace shows_buzz_feed.Services
{
    public class UserSeenTvShowService
    {
        private readonly HttpClient client;
        private string baseUrl;

        public UserSeenTvShowService(HttpClient client)
        {
            this.client = client;
            baseUrl = "https://localhost:44347";
        }

        public async Task<UserSeenTvShowListViewModel> GetUserSeenTvShowAsync(int userId)
        {
            try
            {
                var json = await client.GetStringAsync($"{baseUrl}/api/UserSeenTvShows/{userId}");
                return JsonConvert.DeserializeObject<UserSeenTvShowListViewModel>(json);
            }
            catch (Exception e)
            {
                var message = e.InnerException.Message;
                throw;
            }
        }
        public async Task<int> InsertUserSeenTvShowAsync(CreateUserSeenTvShowCommand command)
        {
            try
            {
                RequestHelper.GetStringContentFromObject(command);
                var result = await client.PostAsync($"{baseUrl}/api/UserSeenTvShows/", RequestHelper.GetStringContentFromObject(command));
                return Convert.ToInt32(result.Content.ReadAsStringAsync().Result);
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public async Task<HttpResponseMessage> DeleteUserSeenTvShow(int id)
        {
            return await client.DeleteAsync($"{baseUrl}/api/UserSeenTvShows/{id}");
        }
    }
}
