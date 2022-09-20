namespace Application.Validations.User
{
    public class UserValidationErrorMessages
    {
        public const string EmptyEmail = "Email can not be empty.";
        public const string IncorrectEmail = "Incorrect email.";
        public const string EmptyFirstName = "First name can not be empty.";
        public const string EmptyLastName = "Last name can not be empty.";
        public const string EmptyPassword = "Password can not be empty.";
        public const string WeakPassword = "The password must contain at least one uppercase letter, one lowercase letter, and one number. Also, the password length cannot be less than 8 characters.";
        public const string NotUniqueEmail = "Not unique email.";

        public const string UserNotExist = "User not found.";
        public const string IncorrectPassword = "Incorrect password";
    }
}
