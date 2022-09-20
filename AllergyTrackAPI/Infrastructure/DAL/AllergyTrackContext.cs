using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.DAL
{
    public class AllergyTrackContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<NotificationCategory> NotificationCategories { get; set; }
        public DbSet<NotificationSchedule> NotificationSchedules { get; set; }
        public DbSet<NotificationType> NotificationTypes { get; set; }
        public DbSet<NotificationTypeNotification> NotificationTypeNotifications { get; set; }

        protected readonly IConfiguration Configuration;

        public AllergyTrackContext(DbContextOptions options, IConfiguration configuration) : base(options)
        {
            Configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            if (!options.IsConfigured)
            {
                options.UseSqlServer(Configuration.GetConnectionString("sqlConnectionString"));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Guid);

                entity.ToTable("Users");
            });

            modelBuilder.Entity<Notification>(entity =>
            {
                entity.HasKey(e => e.Guid);

                entity.ToTable("Notifications");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Notifications)
                    .HasForeignKey(d => d.UserGuid)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("notifications_UserGuid_FK");
            });

            modelBuilder.Entity<NotificationCategory>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.ToTable("NotificationCategories");
            });

            modelBuilder.Entity<NotificationSchedule>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.ToTable("NotificationSchedules");

                entity.HasOne(d => d.Notification)
                    .WithMany(p => p.NotificationSchedules)
                    .HasForeignKey(d => d.NotificationGuid)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("notificationSchedules_NotificationGuid_FK");
            });

            modelBuilder.Entity<NotificationType>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.ToTable("NotificationTypes");

                entity.HasOne(d => d.NotificationCategory)
                   .WithMany(p => p.NotificationTypes)
                   .HasForeignKey(d => d.CategoryId)
                   .OnDelete(DeleteBehavior.Cascade)
                   .HasConstraintName("notificationTypes_NotificationCategories_FK");
            }); 
            
            modelBuilder.Entity<NotificationTypeNotification>(entity =>
            {
                entity.HasKey(e => new { e.NotificationGuid, e.NotificationTypeId });

                entity.ToTable("NotificationTypeNotifications");

                entity.HasOne(d => d.Notification)
                   .WithMany(p => p.NotificationTypeNotifications)
                   .HasForeignKey(d => d.NotificationGuid)
                   .OnDelete(DeleteBehavior.Cascade)
                   .HasConstraintName("notificationTypeNotifications_Notifications_FK");

                entity.HasOne(d => d.NotificationType)
                 .WithMany(p => p.NotificationTypeNotifications)
                 .HasForeignKey(d => d.NotificationTypeId)
                 .OnDelete(DeleteBehavior.Cascade)
                 .HasConstraintName("notificationTypeNotifications_NotificationTypes_FK");
            });


        }
    }
}
