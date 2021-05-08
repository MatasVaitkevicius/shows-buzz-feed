using Newtonsoft.Json;
using shows_buzz_feed.Helpers;
using shows_buzz_feed.Mappings.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace shows_buzz_feed.Services
{
    public class UserService
    {
        private readonly HttpClient client;
        private string baseUrl;

        public UserService(HttpClient client)
        {
            this.client = client;
            baseUrl = "https://localhost:44347";
        }

        public async Task<UserListViewModel> GetUsersAsync()
        {
            try
            {
                var json = await client.GetStringAsync($"{baseUrl}/api/users/");
                return JsonConvert.DeserializeObject<UserListViewModel>(json);
            }
            catch (Exception e)
            {
                var message = e.InnerException.Message;
                throw;
            }
        }

        public async Task<UserViewModel> GetUserAsync(int id)
        {
            try
            {
                var json = await client.GetStringAsync($"{baseUrl}/api/users/{id}");
                return JsonConvert.DeserializeObject<UserViewModel>(json);
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public async Task<int> InsertUserAsync(CreateUserCommand command)
        {
            var result = await client.PostAsync($"{baseUrl}/api/users/", RequestHelper.GetStringContentFromObject(command));
            return Convert.ToInt32(result.Content.ReadAsStringAsync().Result);
        }
    }
}
