﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using svc_InterviewBack.DAL;

#nullable disable

namespace svc_InterviewBack.Migrations
{
    [DbContext(typeof(InterviewDbContext))]
    [Migration("20240606104856_RequestHistory")]
    partial class RequestHistory
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.6")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("svc_InterviewBack.DAL.Company", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid>("SeasonId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("SeasonId");

                    b.ToTable("Companies");
                });

            modelBuilder.Entity("svc_InterviewBack.DAL.InterviewRequest", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("PositionId")
                        .HasColumnType("uuid");

                    b.Property<Guid?>("RequestStatusSnapshotId")
                        .HasColumnType("uuid");

                    b.Property<int>("ResultStatus")
                        .HasColumnType("integer");

                    b.Property<Guid>("StudentId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("PositionId");

                    b.HasIndex("StudentId");

                    b.ToTable("InterviewRequests");
                });

            modelBuilder.Entity("svc_InterviewBack.DAL.Position", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid?>("CompanyId")
                        .HasColumnType("uuid");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<int>("NPositions")
                        .HasColumnType("integer");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("CompanyId");

                    b.ToTable("Positions");
                });

            modelBuilder.Entity("svc_InterviewBack.DAL.RequestStatusSnapshot", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("DateTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("InterviewRequestId")
                        .HasColumnType("uuid");

                    b.Property<Guid?>("PreviousRequestStatusSnapshotId")
                        .HasColumnType("uuid");

                    b.Property<int>("RequestStatus")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("InterviewRequestId")
                        .IsUnique();

                    b.HasIndex("PreviousRequestStatusSnapshotId");

                    b.ToTable("RequestStatusSnapshots");
                });

            modelBuilder.Entity("svc_InterviewBack.DAL.Season", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("SeasonEnd")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("SeasonStart")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("Year")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("Year")
                        .IsUnique();

                    b.ToTable("Seasons");
                });

            modelBuilder.Entity("svc_InterviewBack.DAL.Student", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<int>("EmploymentStatus")
                        .HasColumnType("integer");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid>("SeasonId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("SeasonId");

                    b.ToTable("Students");
                });

            modelBuilder.Entity("svc_InterviewBack.DAL.Company", b =>
                {
                    b.HasOne("svc_InterviewBack.DAL.Season", "Season")
                        .WithMany("Companies")
                        .HasForeignKey("SeasonId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Season");
                });

            modelBuilder.Entity("svc_InterviewBack.DAL.InterviewRequest", b =>
                {
                    b.HasOne("svc_InterviewBack.DAL.Position", "Position")
                        .WithMany()
                        .HasForeignKey("PositionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("svc_InterviewBack.DAL.Student", "Student")
                        .WithMany("InterviewRequests")
                        .HasForeignKey("StudentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Position");

                    b.Navigation("Student");
                });

            modelBuilder.Entity("svc_InterviewBack.DAL.Position", b =>
                {
                    b.HasOne("svc_InterviewBack.DAL.Company", null)
                        .WithMany("Positions")
                        .HasForeignKey("CompanyId");
                });

            modelBuilder.Entity("svc_InterviewBack.DAL.RequestStatusSnapshot", b =>
                {
                    b.HasOne("svc_InterviewBack.DAL.InterviewRequest", "InterviewRequest")
                        .WithOne("RequestStatusSnapshot")
                        .HasForeignKey("svc_InterviewBack.DAL.RequestStatusSnapshot", "InterviewRequestId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("svc_InterviewBack.DAL.RequestStatusSnapshot", "PreviousRequestStatusSnapshot")
                        .WithMany()
                        .HasForeignKey("PreviousRequestStatusSnapshotId");

                    b.Navigation("InterviewRequest");

                    b.Navigation("PreviousRequestStatusSnapshot");
                });

            modelBuilder.Entity("svc_InterviewBack.DAL.Student", b =>
                {
                    b.HasOne("svc_InterviewBack.DAL.Season", "Season")
                        .WithMany("Students")
                        .HasForeignKey("SeasonId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Season");
                });

            modelBuilder.Entity("svc_InterviewBack.DAL.Company", b =>
                {
                    b.Navigation("Positions");
                });

            modelBuilder.Entity("svc_InterviewBack.DAL.InterviewRequest", b =>
                {
                    b.Navigation("RequestStatusSnapshot")
                        .IsRequired();
                });

            modelBuilder.Entity("svc_InterviewBack.DAL.Season", b =>
                {
                    b.Navigation("Companies");

                    b.Navigation("Students");
                });

            modelBuilder.Entity("svc_InterviewBack.DAL.Student", b =>
                {
                    b.Navigation("InterviewRequests");
                });
#pragma warning restore 612, 618
        }
    }
}
