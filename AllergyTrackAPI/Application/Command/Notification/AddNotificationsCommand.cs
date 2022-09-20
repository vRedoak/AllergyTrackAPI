using Application.Helpers;
using Application.Models.Notification;
using Application.Models.User;
using Application.Providers.Interfaces;
using AutoMapper;
using Infrastructure.DAL;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Command.Notification
{
    public class AddNotificationCommand : IRequest<Unit>
    {
        public NotificationSendingModel Request { get; }

        public AddNotificationCommand(NotificationSendingModel request)
        {
            Request = request;
        }
    }

    public class AddNotificationCommandHandler : IRequestHandler<AddNotificationCommand, Unit>
    {
        private readonly ILogger<AddNotificationCommandHandler> _logger;
        private readonly IRepository<Domain.Entities.Notification> _notificationRepository;
        private readonly ICurrentUserInfoProvider _currentUserInfoProvider;
        private readonly IMapper _mapper;

        public AddNotificationCommandHandler(ILogger<AddNotificationCommandHandler> logger, IMapper mapper,
                                              IRepository<Domain.Entities.Notification> notificationRepository, ICurrentUserInfoProvider currentUserInfoProvider)
        {
            _logger = logger;
            _mapper = mapper;
            _notificationRepository = notificationRepository;
            _currentUserInfoProvider = currentUserInfoProvider;
        }

        public async Task<Unit> Handle(AddNotificationCommand command, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Adding new notifications");

            var user = _currentUserInfoProvider.GetUserInfo();

            var notification = new Domain.Entities.Notification
            {
                UserGuid = user,
                NotificationSchedules = command.Request.NotificationSchedules.Select(x =>
                   new Domain.Entities.NotificationSchedule()
                   {
                       Start = ((DateTimeOffset)x.StartFrom).ToUnixTimeSeconds(),
                       Interval = DateTimeHelper.GetNumberOfDaysInUNIX(x.RepetitionIntervalInDays),
                   }).ToList(),
                NotificationTypeNotifications = command.Request.NotificationTypeIds.Distinct().Select(x =>
                    new Domain.Entities.NotificationTypeNotification()
                    {
                        NotificationTypeId = x
                    }).ToList()
            };

            await _notificationRepository.InsertAsync(notification, cancellationToken);
            await _notificationRepository.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}