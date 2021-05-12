using Newtonsoft.Json;
using shows_buzz_feed.Helpers;
using shows_buzz_feed.Mappings.Quiz;
using shows_buzz_feed.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace shows_buzz_feed.Services
{
    public class QuizService
    {
        private readonly HttpClient client;
        private string baseUrl;

        public QuizService(HttpClient client)
        {
            this.client = client;
            baseUrl = "https://localhost:44347";
        }

        public async Task<QuizListViewModel> GetQuizesAsync()
        {
            try
            {
                var json = await client.GetStringAsync($"{baseUrl}/api/quiz/");
                return JsonConvert.DeserializeObject<QuizListViewModel>(json);
            }
            catch (Exception e)
            {
                var message = e.InnerException.Message;
                throw;
            }
        }

        public async Task<QuizViewModel> GetQuizAsync(int id)
        {
            try
            {
                var json = await client.GetStringAsync($"{baseUrl}/api/quiz/{id}");
                return JsonConvert.DeserializeObject<QuizViewModel>(json);
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public async Task<int> InsertQuizAsync(CreateQuizCommand command)
        {
            var result = await client.PostAsync($"{baseUrl}/api/quiz/", RequestHelper.GetStringContentFromObject(command));
            return Convert.ToInt32(result.Content.ReadAsStringAsync().Result);
        }

        public async Task<HttpResponseMessage> UpdateQuizAsync(UpdateQuizCommand command)
        {
            try
            {
                return await client.PutAsync($"{baseUrl}/api/quiz/", RequestHelper.GetStringContentFromObject(command));
            }
            catch (Exception e)
            {

                var message = e.InnerException.Message;
                throw;
            }
        }

        public async Task<HttpResponseMessage> DeleteQuizAsync(int id)
        {
            return await client.DeleteAsync($"{baseUrl}/api/quiz/{id}");
        }
    }
}
