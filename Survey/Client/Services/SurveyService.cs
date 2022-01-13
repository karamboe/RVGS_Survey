using Survey.Shared;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Survey.Client.Services
{
    public class SurveyService : ISurveyService
    {
        private readonly HttpClient client;
        private readonly JsonSerializerOptions options;

        public SurveyService(HttpClient httpClient)
        {
            client = httpClient;
            options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        }

        public async Task<bool> Delete(string id)
        {
            var response = await client.DeleteAsync($"api/survey/{id}");
            
            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("Success");
                return true;
            }
            return false;
        }

        public async Task<SurveyDto> GetById(string id)
        {            
            try
            {
                return await client.GetFromJsonAsync<SurveyDto>($"api/survey/{id}");
            }
            catch
            {
                return null;
            }
        }

        public async Task<IEnumerable<SurveyDto>> List()
        {
            try
            {
                return await client.GetFromJsonAsync<SurveyDto[]>($"api/survey");
            }
            catch
            {
                return Enumerable.Empty<SurveyDto>();      
            }
        }

        public async Task<bool> Save(SurveyDto item)
        {
            try
            {                
                var response = await client.PostAsJsonAsync<SurveyDto>($"api/survey", item);

                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine("Success");
                    return true;
                }                
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return false;
        }
    }
}
