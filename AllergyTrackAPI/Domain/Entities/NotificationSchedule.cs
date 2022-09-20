namespace Domain.Entities
{
    public class NotificationSchedule
    {
        public int Id { get; set; }
        public long Start { get; set; }
        public int Interval { get; set; }
        public Guid NotificationGuid { get; set; }

        public Notification Notification { get; set; }
    }
}
