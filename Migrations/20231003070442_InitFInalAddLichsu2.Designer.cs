﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SoftVersionControl.Models;

#nullable disable

namespace SoftVersionControl.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20231003070442_InitFInalAddLichsu2")]
    partial class InitFInalAddLichsu2
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.22")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("SoftVersionControl.Models.Lichsu", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Hanhdong")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Ten")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("Thoigian")
                        .HasColumnType("datetime2");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Lichsus");
                });

            modelBuilder.Entity("SoftVersionControl.Models.Model", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("ParentModelId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ParentModelId");

                    b.ToTable("Models");
                });

            modelBuilder.Entity("SoftVersionControl.Models.Password", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Hoten")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Pwd")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Role")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Passwords");
                });

            modelBuilder.Entity("SoftVersionControl.Models.SoftName", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("ModelId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("ModelId");

                    b.ToTable("SoftNames");
                });

            modelBuilder.Entity("SoftVersionControl.Models.Software", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("DiemThayDoi")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FileName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("NgayApDung")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("NgayCaiDat")
                        .HasColumnType("datetime2");

                    b.Property<string>("NguoiCaiDat")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NguoiXacNhan")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Path")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("SoLuongJigCaiDat")
                        .HasColumnType("int");

                    b.Property<int>("SoftNameId")
                        .HasColumnType("int");

                    b.Property<DateTime>("TimeCreated")
                        .HasColumnType("datetime2");

                    b.Property<bool>("TrangThaiApDung")
                        .HasColumnType("bit");

                    b.Property<string>("Version")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("SoftNameId");

                    b.ToTable("Softwares");
                });

            modelBuilder.Entity("SoftVersionControl.Models.Model", b =>
                {
                    b.HasOne("SoftVersionControl.Models.Model", "ParentModel")
                        .WithMany("ChildrenModel")
                        .HasForeignKey("ParentModelId");

                    b.Navigation("ParentModel");
                });

            modelBuilder.Entity("SoftVersionControl.Models.SoftName", b =>
                {
                    b.HasOne("SoftVersionControl.Models.Model", "Model")
                        .WithMany("SoftNames")
                        .HasForeignKey("ModelId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Model");
                });

            modelBuilder.Entity("SoftVersionControl.Models.Software", b =>
                {
                    b.HasOne("SoftVersionControl.Models.SoftName", "SoftName")
                        .WithMany("Softwares")
                        .HasForeignKey("SoftNameId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("SoftName");
                });

            modelBuilder.Entity("SoftVersionControl.Models.Model", b =>
                {
                    b.Navigation("ChildrenModel");

                    b.Navigation("SoftNames");
                });

            modelBuilder.Entity("SoftVersionControl.Models.SoftName", b =>
                {
                    b.Navigation("Softwares");
                });
#pragma warning restore 612, 618
        }
    }
}