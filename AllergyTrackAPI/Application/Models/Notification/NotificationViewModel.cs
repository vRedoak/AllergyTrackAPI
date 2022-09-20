namespace Application.Models.Notification
{
    public class NotificationViewModel
    {
        public Guid Guid { get; set; }
        public List<NotificationScheduleViewModel> NotificationSchedules { get; set; }
        public List<NotificationTypeViewModel> NotificationTypes { get; set; }
    }
}
