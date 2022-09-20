using Application.Command.User;
using FluentValidation;
using Infrastructure.DAL;
using Microsoft.EntityFrameworkCore;

namespace Application.Validations.User
{
    public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
    {
        private readonly IRepository<Domain.Entities.User> _userRepository;

        public CreateUserCommandValidator(IRepository<Domain.Entities.User> userRepository)
        {
            _userRepository = userRepository;

            RuleFor(x => x.Request.Email).NotEmpty().WithMessage(UserValidationErrorMessages.EmptyEmail);
            RuleFor(x => x.Request.FirstName).NotEmpty().WithMessage(UserValidationErrorMessages.EmptyFirstName);
            RuleFor(x => x.Request.LastName).NotEmpty().WithMessage(UserValidationErrorMessages.EmptyLastName);
            RuleFor(x => x.Request.Password).NotEmpty().WithMessage(UserValidationErrorMessages.EmptyPassword);

            RuleFor(x => x.Request.Email).EmailAddress().WithMessage(UserValidationErrorMessages.IncorrectEmail);
            RuleFor(x => x.Request.Password).Matches("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9]).{8,}$").WithMessage(UserValidationErrorMessages.WeakPassword);
            RuleFor(x => x.Request.Email).MustAsync(IsEmailUnique).WithMessage(UserValidationErrorMessages.NotUniqueEmail);
        }

        private async Task<bool> IsEmailUnique(string email, CancellationToken token)
        {
            return !await _userRepository.Query.AnyAsync(x => x.Email == email, token);
        }
    }
}