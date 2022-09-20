using Application.Models.User;
using AutoMapper;
using Domain.Entities;
using FluentValidation;
using Infrastructure.DAL;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Web.Http;

namespace Application.Command.User
{
    public class CreateUserCommand : IRequest<Guid>
    {
        public CreateUserRequest Request { get; }

        public CreateUserCommand(CreateUserRequest request)
        {
            Request = request;
        }
    }

    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, Guid>
    {
        private readonly ILogger<CreateUserCommandHandler> _logger;
        private readonly IRepository<Domain.Entities.User> _userRepository;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly IValidator<CreateUserCommand> _validator;

        public CreateUserCommandHandler(ILogger<CreateUserCommandHandler> logger, IRepository<Domain.Entities.User> userRepository,
                                         IConfiguration configuration, IMapper mapper, IValidator<CreateUserCommand> validator)
        {
            _userRepository = userRepository;
            _logger = logger;
            _configuration = configuration;
            _mapper = mapper;
            _validator = validator;
        }

        public async Task<Guid> Handle(CreateUserCommand command, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Creating new user");

            command.Request.Password = BCrypt.Net.BCrypt.HashPassword(command.Request.Password);

            var user = _mapper.Map<Domain.Entities.User>(command.Request);

            await _userRepository.InsertAsync(user, cancellationToken);
            await _userRepository.SaveChangesAsync(cancellationToken);

            return user.Guid;
        }
    }
}