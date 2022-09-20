﻿// <auto-generated />
using System;
using Infrastructure.DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Infrastructure.Migrations
{
    [DbContext(typeof(AllergyTrackContext))]
    partial class AllergyTrackContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.7")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("Domain.Entities.Notification", b =>
                {
                    b.Property<Guid>("Guid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("UserGuid")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Guid");

                    b.HasIndex("UserGuid");

                    b.ToTable("Notifications", (string)null);
                });

            modelBuilder.Entity("Domain.Entities.NotificationCategory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("NotificationCategories", (string)null);
                });

            modelBuilder.Entity("Domain.Entities.NotificationSchedule", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("Interval")
                        .HasColumnType("int");

                    b.Property<Guid>("NotificationGuid")
                        .HasColumnType("uniqueidentifier");

                    b.Property<long>("Start")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("NotificationGuid");

                    b.ToTable("NotificationSchedules", (string)null);
                });

            modelBuilder.Entity("Domain.Entities.NotificationType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("CategoryId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.ToTable("NotificationTypes", (string)null);
                });

            modelBuilder.Entity("Domain.Entities.NotificationTypeNotification", b =>
                {
                    b.Property<Guid>("NotificationGuid")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("NotificationTypeId")
                        .HasColumnType("int");

                    b.HasKey("NotificationGuid", "NotificationTypeId");

                    b.HasIndex("NotificationTypeId");

                    b.ToTable("NotificationTypeNotifications", (string)null);
                });

            modelBuilder.Entity("Domain.Entities.User", b =>
                {
                    b.Property<Guid>("Guid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Guid");

                    b.ToTable("Users", (string)null);
                });

            modelBuilder.Entity("Domain.Entities.Notification", b =>
                {
                    b.HasOne("Domain.Entities.User", "User")
                        .WithMany("Notifications")
                        .HasForeignKey("UserGuid")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("notifications_UserGuid_FK");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Domain.Entities.NotificationSchedule", b =>
                {
                    b.HasOne("Domain.Entities.Notification", "Notification")
                        .WithMany("NotificationSchedules")
                        .HasForeignKey("NotificationGuid")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("notificationSchedules_NotificationGuid_FK");

                    b.Navigation("Notification");
                });

            modelBuilder.Entity("Domain.Entities.NotificationType", b =>
                {
                    b.HasOne("Domain.Entities.NotificationCategory", "NotificationCategory")
                        .WithMany("NotificationTypes")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("notificationTypes_NotificationCategories_FK");

                    b.Navigation("NotificationCategory");
                });

            modelBuilder.Entity("Domain.Entities.NotificationTypeNotification", b =>
                {
                    b.HasOne("Domain.Entities.Notification", "Notification")
                        .WithMany("NotificationTypeNotifications")
                        .HasForeignKey("NotificationGuid")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("notificationTypeNotifications_Notifications_FK");

                    b.HasOne("Domain.Entities.NotificationType", "NotificationType")
                        .WithMany("NotificationTypeNotifications")
                        .HasForeignKey("NotificationTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("notificationTypeNotifications_NotificationTypes_FK");

                    b.Navigation("Notification");

                    b.Navigation("NotificationType");
                });

            modelBuilder.Entity("Domain.Entities.Notification", b =>
                {
                    b.Navigation("NotificationSchedules");

                    b.Navigation("NotificationTypeNotifications");
                });

            modelBuilder.Entity("Domain.Entities.NotificationCategory", b =>
                {
                    b.Navigation("NotificationTypes");
                });

            modelBuilder.Entity("Domain.Entities.NotificationType", b =>
                {
                    b.Navigation("NotificationTypeNotifications");
                });

            modelBuilder.Entity("Domain.Entities.User", b =>
                {
                    b.Navigation("Notifications");
                });
#pragma warning restore 612, 618
        }
    }
}
