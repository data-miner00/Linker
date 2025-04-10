﻿// <auto-generated />
using System;
using Linker.Cli.Integrations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Linker.Cli.Integrations.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20250215131528_initial")]
    partial class initial
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "9.0.2");

            modelBuilder.Entity("LinkUrlList", b =>
                {
                    b.Property<int>("LinkId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("UrlListId")
                        .HasColumnType("INTEGER");

                    b.HasKey("LinkId", "UrlListId");

                    b.HasIndex("UrlListId");

                    b.ToTable("LinkUrlList");
                });

            modelBuilder.Entity("Linker.Cli.Core.Link", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("TEXT");

                    b.Property<string>("Description")
                        .HasColumnType("TEXT");

                    b.Property<string>("Language")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("ModifiedAt")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<string>("Tags")
                        .HasColumnType("TEXT");

                    b.Property<string>("Url")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<bool>("WatchLater")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("Links");
                });

            modelBuilder.Entity("Linker.Cli.Core.UrlList", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("TEXT");

                    b.Property<string>("Description")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("ModifiedAt")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Lists");
                });

            modelBuilder.Entity("Linker.Cli.Core.Visit", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("TEXT");

                    b.Property<int>("LinkId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("LinkId");

                    b.ToTable("Visits");
                });

            modelBuilder.Entity("LinkUrlList", b =>
                {
                    b.HasOne("Linker.Cli.Core.Link", null)
                        .WithMany()
                        .HasForeignKey("LinkId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Linker.Cli.Core.UrlList", null)
                        .WithMany()
                        .HasForeignKey("UrlListId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Linker.Cli.Core.Visit", b =>
                {
                    b.HasOne("Linker.Cli.Core.Link", "Link")
                        .WithMany("Visits")
                        .HasForeignKey("LinkId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Link");
                });

            modelBuilder.Entity("Linker.Cli.Core.Link", b =>
                {
                    b.Navigation("Visits");
                });
#pragma warning restore 612, 618
        }
    }
}
