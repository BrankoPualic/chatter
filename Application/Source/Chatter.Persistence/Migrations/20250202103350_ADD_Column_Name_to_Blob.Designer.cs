﻿// <auto-generated />
using System;
using Chatter.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Chatter.Persistence.Migrations
{
    [DbContext(typeof(DatabaseContext))]
    [Migration("20250202103350_ADD_Column_Name_to_Blob")]
    partial class ADD_Column_Name_to_Blob
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("dbo")
                .HasAnnotation("ProductVersion", "9.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Chatter.Domain.Models.Application.Blob", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("CreatedBy")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<bool?>("IsActive")
                        .HasColumnType("bit");

                    b.Property<Guid>("LastChangedBy")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("LastChangedOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PublicId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<long?>("Size")
                        .HasColumnType("bigint");

                    b.Property<string>("Type")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("TypeId")
                        .HasColumnType("int");

                    b.Property<string>("Url")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Blob", "dbo");
                });

            modelBuilder.Entity("Chatter.Domain.Models.Application.Messaging.Attachment", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("BlobId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("CreatedBy")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("LastChangedBy")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("LastChangedOn")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("MessageId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int?>("Order")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("BlobId");

                    b.HasIndex("MessageId");

                    b.ToTable("Attachment", "dbo");
                });

            modelBuilder.Entity("Chatter.Domain.Models.Application.Messaging.Chat", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("CreatedBy")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<Guid?>("GroupImageId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("GroupName")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<bool>("IsGroup")
                        .HasColumnType("bit");

                    b.Property<Guid>("LastChangedBy")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("LastChangedOn")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("LastMessageOn")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("GroupImageId");

                    b.ToTable("Chat", "dbo");
                });

            modelBuilder.Entity("Chatter.Domain.Models.Application.Messaging.ChatMember", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("ChatId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("CreatedBy")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsMuted")
                        .HasColumnType("bit");

                    b.Property<Guid>("LastChangedBy")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("LastChangedOn")
                        .HasColumnType("datetime2");

                    b.Property<Guid?>("LastReadMessageId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int?>("RoleId")
                        .HasColumnType("int");

                    b.Property<Guid?>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("ChatId");

                    b.HasIndex("LastReadMessageId");

                    b.HasIndex("UserId");

                    b.ToTable("ChatMember", "dbo");
                });

            modelBuilder.Entity("Chatter.Domain.Models.Application.Messaging.Message", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("ChatId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Content")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("CreatedBy")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("LastChangedBy")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("LastChangedOn")
                        .HasColumnType("datetime2");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<int>("TypeId")
                        .HasColumnType("int");

                    b.Property<Guid?>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("ChatId");

                    b.HasIndex("UserId");

                    b.ToTable("Message", "dbo");
                });

            modelBuilder.Entity("Chatter.Domain.Models.Application.SignalR.Connection", b =>
                {
                    b.Property<string>("ConnectionId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<Guid?>("GroupChatId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("ConnectionId");

                    b.HasIndex("GroupChatId");

                    b.ToTable("Connection", "dbo");
                });

            modelBuilder.Entity("Chatter.Domain.Models.Application.SignalR.Group", b =>
                {
                    b.Property<Guid>("ChatId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("ChatId");

                    b.ToTable("Group", "dbo");
                });

            modelBuilder.Entity("Chatter.Domain.Models.Application.Users.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("CreatedBy")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(80)
                        .HasColumnType("nvarchar(80)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<string>("FullName")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasMaxLength(70)
                        .HasColumnType("nvarchar(70)")
                        .HasComputedColumnSql("[FirstName] + ' ' + [LastName]");

                    b.Property<int?>("GenderId")
                        .HasColumnType("int");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<bool>("IsLocked")
                        .HasColumnType("bit");

                    b.Property<bool>("IsPrivate")
                        .HasColumnType("bit");

                    b.Property<Guid>("LastChangedBy")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("LastChangedOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.Property<string>("Password")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.HasKey("Id");

                    b.ToTable("User", "dbo");
                });

            modelBuilder.Entity("Chatter.Domain.Models.Application.Users.UserBlob", b =>
                {
                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("BlobId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("TypeId")
                        .HasColumnType("int");

                    b.HasKey("UserId", "BlobId");

                    b.HasIndex("BlobId");

                    b.ToTable("UserBlob", "dbo");
                });

            modelBuilder.Entity("Chatter.Domain.Models.Application.Users.UserFollow", b =>
                {
                    b.Property<Guid>("FollowerId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("FollowingId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("FollowDate")
                        .HasColumnType("datetime2");

                    b.HasKey("FollowerId", "FollowingId");

                    b.HasIndex("FollowingId");

                    b.ToTable("UserFollow", "dbo");
                });

            modelBuilder.Entity("Chatter.Domain.Models.Application.Users.UserLoginLog", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("UserLoginLog", "dbo");
                });

            modelBuilder.Entity("Chatter.Domain.Models.Application.Users.UserRole", b =>
                {
                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("RoleId")
                        .HasColumnType("int");

                    b.HasKey("UserId", "RoleId");

                    b.ToTable("UserRole", "dbo");
                });

            modelBuilder.Entity("Chatter.Domain.Models.Application.Messaging.Attachment", b =>
                {
                    b.HasOne("Chatter.Domain.Models.Application.Blob", "Blob")
                        .WithMany()
                        .HasForeignKey("BlobId");

                    b.HasOne("Chatter.Domain.Models.Application.Messaging.Message", "Message")
                        .WithMany("Attachments")
                        .HasForeignKey("MessageId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Blob");

                    b.Navigation("Message");
                });

            modelBuilder.Entity("Chatter.Domain.Models.Application.Messaging.Chat", b =>
                {
                    b.HasOne("Chatter.Domain.Models.Application.Blob", "GroupImage")
                        .WithMany()
                        .HasForeignKey("GroupImageId");

                    b.Navigation("GroupImage");
                });

            modelBuilder.Entity("Chatter.Domain.Models.Application.Messaging.ChatMember", b =>
                {
                    b.HasOne("Chatter.Domain.Models.Application.Messaging.Chat", "Chat")
                        .WithMany("Members")
                        .HasForeignKey("ChatId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Chatter.Domain.Models.Application.Messaging.Message", "LastReadMessage")
                        .WithMany()
                        .HasForeignKey("LastReadMessageId");

                    b.HasOne("Chatter.Domain.Models.Application.Users.User", "User")
                        .WithMany("ChatParticipations")
                        .HasForeignKey("UserId");

                    b.Navigation("Chat");

                    b.Navigation("LastReadMessage");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Chatter.Domain.Models.Application.Messaging.Message", b =>
                {
                    b.HasOne("Chatter.Domain.Models.Application.Messaging.Chat", "Chat")
                        .WithMany("Messages")
                        .HasForeignKey("ChatId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Chatter.Domain.Models.Application.Users.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId");

                    b.Navigation("Chat");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Chatter.Domain.Models.Application.SignalR.Connection", b =>
                {
                    b.HasOne("Chatter.Domain.Models.Application.SignalR.Group", null)
                        .WithMany("Connections")
                        .HasForeignKey("GroupChatId");
                });

            modelBuilder.Entity("Chatter.Domain.Models.Application.Users.UserBlob", b =>
                {
                    b.HasOne("Chatter.Domain.Models.Application.Blob", "Blob")
                        .WithMany()
                        .HasForeignKey("BlobId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Chatter.Domain.Models.Application.Users.User", "User")
                        .WithMany("Blobs")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Blob");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Chatter.Domain.Models.Application.Users.UserFollow", b =>
                {
                    b.HasOne("Chatter.Domain.Models.Application.Users.User", "Follower")
                        .WithMany("Following")
                        .HasForeignKey("FollowerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Chatter.Domain.Models.Application.Users.User", "Following")
                        .WithMany("Followers")
                        .HasForeignKey("FollowingId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Follower");

                    b.Navigation("Following");
                });

            modelBuilder.Entity("Chatter.Domain.Models.Application.Users.UserLoginLog", b =>
                {
                    b.HasOne("Chatter.Domain.Models.Application.Users.User", "User")
                        .WithMany("Logins")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("Chatter.Domain.Models.Application.Users.UserRole", b =>
                {
                    b.HasOne("Chatter.Domain.Models.Application.Users.User", "User")
                        .WithMany("Roles")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("Chatter.Domain.Models.Application.Messaging.Chat", b =>
                {
                    b.Navigation("Members");

                    b.Navigation("Messages");
                });

            modelBuilder.Entity("Chatter.Domain.Models.Application.Messaging.Message", b =>
                {
                    b.Navigation("Attachments");
                });

            modelBuilder.Entity("Chatter.Domain.Models.Application.SignalR.Group", b =>
                {
                    b.Navigation("Connections");
                });

            modelBuilder.Entity("Chatter.Domain.Models.Application.Users.User", b =>
                {
                    b.Navigation("Blobs");

                    b.Navigation("ChatParticipations");

                    b.Navigation("Followers");

                    b.Navigation("Following");

                    b.Navigation("Logins");

                    b.Navigation("Roles");
                });
#pragma warning restore 612, 618
        }
    }
}
