using Microsoft.AspNetCore.Components;
using System.ComponentModel.DataAnnotations;
using Survey.Client.Contract;

namespace Survey.Client.Services
{
    public interface IUserContextService
    {
        User User { get; }
   
        Task<User> Login(LoginModel model);
        Task Logout();
              
        Task<bool> SetUser(User user);
        Task<bool> SetUserCompany(User user, string companyId);

        Task<User> Register(User model);
        Task<IEnumerable<User>> GetAll();
        Task<User> GetById(string id);
        Task<User> Update(string id, User model);
        Task Delete(string id);

    }

    public class LoginModel
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
    }

    public class UserContextService : IUserContextService
    {
        private readonly NavigationManager navigationManager;
        private readonly IUserDataService userDataService;

        public User User { get; private set; }
                
        public UserContextService(NavigationManager navigationManager, IUserDataService userDataService)
        {
            this.navigationManager = navigationManager;
            this.userDataService = userDataService;
        }

        public async Task<User> Login(LoginModel model)
        {
            return await userDataService.Get(model.Username, model.Password);
        }

        public async Task<bool> SetUser(User user)
        {
            User = user;

            return await Task.FromResult(true);
        }

        public async Task Logout()
        {
            User = null;
           
            await Task.CompletedTask;
            navigationManager.NavigateTo("account/login");
        }

        public async Task<User> Register(User model)
        {
            return (await userDataService.Save(model));
        }

        public async Task<IEnumerable<User>> GetAll()
        {
            return await userDataService.List();
        }

        public async Task<User> GetById(string id)
        {
            return await userDataService.GetById(id);
        }

        public async Task<User> Update(string id, User model)
        {
            var ret = await userDataService.Save(model);

            // update stored user if the logged in user updated their own record
            if (id == User?.Id)
            {
                // update local storage
                User.FirstName = model.FirstName;
                User.LastName = model.LastName;
                User.Username = model.Username;
            }
            return ret;
        }

        public async Task Delete(string id)
        {
            await userDataService.Delete(id);

            // auto logout if the logged in user deleted their own record
            if (id == User?.Id)
            {
                await Logout();
            }
        }

        public Task<bool> SetUserCompany(User user, string companyId)
        {
            throw new NotImplementedException();
        }
    }
}
