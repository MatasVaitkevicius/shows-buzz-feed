using Newtonsoft.Json;
using shows_buzz_feed.Helpers;
using shows_buzz_feed.Mappings.Answer;
using shows_buzz_feed.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace shows_buzz_feed.Services
{
    public class AnswerService
    {
        private readonly HttpClient client;
        private string baseUrl;

        public AnswerService(HttpClient client)
        {
            this.client = client;
            baseUrl = "https://localhost:44347";
        }

        public async Task<AnswerListViewModel> GetAnswersAsync()
        {
            try
            {
                var json = await client.GetStringAsync($"{baseUrl}/api/answer/");
                return JsonConvert.DeserializeObject<AnswerListViewModel>(json);
            }
            catch (Exception e)
            {
                var message = e.InnerException.Message;
                throw;
            }
        }

        public async Task<AnswerListViewModel> GetAllAnswerAsync(int id)
        {
            try
            {
                var json = await client.GetStringAsync($"{baseUrl}/api/answer/question-answer/{id}");
                return JsonConvert.DeserializeObject<AnswerListViewModel>(json);
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public async Task<int> InsertAnswerAsync(CreateAnswerCommand command)
        {
            try
            {
                var result = await client.PostAsync($"{baseUrl}/api/answer/", RequestHelper.GetStringContentFromObject(command));
                return Convert.ToInt32(result.Content.ReadAsStringAsync().Result);
            }
            catch (Exception e)
            {
                throw e.InnerException;
            }
            
        }

        public async Task<HttpResponseMessage> DeleteAnswerAsync(int id)
        {
            return await client.DeleteAsync($"{baseUrl}/api/answer/{id}");
        }
    }
}
