using Newtonsoft.Json;
using shows_buzz_feed.Helpers;
using shows_buzz_feed.Mappings.UserSeenFilm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace shows_buzz_feed.Services
{
    public class UserSeenFilmService
    {
        private readonly HttpClient client;
        private string baseUrl;

        public UserSeenFilmService(HttpClient client)
        {
            this.client = client;
            baseUrl = "https://localhost:44347";
        }

        public async Task<UserSeenFilmListViewModel> GetUserSeenFilmsAsync(int userId)
        {
            try
            {
                var json = await client.GetStringAsync($"{baseUrl}/api/UserSeenFilms/{userId}");
                return JsonConvert.DeserializeObject<UserSeenFilmListViewModel>(json);
            }
            catch (Exception e)
            {
                var message = e.InnerException.Message;
                throw;
            }
        }
        public async Task<int> InsertUserSeenFilmAsync(CreateUserSeenFilmCommand command)
        {
            try
            {
                RequestHelper.GetStringContentFromObject(command);
                var result = await client.PostAsync($"{baseUrl}/api/UserSeenFilms/", RequestHelper.GetStringContentFromObject(command));
                return Convert.ToInt32(result.Content.ReadAsStringAsync().Result);
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public async Task<HttpResponseMessage> DeleteUserSeenFilm (int id)
        {
            return await client.DeleteAsync($"{baseUrl}/api/UserSeenFilms/{id}");
        }
    }
}
