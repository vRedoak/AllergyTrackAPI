using Application.Models.User;
using Application.Providers.Interfaces;
using AutoMapper;
using Domain.Entities;
using Infrastructure.DAL;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Queries.User
{
    public class GetUserInfoQuery : IRequest<GetUserInfoResponse>
    {
        public GetUserInfoQuery(Guid userGuid)
        {
            UserGuid = userGuid;
        }

        public Guid UserGuid { get; }
    }

    public class GetUserInfoQueryHandler : IRequestHandler<GetUserInfoQuery, GetUserInfoResponse>
    {
        private readonly ILogger<GetUserInfoQueryHandler> _logger;
        private readonly IRepository<Domain.Entities.User> _userRepository;
        private readonly ICurrentUserInfoProvider _currentUserInfoProvider;
        private readonly IMapper _mapper;

        public GetUserInfoQueryHandler(ILogger<GetUserInfoQueryHandler> logger, IRepository<Domain.Entities.User> userRepository,
                                       ICurrentUserInfoProvider currentUserInfoProvider, IMapper mapper)
        {
            _userRepository = userRepository;
            _logger = logger;
            _currentUserInfoProvider = currentUserInfoProvider;
            _mapper = mapper;
        }

        public async Task<GetUserInfoResponse> Handle(GetUserInfoQuery request, CancellationToken cancellationToken)
        {
            var userGuid = _currentUserInfoProvider.GetUserInfo();

            var user = _userRepository.Query.First(x => x.Guid == userGuid);

            return _mapper.Map<GetUserInfoResponse>(user);
        }
    }
}