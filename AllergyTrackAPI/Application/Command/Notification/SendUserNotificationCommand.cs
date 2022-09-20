using Application.Extensions;
using Application.Helpers;
using Application.Models.Notification;
using Application.Models.Pollen;
using Application.Models.User;
using Application.Providers.Interfaces;
using AutoMapper;
using Domain.RabbitMqServices;
using Infrastructure.DAL;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Text;

namespace Application.Command.Notification
{
    public class SendUserNotificationsCommand : IRequest<Unit>
    {

        public SendUserNotificationsCommand()
        {
        }
    }

    public class SendUserNotificationsCommandHandler : IRequestHandler<SendUserNotificationsCommand, Unit>
    {
        private readonly ILogger<SendUserNotificationsCommandHandler> _logger;
        private readonly IRepository<Domain.Entities.Notification> _notificationRepository;

        private readonly ICurrentUserInfoProvider _currentUserInfoProvider;
        private readonly IMapper _mapper;

        private readonly IRabbitMQEmailSenderService _rabbitMQEmailSenderService;

        public SendUserNotificationsCommandHandler(ILogger<SendUserNotificationsCommandHandler> logger, IMapper mapper,
                                              IRepository<Domain.Entities.Notification> notificationRepository, ICurrentUserInfoProvider currentUserInfoProvider, 
                                              IRabbitMQEmailSenderService rabbitMQEmailSenderService)
        {
            _logger = logger;
            _mapper = mapper;
            _notificationRepository = notificationRepository;
            _currentUserInfoProvider = currentUserInfoProvider;
            _rabbitMQEmailSenderService = rabbitMQEmailSenderService;
        }

        public async Task<Unit> Handle(SendUserNotificationsCommand command, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Adding new notifications");

            var currentTime = DateTime.UtcNow;
            currentTime = currentTime.AddSeconds(-currentTime.Second).AddMinutes(-currentTime.Minute);

            var currentTimeUNIX = ((DateTimeOffset)currentTime).ToUnixTimeSeconds();

            var notifications= await _notificationRepository.Query.AsNoTracking()
                                            .Include(s => s.NotificationSchedules)
                                            .Include(u => u.User)
                                            .Include(tn => tn.NotificationTypeNotifications)
                                            .ThenInclude(t => t.NotificationType)
                                            .Where(n => n.NotificationSchedules.Any(ns => (currentTimeUNIX - ns.Start) % ns.Interval == 0))
                                            .ToListAsync();

            var emailNotificationsToSend = notifications.Select(x => new EmailNotificationSendingModel()
            {
                Email = x.User.Email,
                FirstName = x.User.FirstName,
                LastName = x.User.LastName,
                PollenInfo = x.NotificationTypeNotifications?.Select(n => new PollenInfo()
                {
                    TypeName = n.NotificationType.Name,
                    Count = 2,
                    RiskLevel = PollenRiskLevel.Low.GetDisplayName()
                }).ToList()
            }).ToList();

            if (emailNotificationsToSend.Count > 0)
                _rabbitMQEmailSenderService.Send(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(emailNotificationsToSend)));

            return Unit.Value;
        }
    }
}