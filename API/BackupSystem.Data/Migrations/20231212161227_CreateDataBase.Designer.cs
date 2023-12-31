﻿// <auto-generated />
using System;
using BackupSystem.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace BackupSystem.Data.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20231212161227_CreateDataBase")]
    partial class CreateDataBase
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.14")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("BackupSystem.Models.Entities.Agent", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("AgentName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("ConnectionKey")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.ToTable("Agents");
                });

            modelBuilder.Entity("BackupSystem.Models.Entities.BackUpConfiguration", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<bool>("CleanEventTables")
                        .HasColumnType("bit");

                    b.Property<string>("ConfigurationName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("CreateCloudBackUp")
                        .HasColumnType("bit");

                    b.Property<int>("LastNBackUps")
                        .HasColumnType("int");

                    b.Property<string>("PeriodicBackUpType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("StoreLastNBackUps")
                        .HasColumnType("bit");

                    b.Property<string>("TarjetDbName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("BackUpConfigurations");
                });

            modelBuilder.Entity("BackupSystem.Models.Entities.BackUpHistory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<bool>("AvailableToDownload")
                        .HasColumnType("bit");

                    b.Property<string>("BackUpName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("BackUpSize")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("BuckUpDate")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("BackUpHistory");
                });
#pragma warning restore 612, 618
        }
    }
}
