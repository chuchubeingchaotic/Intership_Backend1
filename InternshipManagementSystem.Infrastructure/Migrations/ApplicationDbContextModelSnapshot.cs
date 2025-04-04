﻿// <auto-generated />
using System;
using InternshipManagementSystem.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace InternshipManagementSystem.Infrastructure.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("InternshipManagementSystem.Core.Entities.Admin", b =>
                {
                    b.Property<Guid>("AdminId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasDefaultValueSql("NEWID()");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("MatKhau")
                        .IsRequired()
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.HasKey("AdminId");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.ToTable("Admins");
                });

            modelBuilder.Entity("InternshipManagementSystem.Core.Entities.DangKyThucTap", b =>
                {
                    b.Property<Guid>("DkttId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasDefaultValueSql("NEWID()");

                    b.Property<Guid>("DnId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("NgayDangKy")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("GETDATE()");

                    b.Property<Guid>("SvId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("TrangThai")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.HasKey("DkttId");

                    b.HasIndex("DnId");

                    b.HasIndex("SvId");

                    b.ToTable("DangKyThucTaps");
                });

            modelBuilder.Entity("InternshipManagementSystem.Core.Entities.DoanhNghiep", b =>
                {
                    b.Property<Guid>("DnId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasDefaultValueSql("NEWID()");

                    b.Property<string>("DiaChi")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("MatKhau")
                        .IsRequired()
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("SoDT")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.Property<string>("TenDN")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("DnId");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.ToTable("DoanhNghieps");
                });

            modelBuilder.Entity("InternshipManagementSystem.Core.Entities.SinhVien", b =>
                {
                    b.Property<Guid>("SvId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasDefaultValueSql("NEWID()");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<decimal>("GPA")
                        .HasPrecision(3, 2)
                        .HasColumnType("decimal(3,2)");

                    b.Property<string>("HoTen")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("nvarchar(64)");

                    b.Property<string>("Lop")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.Property<string>("MaSV")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.Property<string>("MatKhau")
                        .IsRequired()
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("SDT")
                        .IsRequired()
                        .HasMaxLength(15)
                        .HasColumnType("nvarchar(15)");

                    b.HasKey("SvId");

                    b.HasIndex("MaSV")
                        .IsUnique();

                    b.ToTable("SinhViens");
                });

            modelBuilder.Entity("InternshipManagementSystem.Core.Entities.DangKyThucTap", b =>
                {
                    b.HasOne("InternshipManagementSystem.Core.Entities.DoanhNghiep", "DoanhNghiep")
                        .WithMany("DangKyThucTaps")
                        .HasForeignKey("DnId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("InternshipManagementSystem.Core.Entities.SinhVien", "SinhVien")
                        .WithMany("DangKyThucTaps")
                        .HasForeignKey("SvId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("DoanhNghiep");

                    b.Navigation("SinhVien");
                });

            modelBuilder.Entity("InternshipManagementSystem.Core.Entities.DoanhNghiep", b =>
                {
                    b.Navigation("DangKyThucTaps");
                });

            modelBuilder.Entity("InternshipManagementSystem.Core.Entities.SinhVien", b =>
                {
                    b.Navigation("DangKyThucTaps");
                });
#pragma warning restore 612, 618
        }
    }
}
