using Application.Models.Pollen;

namespace Application.Models.Notification
{
    public class EmailNotificationSendingModel
    {
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public List<PollenInfo> PollenInfo { get; set; }
    }
}
