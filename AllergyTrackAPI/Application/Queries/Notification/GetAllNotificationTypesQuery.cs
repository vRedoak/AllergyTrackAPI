using Application.Models.Notification;
using AutoMapper;
using Domain.Entities;
using Infrastructure.DAL;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Application.Queries.Notification
{
    public class GetAllNotificationTypesQuery : IRequest<GetAllNotificationsTypesResponse>
    {
        public GetAllNotificationTypesQuery()
        {}
    }

    public class GetAllNotificationTypesQueryHandler : IRequestHandler<GetAllNotificationTypesQuery, GetAllNotificationsTypesResponse>
    {
        private readonly ILogger<GetAllNotificationTypesQueryHandler> _logger;
        private readonly IRepository<NotificationCategory> _notificationCategoryRepository;
        private readonly IMapper _mapper;

        public GetAllNotificationTypesQueryHandler(ILogger<GetAllNotificationTypesQueryHandler> logger,
                                          IMapper mapper, IRepository<NotificationCategory> notificationCategoryRepository)
        {
            _logger = logger;
            _mapper = mapper;
            _notificationCategoryRepository = notificationCategoryRepository;
        }

        public async Task<GetAllNotificationsTypesResponse> Handle(GetAllNotificationTypesQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Getting all notification categories");

            var notificationCategories = await _notificationCategoryRepository.Query.Include(t => t.NotificationTypes).ToListAsync();

            return _mapper.Map<GetAllNotificationsTypesResponse>(notificationCategories);
        }
    }
}