namespace Domain.Entities
{
    public class NotificationTypeNotification
    {
        public Guid NotificationGuid { get; set; }
        public int NotificationTypeId { get; set; }

        public NotificationType NotificationType { get; set; }
        public Notification Notification { get; set; }
    }
}
