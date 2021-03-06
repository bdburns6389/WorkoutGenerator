﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using System;
using WorkoutGenerator.Data;

namespace WorkoutGenerator.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.1-rtm-125")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Name")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("RoleId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider");

                    b.Property<string>("ProviderKey");

                    b.Property<string>("ProviderDisplayName");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("LoginProvider");

                    b.Property<string>("Name");

                    b.Property<string>("Value");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("WorkoutGenerator.Models.ApplicationUser", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccessFailedCount");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Email")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<string>("SecurityStamp");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("WorkoutGenerator.Models.Exercise", b =>
                {
                    b.Property<int>("ExerciseID")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("DateCreated");

                    b.Property<string>("Description");

                    b.Property<int>("MuscleGroupID");

                    b.Property<string>("Name");

                    b.Property<string>("OwnerId");

                    b.Property<string>("UserId");

                    b.HasKey("ExerciseID");

                    b.HasIndex("MuscleGroupID");

                    b.HasIndex("UserId");

                    b.ToTable("Exercises");
                });

            modelBuilder.Entity("WorkoutGenerator.Models.ExerciseRecord", b =>
                {
                    b.Property<int>("ExerciseID");

                    b.Property<int>("RecordID");

                    b.HasKey("ExerciseID", "RecordID");

                    b.HasIndex("RecordID");

                    b.ToTable("ExerciseRecords");
                });

            modelBuilder.Entity("WorkoutGenerator.Models.ExerciseWorkout", b =>
                {
                    b.Property<int>("ExerciseID");

                    b.Property<int>("WorkoutID");

                    b.HasKey("ExerciseID", "WorkoutID");

                    b.HasIndex("WorkoutID");

                    b.ToTable("ExerciseWorkouts");
                });

            modelBuilder.Entity("WorkoutGenerator.Models.MuscleGroup", b =>
                {
                    b.Property<int>("MuscleGroupID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name");

                    b.Property<string>("OwnerId");

                    b.Property<string>("UserId");

                    b.HasKey("MuscleGroupID");

                    b.HasIndex("UserId");

                    b.ToTable("MuscleGroups");
                });

            modelBuilder.Entity("WorkoutGenerator.Models.Record", b =>
                {
                    b.Property<int>("RecordID")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("DateCreated");

                    b.Property<int?>("ExerciseID");

                    b.Property<int>("FK_ExerciseID");

                    b.Property<string>("OwnerId");

                    b.Property<string>("Reps");

                    b.Property<string>("Sets");

                    b.Property<string>("UserId");

                    b.Property<string>("Weight");

                    b.Property<int>("WorkoutID");

                    b.HasKey("RecordID");

                    b.HasIndex("ExerciseID");

                    b.HasIndex("UserId");

                    b.HasIndex("WorkoutID");

                    b.ToTable("Records");
                });

            modelBuilder.Entity("WorkoutGenerator.Models.Workout", b =>
                {
                    b.Property<int>("WorkoutID")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("DateCreated");

                    b.Property<string>("Name");

                    b.Property<string>("OwnerId");

                    b.Property<string>("UserId");

                    b.HasKey("WorkoutID");

                    b.HasIndex("UserId");

                    b.ToTable("Workouts");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("WorkoutGenerator.Models.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("WorkoutGenerator.Models.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("WorkoutGenerator.Models.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("WorkoutGenerator.Models.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("WorkoutGenerator.Models.Exercise", b =>
                {
                    b.HasOne("WorkoutGenerator.Models.MuscleGroup", "MuscleGroup")
                        .WithMany("Exercises")
                        .HasForeignKey("MuscleGroupID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("WorkoutGenerator.Models.ApplicationUser", "User")
                        .WithMany("Exercises")
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("WorkoutGenerator.Models.ExerciseRecord", b =>
                {
                    b.HasOne("WorkoutGenerator.Models.Exercise", "Exercise")
                        .WithMany("ExerciseRecords")
                        .HasForeignKey("ExerciseID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("WorkoutGenerator.Models.Record", "Record")
                        .WithMany("ExerciseRecords")
                        .HasForeignKey("RecordID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("WorkoutGenerator.Models.ExerciseWorkout", b =>
                {
                    b.HasOne("WorkoutGenerator.Models.Exercise", "Exercise")
                        .WithMany("ExerciseWorkouts")
                        .HasForeignKey("ExerciseID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("WorkoutGenerator.Models.Workout", "Workout")
                        .WithMany("ExerciseWorkouts")
                        .HasForeignKey("WorkoutID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("WorkoutGenerator.Models.MuscleGroup", b =>
                {
                    b.HasOne("WorkoutGenerator.Models.ApplicationUser", "User")
                        .WithMany("MuscleGroups")
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("WorkoutGenerator.Models.Record", b =>
                {
                    b.HasOne("WorkoutGenerator.Models.Exercise", "Exercise")
                        .WithMany()
                        .HasForeignKey("ExerciseID");

                    b.HasOne("WorkoutGenerator.Models.ApplicationUser", "User")
                        .WithMany("Records")
                        .HasForeignKey("UserId");

                    b.HasOne("WorkoutGenerator.Models.Workout", "Workout")
                        .WithMany("Records")
                        .HasForeignKey("WorkoutID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("WorkoutGenerator.Models.Workout", b =>
                {
                    b.HasOne("WorkoutGenerator.Models.ApplicationUser", "User")
                        .WithMany("Workouts")
                        .HasForeignKey("UserId");
                });
#pragma warning restore 612, 618
        }
    }
}
