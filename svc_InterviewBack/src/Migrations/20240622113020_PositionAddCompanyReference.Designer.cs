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
    [Migration("20240622113020_PositionAddCompanyReference")]
    partial class PositionAddCompanyReference
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.6")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("RequestStatusTemplateSeason", b =>
                {
                    b.Property<Guid>("RequestStatusTemplatesId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("SeasonsId")
                        .HasColumnType("uuid");

                    b.HasKey("RequestStatusTemplatesId", "SeasonsId");

                    b.HasIndex("SeasonsId");

                    b.ToTable("RequestStatusTemplateSeason");
                });

            modelBuilder.Entity("svc_InterviewBack.DAL.Company", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid");

                    b.Property<Guid>("SeasonId")
                        .HasColumnType("uuid");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id", "SeasonId");

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

                    b.Property<Guid>("StudentId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("StudentSeasonId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("PositionId");

                    b.HasIndex("StudentId", "StudentSeasonId");

                    b.ToTable("InterviewRequests");
                });

            modelBuilder.Entity("svc_InterviewBack.DAL.Position", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("CompanyId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("CompanySeasonId")
                        .HasColumnType("uuid");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<int>("NSeats")
                        .HasColumnType("integer");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("CompanyId", "CompanySeasonId");

                    b.ToTable("Positions");
                });

            modelBuilder.Entity("svc_InterviewBack.DAL.RequestResult", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<bool>("OfferGiven")
                        .HasColumnType("boolean");

                    b.Property<int>("ResultStatus")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("RequestResult");
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

                    b.Property<Guid>("RequestStatusTemplateId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("InterviewRequestId");

                    b.HasIndex("RequestStatusTemplateId");

                    b.ToTable("RequestStatusSnapshots");
                });

            modelBuilder.Entity("svc_InterviewBack.DAL.RequestStatusTemplate", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("RequestStatusTemplates");
                });

            modelBuilder.Entity("svc_InterviewBack.DAL.Season", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<bool>("IsClosed")
                        .HasColumnType("boolean");

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
                        .HasColumnType("uuid");

                    b.Property<Guid>("SeasonId")
                        .HasColumnType("uuid");

                    b.Property<Guid?>("CompanyId")
                        .HasColumnType("uuid");

                    b.Property<Guid?>("CompanySeasonId")
                        .HasColumnType("uuid");

                    b.Property<int>("EmploymentStatus")
                        .HasColumnType("integer");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id", "SeasonId");

                    b.HasIndex("SeasonId");

                    b.HasIndex("CompanyId", "CompanySeasonId");

                    b.ToTable("Students");
                });

            modelBuilder.Entity("RequestStatusTemplateSeason", b =>
                {
                    b.HasOne("svc_InterviewBack.DAL.RequestStatusTemplate", null)
                        .WithMany()
                        .HasForeignKey("RequestStatusTemplatesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("svc_InterviewBack.DAL.Season", null)
                        .WithMany()
                        .HasForeignKey("SeasonsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
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
                        .HasForeignKey("StudentId", "StudentSeasonId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Position");

                    b.Navigation("Student");
                });

            modelBuilder.Entity("svc_InterviewBack.DAL.Position", b =>
                {
                    b.HasOne("svc_InterviewBack.DAL.Company", "Company")
                        .WithMany("Positions")
                        .HasForeignKey("CompanyId", "CompanySeasonId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Company");
                });

            modelBuilder.Entity("svc_InterviewBack.DAL.RequestResult", b =>
                {
                    b.HasOne("svc_InterviewBack.DAL.InterviewRequest", null)
                        .WithOne("RequestResult")
                        .HasForeignKey("svc_InterviewBack.DAL.RequestResult", "Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("svc_InterviewBack.DAL.RequestStatusSnapshot", b =>
                {
                    b.HasOne("svc_InterviewBack.DAL.InterviewRequest", "InterviewRequest")
                        .WithMany("RequestStatusSnapshots")
                        .HasForeignKey("InterviewRequestId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("svc_InterviewBack.DAL.RequestStatusTemplate", "RequestStatusTemplate")
                        .WithMany()
                        .HasForeignKey("RequestStatusTemplateId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("InterviewRequest");

                    b.Navigation("RequestStatusTemplate");
                });

            modelBuilder.Entity("svc_InterviewBack.DAL.Student", b =>
                {
                    b.HasOne("svc_InterviewBack.DAL.Season", "Season")
                        .WithMany("Students")
                        .HasForeignKey("SeasonId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("svc_InterviewBack.DAL.Company", "Company")
                        .WithMany()
                        .HasForeignKey("CompanyId", "CompanySeasonId");

                    b.Navigation("Company");

                    b.Navigation("Season");
                });

            modelBuilder.Entity("svc_InterviewBack.DAL.Company", b =>
                {
                    b.Navigation("Positions");
                });

            modelBuilder.Entity("svc_InterviewBack.DAL.InterviewRequest", b =>
                {
                    b.Navigation("RequestResult");

                    b.Navigation("RequestStatusSnapshots");
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
