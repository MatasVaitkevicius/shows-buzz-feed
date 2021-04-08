using Newtonsoft.Json;
using shows_buzz_feed.Helpers;
using shows_buzz_feed.Mappings.Film;
using shows_buzz_feed.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace shows_buzz_feed.Services
{
    public class FilmService
    {
        private readonly HttpClient client;
        private string baseUrl;

        public FilmService(HttpClient client)
        {
            this.client = client;
            baseUrl = "https://localhost:44347";
        }

        public async Task<FilmListViewModel> GetFilmsAsync()
        {
            try
            {
                var json = await client.GetStringAsync($"{baseUrl}/api/films/");
                return JsonConvert.DeserializeObject<FilmListViewModel>(json);
            }
            catch (Exception e)
            {
                var message = e.InnerException.Message;
                throw;
            }
        }

        public async Task<FilmViewModel> GetFilmAsync(int id)
        {
            try
            {
                var json = await client.GetStringAsync($"{baseUrl}/api/films/{id}");
                return JsonConvert.DeserializeObject<FilmViewModel>(json);
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public async Task<int> InsertFilmAsync(CreateFilmCommand command)
        {
            var result = await client.PostAsync($"{baseUrl}/api/films/", RequestHelper.GetStringContentFromObject(command));
            return Convert.ToInt32(result.Content.ReadAsStringAsync().Result);
        }

        public async Task<HttpResponseMessage> UpdateFilmAsync(UpdateFilmCommand command)
        {
            try
            {
                return await client.PutAsync($"{baseUrl}/api/films/", RequestHelper.GetStringContentFromObject(command));
            }
            catch (Exception e)
            {

                var message = e.InnerException.Message;
                throw;
            }
        }

        public async Task<HttpResponseMessage> DeleteFilmAsync(int id)
        {
            return await client.DeleteAsync($"{baseUrl}/api/films/{id}");
        }
    }
}
