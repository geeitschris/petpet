﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using petpet.Models;

namespace petpet.Migrations
{
    [DbContext(typeof(Context))]
    [Migration("20200224230655_anothermigration")]
    partial class anothermigration
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.4-servicing-10062")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("petpet.Models.Comment", b =>
                {
                    b.Property<int>("CommentId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Content");

                    b.Property<DateTime>("CreatedAt");

                    b.Property<int>("PetId");

                    b.Property<DateTime>("UpdatedAt");

                    b.Property<int>("UserId");

                    b.HasKey("CommentId");

                    b.HasIndex("PetId");

                    b.HasIndex("UserId");

                    b.ToTable("PetComments");
                });

            modelBuilder.Entity("petpet.Models.Mail", b =>
                {
                    b.Property<int>("MailId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("AuthorName");

                    b.Property<string>("Content")
                        .IsRequired();

                    b.Property<DateTime>("CreatedAt");

                    b.Property<int>("RecipientId");

                    b.Property<string>("RecipientName");

                    b.Property<string>("Subject");

                    b.Property<DateTime>("UpdatedAt");

                    b.Property<int>("UserId");

                    b.HasKey("MailId");

                    b.HasIndex("UserId");

                    b.ToTable("AllMail");
                });

            modelBuilder.Entity("petpet.Models.Pet", b =>
                {
                    b.Property<int>("PetId")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreatedAt");

                    b.Property<string>("PetBio")
                        .IsRequired()
                        .HasMaxLength(250);

                    b.Property<double>("PetExperience");

                    b.Property<int>("PetHappiness");

                    b.Property<int>("PetHunger");

                    b.Property<int>("PetLevel");

                    b.Property<string>("PetName")
                        .IsRequired();

                    b.Property<string>("PetPicture");

                    b.Property<int>("PetValue");

                    b.Property<DateTime>("UpdatedAt");

                    b.Property<int>("UserId");

                    b.Property<bool>("isAdult");

                    b.HasKey("PetId");

                    b.HasIndex("UserId")
                        .IsUnique();

                    b.ToTable("Pets");
                });

            modelBuilder.Entity("petpet.Models.User", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("Balance");

                    b.Property<DateTime>("CreatedAt");

                    b.Property<string>("Email")
                        .IsRequired();

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<string>("Password")
                        .IsRequired();

                    b.Property<DateTime>("UpdatedAt");

                    b.HasKey("UserId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("petpet.Models.Comment", b =>
                {
                    b.HasOne("petpet.Models.Pet", "Pet")
                        .WithMany("PetComments")
                        .HasForeignKey("PetId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("petpet.Models.User", "Author")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("petpet.Models.Mail", b =>
                {
                    b.HasOne("petpet.Models.User", "Author")
                        .WithMany("UserMail")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("petpet.Models.Pet", b =>
                {
                    b.HasOne("petpet.Models.User", "Creator")
                        .WithOne("Pet")
                        .HasForeignKey("petpet.Models.Pet", "UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
