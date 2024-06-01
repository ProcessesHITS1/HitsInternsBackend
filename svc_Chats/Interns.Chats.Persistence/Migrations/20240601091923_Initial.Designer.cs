﻿// <auto-generated />
using System;
using System.Collections.Generic;
using Interns.Chats.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Interns.Chats.Persistence.Migrations
{
    [DbContext(typeof(ChatDbContext))]
    [Migration("20240601091923_Initial")]
    partial class Initial
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.6")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Interns.Chats.Domain.Group", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid>("OwnerId")
                        .HasColumnType("uuid");

                    b.Property<List<Guid>>("UserIds")
                        .IsRequired()
                        .HasColumnType("uuid[]");

                    b.HasKey("Id");

                    b.ToTable("Groups");
                });

            modelBuilder.Entity("Interns.Chats.Domain.Message", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("AuthorId")
                        .HasColumnType("uuid");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid?>("GroupId")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("SentAt")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.HasIndex("GroupId");

                    b.ToTable("Messages");
                });

            modelBuilder.Entity("Interns.Chats.Domain.Message", b =>
                {
                    b.HasOne("Interns.Chats.Domain.Group", null)
                        .WithMany("Messages")
                        .HasForeignKey("GroupId");
                });

            modelBuilder.Entity("Interns.Chats.Domain.Group", b =>
                {
                    b.Navigation("Messages");
                });
#pragma warning restore 612, 618
        }
    }
}