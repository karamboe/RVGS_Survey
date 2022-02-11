using Survey.Client.Contract;
using System.Net.Http.Json;

namespace Survey.Client.Services
{
    public class UserDataService : IUserDataService
    {
        private readonly HttpClient httpClient;

        public UserDataService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<bool> Delete(string id)
        {
            var response = await httpClient.DeleteAsync($"api/User/DeleteUser/{id}");

            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            return false;
        }

        public async Task<bool> Exists(User u, string username)
        {
            throw new NotImplementedException();
        }

        public async Task<User> Get(string username, string password)
        {
            User ret = null;
            try
            {
                if (username == "kbo" && password == "kenneth")
                {
                    ret = new User
                    {
                        Id = "1",
                        Username = "kbo",
                        Password = "kenneth",
                        FirstName = "Kenneth",
                        LastName = "Bøe",
                        Role = UserRole.Admin,
                    };
                }
                else if (username == "test" && password == "test")
                {
                    ret = new User
                    {
                        Id = "2",
                        Username = "test",
                        Password = "test",
                        FirstName = "Test",
                        LastName = "Test",
                        Role = UserRole.User,
                    };
                }
                return await Task.FromResult(ret);

                //return await httpClient.GetFromJsonAsync<User>($"api/User/Get/{username}/{password}");
            }
            catch
            {
                throw;
            }
        }

        public async Task<User> GetById(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<User>> List()
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<User>> ListByCompany(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<User> Save(User user)
        {
            User ret = null;
            try
            {
                var response = await httpClient.PostAsJsonAsync<User>($"api/User/SaveUser/{user}", user);

                if (response.IsSuccessStatusCode)
                {
                    ret = await response.Content.ReadFromJsonAsync<User>();
                }
            }
            catch
            {
                throw;
            }
            return ret;
        }
    }
}
