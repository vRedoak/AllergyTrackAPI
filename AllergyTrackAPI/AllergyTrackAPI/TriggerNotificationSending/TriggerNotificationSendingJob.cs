using Application.Command.Notification;
using MediatR;
using Quartz;

namespace AllergyTrackAPI.TriggerNotificationSending
{
    [DisallowConcurrentExecution]
    public class TriggerNotificationSendingJob : IJob
    {
        private ILogger<TriggerNotificationSendingJob> _logger;
        private readonly IMediator _mediator;

        public TriggerNotificationSendingJob(ILogger<TriggerNotificationSendingJob> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            _logger.LogInformation("Started sending notifications to the use");

            await _mediator.Send(new SendUserNotificationsCommand());

            
        }
    }
}

