﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SolarRelog.Infrastructure;

#nullable disable

namespace SolarRelog.Infrastructure.Migrations.AppData
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20231005154036_Initial")]
    partial class Initial
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "7.0.10");

            modelBuilder.Entity("SolarRelog.Domain.Entities.DeviceEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("Ip")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<bool>("IsActive")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int?>("Port")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("devices", (string)null);
                });

            modelBuilder.Entity("SolarRelog.Domain.Entities.LogConsumerData", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("ConsumerIndex")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<decimal>("Consumption")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("LogDataId")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("LogDataId");

                    b.ToTable("log_consumer_data", (string)null);
                });

            modelBuilder.Entity("SolarRelog.Domain.Entities.LogDataEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<decimal>("ConsPac")
                        .HasColumnType("TEXT");

                    b.Property<decimal>("ConsYieldDay")
                        .HasColumnType("TEXT");

                    b.Property<decimal>("ConsYieldMonth")
                        .HasColumnType("TEXT");

                    b.Property<decimal>("ConsYieldTotal")
                        .HasColumnType("TEXT");

                    b.Property<decimal>("ConsYieldYear")
                        .HasColumnType("TEXT");

                    b.Property<decimal>("ConsYieldYesterday")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("DeviceId")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("LoggedDate")
                        .HasColumnType("TEXT");

                    b.Property<decimal>("Pac")
                        .HasColumnType("TEXT");

                    b.Property<decimal>("Pdc")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("RecordDate")
                        .HasColumnType("TEXT");

                    b.Property<decimal>("TotalPower")
                        .HasColumnType("TEXT");

                    b.Property<decimal>("Uac")
                        .HasColumnType("TEXT");

                    b.Property<decimal>("Udc")
                        .HasColumnType("TEXT");

                    b.Property<decimal>("YieldDay")
                        .HasColumnType("TEXT");

                    b.Property<decimal>("YieldMonth")
                        .HasColumnType("TEXT");

                    b.Property<decimal>("YieldTotal")
                        .HasColumnType("TEXT");

                    b.Property<decimal>("YieldYear")
                        .HasColumnType("TEXT");

                    b.Property<decimal>("YieldYesterday")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("DeviceId");

                    b.ToTable("log_data", (string)null);
                });

            modelBuilder.Entity("SolarRelog.Domain.Entities.SettingsEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("settings", (string)null);
                });

            modelBuilder.Entity("SolarRelog.Domain.Entities.LogConsumerData", b =>
                {
                    b.HasOne("SolarRelog.Domain.Entities.LogDataEntity", "LogData")
                        .WithMany("ConsumerData")
                        .HasForeignKey("LogDataId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("LogData");
                });

            modelBuilder.Entity("SolarRelog.Domain.Entities.LogDataEntity", b =>
                {
                    b.HasOne("SolarRelog.Domain.Entities.DeviceEntity", "Device")
                        .WithMany("LogData")
                        .HasForeignKey("DeviceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Device");
                });

            modelBuilder.Entity("SolarRelog.Domain.Entities.SettingsEntity", b =>
                {
                    b.OwnsOne("SolarRelog.Domain.Entities.AppLogSettings", "AppLogSettings", b1 =>
                        {
                            b1.Property<Guid>("SettingsEntityId")
                                .HasColumnType("TEXT");

                            b1.Property<string>("MinLogLevel")
                                .IsRequired()
                                .HasColumnType("TEXT");

                            b1.Property<int>("RetentionDays")
                                .HasColumnType("INTEGER");

                            b1.HasKey("SettingsEntityId");

                            b1.ToTable("settings");

                            b1.WithOwner()
                                .HasForeignKey("SettingsEntityId");
                        });

                    b.OwnsOne("SolarRelog.Domain.Entities.DataLogSettings", "DataLogSettings", b1 =>
                        {
                            b1.Property<Guid>("SettingsEntityId")
                                .HasColumnType("TEXT");

                            b1.Property<int>("PollingIntervalSeconds")
                                .HasColumnType("INTEGER");

                            b1.Property<int>("RetentionDays")
                                .HasColumnType("INTEGER");

                            b1.HasKey("SettingsEntityId");

                            b1.ToTable("settings");

                            b1.WithOwner()
                                .HasForeignKey("SettingsEntityId");
                        });

                    b.OwnsOne("SolarRelog.Domain.Entities.InfluxSettings", "InfluxSettings", b1 =>
                        {
                            b1.Property<Guid>("SettingsEntityId")
                                .HasColumnType("TEXT");

                            b1.Property<string>("ApiToken")
                                .IsRequired()
                                .HasColumnType("TEXT");

                            b1.Property<string>("Bucket")
                                .IsRequired()
                                .HasColumnType("TEXT");

                            b1.Property<string>("Organization")
                                .IsRequired()
                                .HasColumnType("TEXT");

                            b1.Property<string>("Url")
                                .IsRequired()
                                .HasColumnType("TEXT");

                            b1.HasKey("SettingsEntityId");

                            b1.ToTable("settings");

                            b1.WithOwner()
                                .HasForeignKey("SettingsEntityId");
                        });

                    b.Navigation("AppLogSettings")
                        .IsRequired();

                    b.Navigation("DataLogSettings")
                        .IsRequired();

                    b.Navigation("InfluxSettings")
                        .IsRequired();
                });

            modelBuilder.Entity("SolarRelog.Domain.Entities.DeviceEntity", b =>
                {
                    b.Navigation("LogData");
                });

            modelBuilder.Entity("SolarRelog.Domain.Entities.LogDataEntity", b =>
                {
                    b.Navigation("ConsumerData");
                });
#pragma warning restore 612, 618
        }
    }
}