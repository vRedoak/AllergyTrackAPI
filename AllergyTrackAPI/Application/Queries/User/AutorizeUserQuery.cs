using Application.Exceptions;
using Application.Models.User;
using Application.Validations.User;
using Infrastructure.DAL;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Application.Queries.User
{
    public class AuthorizeUserQuery : IRequest<string>
    {
        public AuthorizeUserQuery(AuthorizeUserRequest request)
        {
            Request = request;
        }

        public AuthorizeUserRequest Request { get; }
    }

    public class AuthorizeUserQueryHandler : IRequestHandler<AuthorizeUserQuery, string>
    {
        private readonly ILogger<AuthorizeUserQueryHandler> _logger;
        private readonly IRepository<Domain.Entities.User> _userRepository;
        private readonly IConfiguration _configuration;

        public AuthorizeUserQueryHandler(ILogger<AuthorizeUserQueryHandler> logger, IRepository<Domain.Entities.User> userRepository,
                                         IConfiguration configuration)
        {
            _userRepository = userRepository;
            _logger = logger;
            _configuration = configuration;
        }

        public async Task<string> Handle(AuthorizeUserQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Authorizing user: {request.Request.UserEmail}");

            var user = await _userRepository.Query.AsNoTracking().FirstAsync(x => x.Email == request.Request.UserEmail, cancellationToken);

            if (!IsPasswordCorrect(request.Request.Password, user.Password))
            {
                throw new UnauthorizedException(UserValidationErrorMessages.IncorrectPassword);
            }

            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Guid.ToString())
            };

            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

            var jwt = new JwtSecurityToken(
                    issuer: _configuration["JWT:ValidIssuer"],
                    audience: _configuration["JWT:ValidAudience"],
                    notBefore: DateTime.UtcNow,
                    claims: authClaims,
                    expires: DateTime.Now.AddMinutes(30),
                    signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256));

            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }

        private bool IsPasswordCorrect(string passwordToCheck, string correctPassword)
        {
            return BCrypt.Net.BCrypt.Verify(passwordToCheck, correctPassword);
        }
    }
}