namespace Domain.Entities
{
    public class NotificationType
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int CategoryId { get; set; }

        public NotificationCategory NotificationCategory { get; set; }
        public List<NotificationTypeNotification> NotificationTypeNotifications { get; set; }
    }
}
