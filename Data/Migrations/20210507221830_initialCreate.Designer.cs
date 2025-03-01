﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Sharing_API.Data;

namespace Sharing_API.Data.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20210507221830_initialCreate")]
    partial class initialCreate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.4")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Sharing_API.Models.AppUser", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("City")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(60)
                        .HasColumnType("nvarchar(60)");

                    b.Property<byte[]>("PasswordHash")
                        .HasColumnType("varbinary(max)");

                    b.Property<byte[]>("PasswordSalt")
                        .HasColumnType("varbinary(max)");

                    b.Property<string>("PhotoPublicId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhotoUrl")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Street")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Sharing_API.Models.Category", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("Sharing_API.Models.Comment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("JoinUserId")
                        .HasColumnType("int");

                    b.Property<string>("SharerComment")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("SharerRate")
                        .HasColumnType("int");

                    b.Property<string>("SharingComment")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("SharingId")
                        .HasColumnType("int");

                    b.Property<int>("SharingRate")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("SharingId");

                    b.ToTable("Comment");
                });

            modelBuilder.Entity("Sharing_API.Models.Interest", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("AppUserId")
                        .HasColumnType("int");

                    b.Property<int>("CategoryId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("AppUserId");

                    b.HasIndex("CategoryId");

                    b.ToTable("Interest");
                });

            modelBuilder.Entity("Sharing_API.Models.Joining", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("JoinQty")
                        .HasColumnType("int");

                    b.Property<int>("JoinUserId")
                        .HasColumnType("int");

                    b.Property<int>("SharingId")
                        .HasColumnType("int");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("SharingId");

                    b.ToTable("Joinings");
                });

            modelBuilder.Entity("Sharing_API.Models.Sharing", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("AchievedDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("AppUserId")
                        .HasColumnType("int");

                    b.Property<int>("AvailableQty")
                        .HasColumnType("int");

                    b.Property<int>("CategoryId")
                        .HasColumnType("int");

                    b.Property<DateTime>("Deadline")
                        .HasColumnType("datetime2");

                    b.Property<string>("HowToShare")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("KeepQty")
                        .HasColumnType("int");

                    b.Property<string>("PhotoPublicId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhotoUrl")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PortionDescription")
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("PortionPrice")
                        .HasColumnType("float");

                    b.Property<int>("PortionQty")
                        .HasColumnType("int");

                    b.Property<string>("ProductDescription")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("PublishedDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("SavedDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("AppUserId");

                    b.HasIndex("CategoryId");

                    b.ToTable("Sharings");
                });

            modelBuilder.Entity("Sharing_API.Models.Comment", b =>
                {
                    b.HasOne("Sharing_API.Models.Sharing", null)
                        .WithMany("Comments")
                        .HasForeignKey("SharingId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Sharing_API.Models.Interest", b =>
                {
                    b.HasOne("Sharing_API.Models.AppUser", null)
                        .WithMany("Interests")
                        .HasForeignKey("AppUserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Sharing_API.Models.Category", "Category")
                        .WithMany()
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Category");
                });

            modelBuilder.Entity("Sharing_API.Models.Joining", b =>
                {
                    b.HasOne("Sharing_API.Models.Sharing", "Sharing")
                        .WithMany()
                        .HasForeignKey("SharingId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Sharing");
                });

            modelBuilder.Entity("Sharing_API.Models.Sharing", b =>
                {
                    b.HasOne("Sharing_API.Models.AppUser", "AppUser")
                        .WithMany()
                        .HasForeignKey("AppUserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Sharing_API.Models.Category", "Category")
                        .WithMany()
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("AppUser");

                    b.Navigation("Category");
                });

            modelBuilder.Entity("Sharing_API.Models.AppUser", b =>
                {
                    b.Navigation("Interests");
                });

            modelBuilder.Entity("Sharing_API.Models.Sharing", b =>
                {
                    b.Navigation("Comments");
                });
#pragma warning restore 612, 618
        }
    }
}
