﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using Synchrowise.Database;

#nullable disable

namespace Synchrowise.Database.Migrations
{
    [DbContext(typeof(SynchrowiseDbContext))]
    [Migration("20220603145417_firebase_messaging_token")]
    partial class firebase_messaging_token
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Synchrowise.Core.Models.Group", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("EndDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("GroupMemberCount")
                        .HasColumnType("integer");

                    b.Property<string>("GroupName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid>("Guid")
                        .HasColumnType("uuid");

                    b.Property<bool>("IsActive")
                        .HasColumnType("boolean");

                    b.Property<Guid>("OwnerGuid")
                        .HasColumnType("uuid");

                    b.Property<int?>("OwnerUserId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("OwnerUserId");

                    b.ToTable("Groups");
                });

            modelBuilder.Entity("Synchrowise.Core.Models.GroupFile", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("FolderPath")
                        .HasColumnType("text");

                    b.Property<Guid>("GroupGuid")
                        .HasColumnType("uuid");

                    b.Property<int?>("GroupId")
                        .HasColumnType("integer");

                    b.Property<Guid>("Guid")
                        .HasColumnType("uuid");

                    b.Property<string>("Path")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool>("isDeleted")
                        .HasColumnType("boolean");

                    b.HasKey("Id");

                    b.HasIndex("GroupId");

                    b.ToTable("GroupFiles");
                });

            modelBuilder.Entity("Synchrowise.Core.Models.NotificationSettings", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<bool>("GroupNotification")
                        .HasColumnType("boolean");

                    b.Property<Guid>("Guid")
                        .HasColumnType("uuid");

                    b.Property<bool>("MessageNotification")
                        .HasColumnType("boolean");

                    b.Property<int>("UserId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("Notifications");
                });

            modelBuilder.Entity("Synchrowise.Core.Models.User", b =>
                {
                    b.Property<int>("UserId")
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("UserId"));

                    b.Property<int>("AvatarID")
                        .HasColumnType("integer");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool>("Email_verified")
                        .HasColumnType("boolean");

                    b.Property<DateTimeOffset>("Firebase_Creation_Time")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTimeOffset>("Firebase_Last_Signin_Time")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Firebase_id_token")
                        .HasColumnType("text");

                    b.Property<string>("Firebase_uid")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid>("GroupId")
                        .HasColumnType("uuid");

                    b.Property<int?>("GroupId1")
                        .HasColumnType("integer");

                    b.Property<Guid>("Guid")
                        .HasColumnType("uuid");

                    b.Property<bool>("Is_New_user")
                        .HasColumnType("boolean");

                    b.Property<int>("PremiumType")
                        .HasColumnType("integer");

                    b.Property<string>("Signin_Method")
                        .HasColumnType("text");

                    b.Property<int>("Term_Vision")
                        .HasColumnType("integer");

                    b.Property<string>("Username")
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<bool>("isDelete")
                        .HasColumnType("boolean");

                    b.Property<bool>("isHaveGroup")
                        .HasColumnType("boolean");

                    b.HasKey("UserId");

                    b.HasIndex("AvatarID")
                        .IsUnique();

                    b.HasIndex("Firebase_uid")
                        .IsUnique();

                    b.HasIndex("GroupId1");

                    b.HasIndex("Username")
                        .IsUnique();

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Synchrowise.Core.Models.UserAvatar", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("FolderPath")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid>("Guid")
                        .HasColumnType("uuid");

                    b.Property<Guid>("OwnerGuid")
                        .HasColumnType("uuid");

                    b.Property<int>("OwnerID")
                        .HasColumnType("integer");

                    b.Property<DateTime>("UpdatedDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Url")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool>("isDeleted")
                        .HasColumnType("boolean");

                    b.HasKey("Id");

                    b.ToTable("UserAvatars");
                });

            modelBuilder.Entity("Synchrowise.Core.Models.Group", b =>
                {
                    b.HasOne("Synchrowise.Core.Models.User", "Owner")
                        .WithMany()
                        .HasForeignKey("OwnerUserId");

                    b.Navigation("Owner");
                });

            modelBuilder.Entity("Synchrowise.Core.Models.GroupFile", b =>
                {
                    b.HasOne("Synchrowise.Core.Models.Group", "Group")
                        .WithMany("GroupFiles")
                        .HasForeignKey("GroupId");

                    b.Navigation("Group");
                });

            modelBuilder.Entity("Synchrowise.Core.Models.User", b =>
                {
                    b.HasOne("Synchrowise.Core.Models.UserAvatar", "Avatar")
                        .WithOne("Owner")
                        .HasForeignKey("Synchrowise.Core.Models.User", "AvatarID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Synchrowise.Core.Models.Group", "Group")
                        .WithMany("Users")
                        .HasForeignKey("GroupId1");

                    b.HasOne("Synchrowise.Core.Models.NotificationSettings", "Notifications")
                        .WithOne("Owner")
                        .HasForeignKey("Synchrowise.Core.Models.User", "UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Avatar");

                    b.Navigation("Group");

                    b.Navigation("Notifications");
                });

            modelBuilder.Entity("Synchrowise.Core.Models.Group", b =>
                {
                    b.Navigation("GroupFiles");

                    b.Navigation("Users");
                });

            modelBuilder.Entity("Synchrowise.Core.Models.NotificationSettings", b =>
                {
                    b.Navigation("Owner");
                });

            modelBuilder.Entity("Synchrowise.Core.Models.UserAvatar", b =>
                {
                    b.Navigation("Owner");
                });
#pragma warning restore 612, 618
        }
    }
}
