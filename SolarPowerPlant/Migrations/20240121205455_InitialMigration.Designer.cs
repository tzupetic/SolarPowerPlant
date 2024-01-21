﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using SolarPowerPlant.Data;

#nullable disable

namespace SolarPowerPlant.Migrations
{
    [DbContext(typeof(PowerPlantContext))]
    [Migration("20240121205455_InitialMigration")]
    partial class InitialMigration
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.15")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("SolarPowerPlant.PowerPlants.PowerPlant", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("DateOfInstallation")
                        .HasColumnType("timestamp with time zone");

                    b.Property<double>("InstalledPower")
                        .HasColumnType("double precision");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean");

                    b.Property<double>("LocationLatitude")
                        .HasColumnType("double precision");

                    b.Property<double>("LocationLongitude")
                        .HasColumnType("double precision");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("UserCreatedId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("UserUpdatedId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("UserCreatedId");

                    b.HasIndex("UserUpdatedId");

                    b.ToTable("PowerPlants");
                });

            modelBuilder.Entity("SolarPowerPlant.PowerPlants.ProductionData", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("PowerPlantId")
                        .HasColumnType("uuid");

                    b.Property<double>("ProductionValue")
                        .HasColumnType("double precision");

                    b.Property<DateTime>("Timestamp")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("Type")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("PowerPlantId");

                    b.HasIndex("Type", "Timestamp");

                    b.ToTable("ProductionData");
                });

            modelBuilder.Entity("SolarPowerPlant.Users.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    NpgsqlPropertyBuilderExtensions.HasIdentityOptions(b.Property<Guid>("Id"), 2L, null, null, null, null, null);

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.ToTable("Users");

                    b.HasData(
                        new
                        {
                            Id = new Guid("00000000-0000-0000-0000-000000000001"),
                            CreatedAt = new DateTime(2022, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
                            Email = "admin@powerplant.com",
                            FirstName = "System",
                            LastName = "",
                            Password = "",
                            UpdatedAt = new DateTime(2022, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc)
                        });
                });

            modelBuilder.Entity("SolarPowerPlant.PowerPlants.PowerPlant", b =>
                {
                    b.HasOne("SolarPowerPlant.Users.User", "UserCreated")
                        .WithMany()
                        .HasForeignKey("UserCreatedId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SolarPowerPlant.Users.User", "UserUpdated")
                        .WithMany()
                        .HasForeignKey("UserUpdatedId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("UserCreated");

                    b.Navigation("UserUpdated");
                });

            modelBuilder.Entity("SolarPowerPlant.PowerPlants.ProductionData", b =>
                {
                    b.HasOne("SolarPowerPlant.PowerPlants.PowerPlant", "PowerPlant")
                        .WithMany("ProductionData")
                        .HasForeignKey("PowerPlantId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("PowerPlant");
                });

            modelBuilder.Entity("SolarPowerPlant.PowerPlants.PowerPlant", b =>
                {
                    b.Navigation("ProductionData");
                });
#pragma warning restore 612, 618
        }
    }
}