using Survey.Client.Contract;

namespace Survey.Client.Services
{
    public interface IUserDataService
    {
        Task<User> Get(string username, string password);
        Task<User> GetById(string id);

        Task<bool> Exists(User u, string username);

        Task<IEnumerable<User>> List();
        Task<IEnumerable<User>> ListByCompany(string id);

        Task<User> Save(User user);

        Task<bool> Delete(string id);
    }
}
