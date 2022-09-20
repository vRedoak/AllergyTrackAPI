namespace Domain.Entities
{
    public class Notification
    {
        public Guid Guid { get; set; }
        public Guid UserGuid { get; set; }

        public User User { get; set; }
        public List<NotificationTypeNotification> NotificationTypeNotifications { get; set; }
        public List<NotificationSchedule> NotificationSchedules { get; set; }
    }
}
