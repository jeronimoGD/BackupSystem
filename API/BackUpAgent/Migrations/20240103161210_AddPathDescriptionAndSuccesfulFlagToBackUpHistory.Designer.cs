﻿// <auto-generated />
using System;
using BackUpAgent.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace BackUpAgent.Migrations
{
    [DbContext(typeof(BackUpSystemDbContext))]
    [Migration("20240103161210_AddPathDescriptionAndSuccesfulFlagToBackUpHistory")]
    partial class AddPathDescriptionAndSuccesfulFlagToBackUpHistory
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.14")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("BackUpAgent.Data.Entities.BackUpConfiguration", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ConfigurationName")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<bool>("CreateCloudBackUp")
                        .HasColumnType("bit");

                    b.Property<string>("ExcludedTablesJsonList")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("LastNBackUps")
                        .HasColumnType("int");

                    b.Property<int>("Periodicity")
                        .HasColumnType("int");

                    b.Property<string>("SourceDbName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("StoreLastNBackUps")
                        .HasColumnType("bit");

                    b.Property<string>("TarjetDbName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("ConfigurationName")
                        .IsUnique();

                    b.ToTable("BackUpConfigurations");
                });

            modelBuilder.Entity("BackUpAgent.Data.Entities.BackUpHistory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<bool>("AvailableToDownload")
                        .HasColumnType("bit");

                    b.Property<string>("BackUpName")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("BackUpPath")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("BackUpSizeInMB")
                        .HasColumnType("int");

                    b.Property<DateTime>("BuckUpDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsSuccessfull")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.HasIndex("BackUpName")
                        .IsUnique();

                    b.ToTable("BackUpHistory");
                });
#pragma warning restore 612, 618
        }
    }
}
