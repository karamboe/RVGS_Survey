using Survey.Shared.Models;
using System.Net.Http.Json;
using System.Text.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Survey.Client.Services
{
    public class QuestionService : IQuestionService
    {
        private readonly HttpClient httpClient;

        public QuestionService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<bool> DeleteQuestion(string id)
        {
            var response = await httpClient.DeleteAsync($"api/Question/DeleteQuestion/{id}");

            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            return false;
        }

        public async Task<bool> DeleteAlternative(string id)
        {
            var response = await httpClient.DeleteAsync($"api/Question/DeleteQuestionAlternative/{id}");

            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            return false;
        }

        public async Task<QuestionDto> GetQuestionById(string id)
        {
            try
            {
                return await httpClient.GetFromJsonAsync<QuestionDto>($"api/Question/GetQuestionById/{id}");
            }
            catch
            {
                throw;
            }
        }

        public async Task<IEnumerable<QuestionDto>> ListQuestions(string id)
        {
            return await httpClient.GetFromJsonAsync<IEnumerable<QuestionDto>>($"api/Question/ListQuestions/{id}");            
        }

        public async Task<IEnumerable<AlternativeDto>> ListAlternatives(string id)
        {
            return await httpClient.GetFromJsonAsync<IEnumerable<AlternativeDto>>($"api/Question/ListQuestionAlternatives/{id}");
        }

        public async Task<QuestionDto> SaveQuestion(QuestionDto item)
        {
            QuestionDto ret = null;
            try
            {
                var response = await httpClient.PostAsJsonAsync<QuestionDto>($"api/Question/SaveQuestion/{item}", item);

                if (response.IsSuccessStatusCode)
                {
                    ret = await response.Content.ReadFromJsonAsync<QuestionDto>();                    
                }
            }
            catch
            {
                throw;
            }
            return ret;
        }

        public async Task<bool> SaveAlternative(AlternativeDto item)
        {
            try
            {
                var response = await httpClient.PostAsJsonAsync<AlternativeDto>($"api/Question/SaveQuestionAlternative/{item}", item);

                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
            }
            catch
            {
                throw;
            }
            return false;
        }
    }
}
