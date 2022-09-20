namespace Application.Models.User
{
    public class AuthorizeUserRequest
    {
        public string UserEmail { get; set; }
        public string Password { get; set; }
    }
}
