﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using orkesterapp.Data;

#nullable disable

namespace orkesterapp.Migrations
{
    [DbContext(typeof(OrchesterContext))]
    [Migration("20231229140928_Initial")]
    partial class Initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.22")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("orkesterapp.Models.Orchester", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"), 1L, 1);

                    b.Property<string>("OrchestraName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ID");

                    b.ToTable("Orchesters", (string)null);
                });

            modelBuilder.Entity("orkesterapp.Models.Performance", b =>
                {
                    b.Property<int>("OrchesterID")
                        .HasColumnType("int");

                    b.Property<int>("VenueID")
                        .HasColumnType("int");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<int>("ID")
                        .HasColumnType("int");

                    b.HasKey("OrchesterID", "VenueID");

                    b.HasIndex("VenueID");

                    b.ToTable("Performances", (string)null);
                });

            modelBuilder.Entity("orkesterapp.Models.Role", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"), 1L, 1);

                    b.Property<string>("RoleName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ID");

                    b.ToTable("Roles", (string)null);
                });

            modelBuilder.Entity("orkesterapp.Models.User", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"), 1L, 1);

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FirstMidName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Geslo")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("OrchesterID")
                        .HasColumnType("int");

                    b.Property<int>("RoleID")
                        .HasColumnType("int");

                    b.HasKey("ID");

                    b.HasIndex("OrchesterID");

                    b.HasIndex("RoleID");

                    b.ToTable("Users", (string)null);
                });

            modelBuilder.Entity("orkesterapp.Models.Venue", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"), 1L, 1);

                    b.Property<string>("Location")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("VenueName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ID");

                    b.ToTable("Venues", (string)null);
                });

            modelBuilder.Entity("orkesterapp.Models.Performance", b =>
                {
                    b.HasOne("orkesterapp.Models.Orchester", "Orchester")
                        .WithMany("Performances")
                        .HasForeignKey("OrchesterID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("orkesterapp.Models.Venue", "Venue")
                        .WithMany("Performances")
                        .HasForeignKey("VenueID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Orchester");

                    b.Navigation("Venue");
                });

            modelBuilder.Entity("orkesterapp.Models.User", b =>
                {
                    b.HasOne("orkesterapp.Models.Orchester", "Orchester")
                        .WithMany("Users")
                        .HasForeignKey("OrchesterID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("orkesterapp.Models.Role", "Role")
                        .WithMany("Users")
                        .HasForeignKey("RoleID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Orchester");

                    b.Navigation("Role");
                });

            modelBuilder.Entity("orkesterapp.Models.Orchester", b =>
                {
                    b.Navigation("Performances");

                    b.Navigation("Users");
                });

            modelBuilder.Entity("orkesterapp.Models.Role", b =>
                {
                    b.Navigation("Users");
                });

            modelBuilder.Entity("orkesterapp.Models.Venue", b =>
                {
                    b.Navigation("Performances");
                });
#pragma warning restore 612, 618
        }
    }
}