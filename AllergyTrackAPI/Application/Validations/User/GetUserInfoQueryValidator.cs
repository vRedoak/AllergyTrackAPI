using Application.Queries.User;
using FluentValidation;
using Infrastructure.DAL;
using Microsoft.EntityFrameworkCore;

namespace Application.Validations.User
{
    public class GetUserInfoQueryValidator : AbstractValidator<GetUserInfoQuery>
    {
        private readonly IRepository<Domain.Entities.User> _userRepository;

        public GetUserInfoQueryValidator(IRepository<Domain.Entities.User> userRepository)
        {
            _userRepository = userRepository;
            
            RuleFor(x => x.UserGuid).MustAsync(IsUserExists).WithMessage(UserValidationErrorMessages.UserNotExist);
        }

        private async Task<bool> IsUserExists(Guid userGuid, CancellationToken token)
        {
            return await _userRepository.Query.AnyAsync(x => x.Guid == userGuid, token);
        }
    }
}