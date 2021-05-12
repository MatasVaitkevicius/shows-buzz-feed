using Newtonsoft.Json;
using shows_buzz_feed.Helpers;
using shows_buzz_feed.Mappings.Question;
using shows_buzz_feed.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace shows_buzz_feed.Services
{
    public class QuestionService
    {
        private readonly HttpClient client;
        private string baseUrl;

        public QuestionService(HttpClient client)
        {
            this.client = client;
            baseUrl = "https://localhost:44347";
        }

        public async Task<QuestionListViewModel> GetQuestionsAsync()
        {
            try
            {
                var json = await client.GetStringAsync($"{baseUrl}/api/question/");
                return JsonConvert.DeserializeObject<QuestionListViewModel>(json);
            }
            catch (Exception e)
            {
                var message = e.InnerException.Message;
                throw;
            }
        }

        public async Task<QuestionViewModel> GetQuestionAsync(int id)
        {
            try
            {
                var json = await client.GetStringAsync($"{baseUrl}/api/question/{id}");
                return JsonConvert.DeserializeObject<QuestionViewModel>(json);
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public async Task<int> InsertQuestionAsync(CreateQuestionCommand command)
        {
            try
            {
                var result = await client.PostAsync($"{baseUrl}/api/question/", RequestHelper.GetStringContentFromObject(command));
                return Convert.ToInt32(result.Content.ReadAsStringAsync().Result);
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public async Task<HttpResponseMessage> UpdateQuestionAsync(UpdateQuestionCommand command)
        {
            try
            {
                return await client.PutAsync($"{baseUrl}/api/question/", RequestHelper.GetStringContentFromObject(command));
            }
            catch (Exception e)
            {

                var message = e.InnerException.Message;
                throw;
            }
        }

        public async Task<HttpResponseMessage> DeleteQuestionAsync(int id)
        {
            return await client.DeleteAsync($"{baseUrl}/api/question/{id}");
        }

        public async Task<QuestionListViewModel> GetAllQuestionAsync(int id)
        {
            try
            {
                var json = await client.GetStringAsync($"{baseUrl}/api/question/quiz-question/{id}");
                return JsonConvert.DeserializeObject<QuestionListViewModel>(json);
            }
            catch (Exception e)
            {
                throw;
            }
        }
    }
}
