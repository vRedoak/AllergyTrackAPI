namespace Domain.Entities
{
    public class NotificationCategory
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public List<NotificationType> NotificationTypes { get; set; }
    }
}
