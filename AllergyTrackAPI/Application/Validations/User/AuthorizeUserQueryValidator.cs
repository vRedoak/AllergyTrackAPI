using Application.Queries.User;
using FluentValidation;
using Infrastructure.DAL;
using Microsoft.EntityFrameworkCore;

namespace Application.Validations.User
{
    public class AuthorizeUserQueryValidator : AbstractValidator<AuthorizeUserQuery>
    {
        private readonly IRepository<Domain.Entities.User> _userRepository;

        public AuthorizeUserQueryValidator(IRepository<Domain.Entities.User> userRepository)
        {
            _userRepository = userRepository;
            
            RuleFor(x => x.Request.UserEmail).MustAsync(IsUserExists).WithMessage(UserValidationErrorMessages.UserNotExist);
        }

        private async Task<bool> IsUserExists(string userEmail, CancellationToken token)
        {
            return await _userRepository.Query.AnyAsync(x => x.Email == userEmail, token);
        }
    }
}