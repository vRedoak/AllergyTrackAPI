namespace Application.Models.Notification
{
    public class NotificationSendingModel
    {
        public List<NotificationScheduleViewModel> NotificationSchedules { get; set; }
        public List<int> NotificationTypeIds { get; set; }
    }
}
