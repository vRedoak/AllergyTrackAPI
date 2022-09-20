using Application.Helpers;
using Application.Models.Notification;
using Application.Models.User;
using AutoMapper;
using Domain.Entities;

namespace Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<CreateUserRequest, User>();

            CreateMap<User, GetUserInfoResponse>()
                  .ForMember(dest => dest.FulName, opt => opt.MapFrom(src => src.FirstName + " " + src.LastName));

            CreateMap<List<NotificationCategory>, GetAllNotificationsTypesResponse>()
                 .ForMember(dest => dest.NotificationCategories, opt => opt.MapFrom(src => src));

            CreateMap<NotificationCategory, NotificationCategoryViewModel>()
                 .ForMember(dest => dest.NotificationCategory, opt => opt.MapFrom(src => src.Name))
                 .ForMember(dest => dest.NotificationTypes, opt => opt.MapFrom(src => src.NotificationTypes));

            CreateMap<NotificationType, NotificationTypeViewModel>()
                 .ForMember(dest => dest.NotificationType, opt => opt.MapFrom(src => src.Name))
                 .ForMember(dest => dest.NotificationId, opt => opt.MapFrom(src => src.Id));

            CreateMap<List<Notification>, GetAllUserNotificationsResponse>()
                .ForMember(dest => dest.Notifications, opt => opt.MapFrom(src => src));

                CreateMap<Notification, NotificationViewModel>()
                 .ForMember(dest => dest.NotificationSchedules, opt => opt.MapFrom(src => src.NotificationSchedules))
                 .ForMember(dest => dest.NotificationTypes, opt => opt.MapFrom(src => src.NotificationTypeNotifications));

            CreateMap<NotificationSchedule, NotificationScheduleViewModel>()
                 .ForMember(dest => dest.StartFrom, opt => opt.MapFrom(src => DateTimeOffset.FromUnixTimeSeconds(src.Start).UtcDateTime))
                 .ForMember(dest => dest.RepetitionIntervalInDays, opt => opt.MapFrom(src => DateTimeHelper.GetUNIXInNumberOfDays(src.Interval)));

            CreateMap<NotificationTypeNotification, NotificationTypeViewModel>()
                  .ForMember(dest => dest.NotificationId, opt => opt.MapFrom(src => src.NotificationTypeId))
                  .ForMember(dest => dest.NotificationType, opt => opt.MapFrom(src => src.NotificationType.Name));
        }
    }
}
