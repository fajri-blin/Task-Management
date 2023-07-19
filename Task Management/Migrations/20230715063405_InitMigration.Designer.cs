﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Task_Management.Data;

#nullable disable

namespace Task_Management.Migrations
{
    [DbContext(typeof(BookingDbContext))]
    [Migration("20230715063405_InitMigration")]
    partial class InitMigration
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.19")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("Task_Management.Model.Account", b =>
                {
                    b.Property<Guid>("Guid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("guid");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2")
                        .HasColumnName("created_at");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(255)")
                        .HasColumnName("email");

                    b.Property<bool>("IsUsedOTP")
                        .HasColumnType("bit")
                        .HasColumnName("is_used_otp");

                    b.Property<DateTime>("ModifiedAt")
                        .HasColumnType("datetime2")
                        .HasColumnName("modified_at");

                    b.Property<int>("OTP")
                        .HasColumnType("int")
                        .HasColumnName("otp");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("password");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("nvarchar(255)")
                        .HasColumnName("username");

                    b.HasKey("Guid");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.ToTable("tb_m_accounts");
                });

            modelBuilder.Entity("Task_Management.Model.AccountDivision", b =>
                {
                    b.Property<Guid>("Guid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("guid");

                    b.Property<Guid?>("AccountGuid")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("account_guid");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2")
                        .HasColumnName("created_at");

                    b.Property<Guid?>("DivisionGuid")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("division_guid");

                    b.Property<DateTime>("ModifiedAt")
                        .HasColumnType("datetime2")
                        .HasColumnName("modified_at");

                    b.Property<string>("Position")
                        .IsRequired()
                        .HasColumnType("nvarchar(120)")
                        .HasColumnName("position");

                    b.HasKey("Guid");

                    b.HasIndex("AccountGuid");

                    b.HasIndex("DivisionGuid");

                    b.ToTable("tb_m_account_divisions");
                });

            modelBuilder.Entity("Task_Management.Model.AccountRole", b =>
                {
                    b.Property<Guid>("Guid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("guid");

                    b.Property<Guid?>("AccountGuid")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("account_guid");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2")
                        .HasColumnName("created_at");

                    b.Property<DateTime>("ModifiedAt")
                        .HasColumnType("datetime2")
                        .HasColumnName("modified_at");

                    b.Property<Guid?>("RoleGuid")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("role_guid");

                    b.HasKey("Guid");

                    b.HasIndex("AccountGuid");

                    b.HasIndex("RoleGuid");

                    b.ToTable("tb_tr_account_roles");
                });

            modelBuilder.Entity("Task_Management.Model.AssignMap", b =>
                {
                    b.Property<Guid>("Guid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("guid");

                    b.Property<Guid?>("AssignmentGuid")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("assignment_guid");

                    b.Property<Guid?>("CategoryGuid")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("category_guid");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2")
                        .HasColumnName("created_at");

                    b.Property<DateTime>("ModifiedAt")
                        .HasColumnType("datetime2")
                        .HasColumnName("modified_at");

                    b.HasKey("Guid");

                    b.HasIndex("AssignmentGuid");

                    b.HasIndex("CategoryGuid");

                    b.ToTable("tb_tr_assign_maps");
                });

            modelBuilder.Entity("Task_Management.Model.Assignment", b =>
                {
                    b.Property<Guid>("Guid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("guid");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2")
                        .HasColumnName("created_at");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(255)")
                        .HasColumnName("description");

                    b.Property<DateTime>("DueDate")
                        .HasColumnType("datetime2")
                        .HasColumnName("due_date");

                    b.Property<DateTime>("ModifiedAt")
                        .HasColumnType("datetime2")
                        .HasColumnName("modified_at");

                    b.Property<int>("Status")
                        .HasColumnType("int")
                        .HasColumnName("status");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)")
                        .HasColumnName("title");

                    b.HasKey("Guid");

                    b.ToTable("tb_m_assignemts");
                });

            modelBuilder.Entity("Task_Management.Model.Category", b =>
                {
                    b.Property<Guid>("Guid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("guid");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2")
                        .HasColumnName("created_at");

                    b.Property<DateTime>("ModifiedAt")
                        .HasColumnType("datetime2")
                        .HasColumnName("modified_at");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)")
                        .HasColumnName("name");

                    b.HasKey("Guid");

                    b.ToTable("tb_m_categories");
                });

            modelBuilder.Entity("Task_Management.Model.Division", b =>
                {
                    b.Property<Guid>("Guid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("guid");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2")
                        .HasColumnName("created_at");

                    b.Property<string>("Location")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)")
                        .HasColumnName("locations");

                    b.Property<DateTime>("ModifiedAt")
                        .HasColumnType("datetime2")
                        .HasColumnName("modified_at");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)")
                        .HasColumnName("name");

                    b.HasKey("Guid");

                    b.ToTable("tb_m_divisions");
                });

            modelBuilder.Entity("Task_Management.Model.Progress", b =>
                {
                    b.Property<Guid>("Guid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("guid");

                    b.Property<Guid>("AccountGuid")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("account_guid");

                    b.Property<Guid>("AssignmentGuid")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("assignment_guid");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2")
                        .HasColumnName("created_at");

                    b.Property<bool>("IsCompleted")
                        .HasColumnType("bit")
                        .HasColumnName("is_completed");

                    b.Property<DateTime>("ModifiedAt")
                        .HasColumnType("datetime2")
                        .HasColumnName("modified_at");

                    b.HasKey("Guid");

                    b.HasIndex("AccountGuid");

                    b.HasIndex("AssignmentGuid");

                    b.ToTable("tb_tr_progresses");
                });

            modelBuilder.Entity("Task_Management.Model.Role", b =>
                {
                    b.Property<Guid>("Guid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("guid");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2")
                        .HasColumnName("created_at");

                    b.Property<DateTime>("ModifiedAt")
                        .HasColumnType("datetime2")
                        .HasColumnName("modified_at");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)")
                        .HasColumnName("name");

                    b.HasKey("Guid");

                    b.ToTable("tb_m_roles");
                });

            modelBuilder.Entity("Task_Management.Model.AccountDivision", b =>
                {
                    b.HasOne("Task_Management.Model.Account", "Account")
                        .WithMany("AccountDivisions")
                        .HasForeignKey("AccountGuid");

                    b.HasOne("Task_Management.Model.Division", "Division")
                        .WithMany("AccountDivisions")
                        .HasForeignKey("DivisionGuid")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.Navigation("Account");

                    b.Navigation("Division");
                });

            modelBuilder.Entity("Task_Management.Model.AccountRole", b =>
                {
                    b.HasOne("Task_Management.Model.Account", "Account")
                        .WithMany("AccountRoles")
                        .HasForeignKey("AccountGuid");

                    b.HasOne("Task_Management.Model.Role", "Role")
                        .WithMany("AccountRoles")
                        .HasForeignKey("RoleGuid")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.Navigation("Account");

                    b.Navigation("Role");
                });

            modelBuilder.Entity("Task_Management.Model.AssignMap", b =>
                {
                    b.HasOne("Task_Management.Model.Assignment", "Assignment")
                        .WithMany("AssignMaps")
                        .HasForeignKey("AssignmentGuid");

                    b.HasOne("Task_Management.Model.Category", "Category")
                        .WithMany("AssignMaps")
                        .HasForeignKey("CategoryGuid")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.Navigation("Assignment");

                    b.Navigation("Category");
                });

            modelBuilder.Entity("Task_Management.Model.Progress", b =>
                {
                    b.HasOne("Task_Management.Model.Account", "Account")
                        .WithMany("Progresses")
                        .HasForeignKey("AccountGuid")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Task_Management.Model.Assignment", "Assignment")
                        .WithMany("Progresses")
                        .HasForeignKey("AssignmentGuid")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Account");

                    b.Navigation("Assignment");
                });

            modelBuilder.Entity("Task_Management.Model.Account", b =>
                {
                    b.Navigation("AccountDivisions");

                    b.Navigation("AccountRoles");

                    b.Navigation("Progresses");
                });

            modelBuilder.Entity("Task_Management.Model.Assignment", b =>
                {
                    b.Navigation("AssignMaps");

                    b.Navigation("Progresses");
                });

            modelBuilder.Entity("Task_Management.Model.Category", b =>
                {
                    b.Navigation("AssignMaps");
                });

            modelBuilder.Entity("Task_Management.Model.Division", b =>
                {
                    b.Navigation("AccountDivisions");
                });

            modelBuilder.Entity("Task_Management.Model.Role", b =>
                {
                    b.Navigation("AccountRoles");
                });
#pragma warning restore 612, 618
        }
    }
}