using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Task_Management.Migrations
{
    public partial class InitMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tb_m_accounts",
                columns: table => new
                {
                    guid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    username = table.Column<string>(type: "nvarchar(255)", nullable: false),
                    email = table.Column<string>(type: "nvarchar(255)", nullable: false),
                    otp = table.Column<int>(type: "int", nullable: false),
                    is_used_otp = table.Column<bool>(type: "bit", nullable: false),
                    password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    modified_at = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_m_accounts", x => x.guid);
                });

            migrationBuilder.CreateTable(
                name: "tb_m_assignemts",
                columns: table => new
                {
                    guid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    title = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    description = table.Column<string>(type: "nvarchar(255)", nullable: false),
                    due_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    status = table.Column<int>(type: "int", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    modified_at = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_m_assignemts", x => x.guid);
                });

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
                name: "tb_m_divisions",
                columns: table => new
                {
                    guid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    name = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    locations = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    modified_at = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_m_divisions", x => x.guid);
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
                name: "tb_tr_progresses",
                columns: table => new
                {
                    guid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    account_guid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    assignment_guid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    is_completed = table.Column<bool>(type: "bit", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    modified_at = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_tr_progresses", x => x.guid);
                    table.ForeignKey(
                        name: "FK_tb_tr_progresses_tb_m_accounts_account_guid",
                        column: x => x.account_guid,
                        principalTable: "tb_m_accounts",
                        principalColumn: "guid",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tb_tr_progresses_tb_m_assignemts_assignment_guid",
                        column: x => x.assignment_guid,
                        principalTable: "tb_m_assignemts",
                        principalColumn: "guid",
                        onDelete: ReferentialAction.Cascade);
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
                        principalColumn: "guid");
                    table.ForeignKey(
                        name: "FK_tb_tr_assign_maps_tb_m_categories_category_guid",
                        column: x => x.category_guid,
                        principalTable: "tb_m_categories",
                        principalColumn: "guid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tb_m_account_divisions",
                columns: table => new
                {
                    guid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    account_guid = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    division_guid = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    position = table.Column<string>(type: "nvarchar(120)", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    modified_at = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_m_account_divisions", x => x.guid);
                    table.ForeignKey(
                        name: "FK_tb_m_account_divisions_tb_m_accounts_account_guid",
                        column: x => x.account_guid,
                        principalTable: "tb_m_accounts",
                        principalColumn: "guid");
                    table.ForeignKey(
                        name: "FK_tb_m_account_divisions_tb_m_divisions_division_guid",
                        column: x => x.division_guid,
                        principalTable: "tb_m_divisions",
                        principalColumn: "guid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tb_tr_account_roles",
                columns: table => new
                {
                    guid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    account_guid = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    role_guid = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    modified_at = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_tr_account_roles", x => x.guid);
                    table.ForeignKey(
                        name: "FK_tb_tr_account_roles_tb_m_accounts_account_guid",
                        column: x => x.account_guid,
                        principalTable: "tb_m_accounts",
                        principalColumn: "guid");
                    table.ForeignKey(
                        name: "FK_tb_tr_account_roles_tb_m_roles_role_guid",
                        column: x => x.role_guid,
                        principalTable: "tb_m_roles",
                        principalColumn: "guid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tb_m_account_divisions_account_guid",
                table: "tb_m_account_divisions",
                column: "account_guid");

            migrationBuilder.CreateIndex(
                name: "IX_tb_m_account_divisions_division_guid",
                table: "tb_m_account_divisions",
                column: "division_guid");

            migrationBuilder.CreateIndex(
                name: "IX_tb_m_accounts_email",
                table: "tb_m_accounts",
                column: "email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_tb_tr_account_roles_account_guid",
                table: "tb_tr_account_roles",
                column: "account_guid");

            migrationBuilder.CreateIndex(
                name: "IX_tb_tr_account_roles_role_guid",
                table: "tb_tr_account_roles",
                column: "role_guid");

            migrationBuilder.CreateIndex(
                name: "IX_tb_tr_assign_maps_assignment_guid",
                table: "tb_tr_assign_maps",
                column: "assignment_guid");

            migrationBuilder.CreateIndex(
                name: "IX_tb_tr_assign_maps_category_guid",
                table: "tb_tr_assign_maps",
                column: "category_guid");

            migrationBuilder.CreateIndex(
                name: "IX_tb_tr_progresses_account_guid",
                table: "tb_tr_progresses",
                column: "account_guid");

            migrationBuilder.CreateIndex(
                name: "IX_tb_tr_progresses_assignment_guid",
                table: "tb_tr_progresses",
                column: "assignment_guid");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tb_m_account_divisions");

            migrationBuilder.DropTable(
                name: "tb_tr_account_roles");

            migrationBuilder.DropTable(
                name: "tb_tr_assign_maps");

            migrationBuilder.DropTable(
                name: "tb_tr_progresses");

            migrationBuilder.DropTable(
                name: "tb_m_divisions");

            migrationBuilder.DropTable(
                name: "tb_m_roles");

            migrationBuilder.DropTable(
                name: "tb_m_categories");

            migrationBuilder.DropTable(
                name: "tb_m_accounts");

            migrationBuilder.DropTable(
                name: "tb_m_assignemts");
        }
    }
}
