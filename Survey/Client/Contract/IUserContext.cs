namespace Survey.Client.Contract
{
    public interface IUserContext
    {
        public string Id { get; }
        public string Username { get; }
        public string DisplayName { get; }
  
        public bool IsAuthenticated { get; }
        public bool IsAdmin { get; }
        public bool IsUser { get; }
    }
}
