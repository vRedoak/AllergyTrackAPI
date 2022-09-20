using Application.Command.Notification;
using Application.Models.Notification;
using FluentValidation;
using Infrastructure.DAL;
using Microsoft.EntityFrameworkCore;

namespace Application.Validations.Notification
{
    public class AddNotificationCommandValidator : AbstractValidator<AddNotificationCommand>
    {
        private const int MaxRepetitionIntervalInDays = 32;
        private const int MaximumNumberOfMonthsForStartFrom = 3;

        private readonly IRepository<Domain.Entities.NotificationType> _notificationTypeRepository;

        public AddNotificationCommandValidator(IRepository<Domain.Entities.NotificationType> notificationTypeRepository)
        {
            _notificationTypeRepository = notificationTypeRepository;

            RuleFor(x => x.Request.NotificationTypeIds).NotEmpty().WithMessage(NotificationValidationErrorMessages.EmptyNotificationTypeIds);
            RuleFor(x => x.Request.NotificationTypeIds).Must(x => x.Any()).WithMessage(NotificationValidationErrorMessages.EmptyNotificationTypeIds);
            RuleFor(x => x.Request.NotificationTypeIds).MustAsync(AreIdsExist).WithMessage(NotificationValidationErrorMessages.IncorrectNotificationTypeIds);

            RuleFor(x => x.Request.NotificationSchedules).NotEmpty().WithMessage(NotificationValidationErrorMessages.EmptyNotificationSchedules);
            RuleFor(x => x.Request.NotificationSchedules).Must(x => x.Any()).WithMessage(NotificationValidationErrorMessages.EmptyNotificationSchedules);
         //   RuleFor(x => x.Request.NotificationSchedules).Must(IsStartFromCorrect).WithMessage(NotificationValidationErrorMessages.IncorrectStartFromFields);
            RuleFor(x => x.Request.NotificationSchedules).Must(IsRepetitionIntervalInDaysCorrect).WithMessage(NotificationValidationErrorMessages.IncorrectRepetitionIntervalInDaysFields);

            
           
        }

        private async Task<bool> AreIdsExist(List<int> typeIds, CancellationToken token)
        {
            var allNotificationTypeIds = await _notificationTypeRepository.Query.Select(x => x.Id).ToListAsync(token);
            return typeIds is null || typeIds.All(x => allNotificationTypeIds.Contains(x));
        }

        private bool IsStartFromCorrect(List<NotificationScheduleViewModel> notificationScheduleViewModels)
        {
            return notificationScheduleViewModels is null || notificationScheduleViewModels.All(x => x.StartFrom > DateTime.UtcNow && x.StartFrom < DateTime.UtcNow.AddMonths(MaximumNumberOfMonthsForStartFrom));
        }

        private bool IsRepetitionIntervalInDaysCorrect(List<NotificationScheduleViewModel> notificationScheduleViewModels)
        {
            return notificationScheduleViewModels is null || notificationScheduleViewModels.All(x => x.RepetitionIntervalInDays > 0 && x.RepetitionIntervalInDays < MaxRepetitionIntervalInDays);
        }
    }
}