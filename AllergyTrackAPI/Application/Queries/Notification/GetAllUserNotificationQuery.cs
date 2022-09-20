using Application.Models.Notification;
using Application.Providers.Interfaces;
using AutoMapper;
using Infrastructure.DAL;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Application.Queries.Notification
{
    public class GetAllUserNotificationQuery : IRequest<GetAllUserNotificationsResponse>
    {
        public GetAllUserNotificationQuery()
        {}
    }

    public class GetAllUserNotificationQueryHandler : IRequestHandler<GetAllUserNotificationQuery, GetAllUserNotificationsResponse>
    {
        private readonly ILogger<GetAllUserNotificationQueryHandler> _logger;
        private readonly IRepository<Domain.Entities.Notification> _notificationRepository;
        private readonly IMapper _mapper;
        private readonly ICurrentUserInfoProvider _currentUserInfoProvider;

        public GetAllUserNotificationQueryHandler(ILogger<GetAllUserNotificationQueryHandler> logger,
                                          IMapper mapper, ICurrentUserInfoProvider currentUserInfoProvider, IRepository<Domain.Entities.Notification> notificationRepository)
        {
            _logger = logger;
            _mapper = mapper;
            _currentUserInfoProvider = currentUserInfoProvider;
            _notificationRepository = notificationRepository;
        }

        public async Task<GetAllUserNotificationsResponse> Handle(GetAllUserNotificationQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Getting all user notifications");

            var userGuid = _currentUserInfoProvider.GetUserInfo();

            var notifications = await _notificationRepository.Query.AsNoTracking()
                                                                            .Include(t => t.NotificationTypeNotifications)
                                                                            .ThenInclude(t => t.NotificationType)
                                                                            .Include(s => s.NotificationSchedules)
                                                                            .Where(x => x.UserGuid == userGuid)
                                                                            .ToListAsync(cancellationToken);

            return _mapper.Map<GetAllUserNotificationsResponse>(notifications);
        }
    }
}