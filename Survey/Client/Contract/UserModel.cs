namespace Survey.Client.Contract
{

    public class User
    {
        public string Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public UserRole Role { get; set; }

        public bool IsAdmin => Role == UserRole.Admin;
        public bool IsUser => Role == UserRole.User;
       
        public User()
        {

        }

        public User(User user)
        {
            FirstName = user.FirstName;
            LastName = user.LastName;
            Username = user.Username;
        }
    }


    public enum UserRole { User, Admin}
}

