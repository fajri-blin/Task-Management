using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Task_Management.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tb_m_categories",
                columns: table => new
                {
                    guid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    name = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    modified_at = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_m_categories", x => x.guid);
                });

            migrationBuilder.CreateTable(
                name: "tb_m_roles",
                columns: table => new
                {
                    guid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    name = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    modified_at = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_m_roles", x => x.guid);
                });

            migrationBuilder.CreateTable(
                name: "tb_m_accounts",
                columns: table => new
                {
                    guid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    username = table.Column<string>(type: "nvarchar(255)", nullable: false),
                    email = table.Column<string>(type: "nvarchar(255)", nullable: false),
                    name = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    otp = table.Column<int>(type: "int", nullable: false),
                    is_used_otp = table.Column<bool>(type: "bit", nullable: false),
                    password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    image_profile = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    role_guid = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    modified_at = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_m_accounts", x => x.guid);
                    table.ForeignKey(
                        name: "FK_tb_m_accounts_tb_m_roles_role_guid",
                        column: x => x.role_guid,
                        principalTable: "tb_m_roles",
                        principalColumn: "guid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tb_m_assignemts",
                columns: table => new
                {
                    guid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    manager_guid = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    title = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    description = table.Column<string>(type: "nvarchar(255)", nullable: false),
                    due_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    is_completed = table.Column<bool>(type: "bit", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    modified_at = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_m_assignemts", x => x.guid);
                    table.ForeignKey(
                        name: "FK_tb_m_assignemts_tb_m_accounts_manager_guid",
                        column: x => x.manager_guid,
                        principalTable: "tb_m_accounts",
                        principalColumn: "guid",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "tb_m_progresses",
                columns: table => new
                {
                    guid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    assignment_guid = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    description = table.Column<string>(type: "nvarchar(150)", nullable: false),
                    status = table.Column<int>(type: "int", nullable: false),
                    additional = table.Column<string>(type: "nvarchar(255)", nullable: true),
                    check_mark = table.Column<bool>(type: "bit", nullable: false),
                    manager_message = table.Column<string>(type: "nvarchar(255)", nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    modified_at = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_m_progresses", x => x.guid);
                    table.ForeignKey(
                        name: "FK_tb_m_progresses_tb_m_assignemts_assignment_guid",
                        column: x => x.assignment_guid,
                        principalTable: "tb_m_assignemts",
                        principalColumn: "guid",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "tb_tr_assign_maps",
                columns: table => new
                {
                    guid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    assignment_guid = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    category_guid = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    modified_at = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_tr_assign_maps", x => x.guid);
                    table.ForeignKey(
                        name: "FK_tb_tr_assign_maps_tb_m_assignemts_assignment_guid",
                        column: x => x.assignment_guid,
                        principalTable: "tb_m_assignemts",
                        principalColumn: "guid",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tb_tr_assign_maps_tb_m_categories_category_guid",
                        column: x => x.category_guid,
                        principalTable: "tb_m_categories",
                        principalColumn: "guid",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "tb_m_additional",
                columns: table => new
                {
                    guid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    progress_guid = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    filename = table.Column<string>(type: "nvarchar(255)", nullable: false),
                    filedata = table.Column<string>(type: "nvarchar(255)", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    modified_at = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_m_additional", x => x.guid);
                    table.ForeignKey(
                        name: "FK_tb_m_additional_tb_m_progresses_progress_guid",
                        column: x => x.progress_guid,
                        principalTable: "tb_m_progresses",
                        principalColumn: "guid",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "tb_tr_account_progress",
                columns: table => new
                {
                    guid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    account_guid = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    progress_guid = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    modified_at = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_tr_account_progress", x => x.guid);
                    table.ForeignKey(
                        name: "FK_tb_tr_account_progress_tb_m_accounts_account_guid",
                        column: x => x.account_guid,
                        principalTable: "tb_m_accounts",
                        principalColumn: "guid",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_tb_tr_account_progress_tb_m_progresses_progress_guid",
                        column: x => x.progress_guid,
                        principalTable: "tb_m_progresses",
                        principalColumn: "guid",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tb_m_accounts_email_username",
                table: "tb_m_accounts",
                columns: new[] { "email", "username" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_tb_m_accounts_role_guid",
                table: "tb_m_accounts",
                column: "role_guid");

            migrationBuilder.CreateIndex(
                name: "IX_tb_m_additional_progress_guid",
                table: "tb_m_additional",
                column: "progress_guid");

            migrationBuilder.CreateIndex(
                name: "IX_tb_m_assignemts_manager_guid",
                table: "tb_m_assignemts",
                column: "manager_guid");

            migrationBuilder.CreateIndex(
                name: "IX_tb_m_progresses_assignment_guid",
                table: "tb_m_progresses",
                column: "assignment_guid");

            migrationBuilder.CreateIndex(
                name: "IX_tb_tr_account_progress_account_guid",
                table: "tb_tr_account_progress",
                column: "account_guid");

            migrationBuilder.CreateIndex(
                name: "IX_tb_tr_account_progress_progress_guid",
                table: "tb_tr_account_progress",
                column: "progress_guid");

            migrationBuilder.CreateIndex(
                name: "IX_tb_tr_assign_maps_assignment_guid",
                table: "tb_tr_assign_maps",
                column: "assignment_guid");

            migrationBuilder.CreateIndex(
                name: "IX_tb_tr_assign_maps_category_guid",
                table: "tb_tr_assign_maps",
                column: "category_guid");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tb_m_additional");

            migrationBuilder.DropTable(
                name: "tb_tr_account_progress");

            migrationBuilder.DropTable(
                name: "tb_tr_assign_maps");

            migrationBuilder.DropTable(
                name: "tb_m_progresses");

            migrationBuilder.DropTable(
                name: "tb_m_categories");

            migrationBuilder.DropTable(
                name: "tb_m_assignemts");

            migrationBuilder.DropTable(
                name: "tb_m_accounts");

            migrationBuilder.DropTable(
                name: "tb_m_roles");
        }
    }
}
