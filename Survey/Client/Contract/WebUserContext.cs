using Survey.Client.Services;

namespace Survey.Client.Contract
{
    public class WebUserContext : IUserContext
    {
        // TODO refactor this , temp solution to run solution...

        private readonly IUserContextService userContextService;


        public WebUserContext(IUserContextService userContextService)
        {
            this.userContextService = userContextService;
        }

        public bool IsAuthenticated => userContextService.User != null;

        public string Id => IsAuthenticated
            ? userContextService.User.Id
            : null;

        public string Username => IsAuthenticated
            ? userContextService.User.Username
            : null;

        public string DisplayName => IsAuthenticated
            ? $"{userContextService.User.FirstName} {userContextService.User.LastName}"
            : null;

        public bool IsAdmin => IsAuthenticated && userContextService.User.IsAdmin;
        public bool IsUser => IsAuthenticated && userContextService.User.IsUser;
    }
}
