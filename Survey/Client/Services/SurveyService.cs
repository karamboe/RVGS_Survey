using Survey.Shared.Models;
using System.Net.Http.Json;
using System.Text.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Survey.Client.Services
{
    public class SurveyService : ISurveyService
    {
        private readonly HttpClient httpClient;

        public SurveyService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<bool> Delete(string id)
        {
            var response = await httpClient.DeleteAsync($"api/survey/{id}");

            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            return false;
        }

        public async Task<SurveyDto> GetById(string id)
        {
            try
            {
                return await httpClient.GetFromJsonAsync<SurveyDto>($"api/survey/{id}");
            }
            catch
            {
                throw;
            }
        }

        public async Task<IEnumerable<SurveyDto>> List()
        {

            return await httpClient.GetFromJsonAsync<IEnumerable<SurveyDto>>($"api/survey");
        }

        public async Task<bool> Save(SurveyDto item)
        {
            try
            {
                var response = await httpClient.PostAsJsonAsync<SurveyDto>($"api/survey", item);

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
