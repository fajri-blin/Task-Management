using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Task_Management.Migrations
{
    public partial class Adding_and_Fixing_Account_Progress : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tb_tr_assign_maps_tb_m_assignemts_assignment_guid",
                table: "tb_tr_assign_maps");

            migrationBuilder.DropForeignKey(
                name: "FK_tb_tr_progresses_tb_m_accounts_account_guid",
                table: "tb_tr_progresses");

            migrationBuilder.DropForeignKey(
                name: "FK_tb_tr_progresses_tb_m_assignemts_assignment_guid",
                table: "tb_tr_progresses");

            migrationBuilder.DropTable(
                name: "tb_m_account_divisions");

            migrationBuilder.DropTable(
                name: "tb_m_divisions");

            migrationBuilder.DropIndex(
                name: "IX_tb_m_accounts_email",
                table: "tb_m_accounts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_tb_tr_progresses",
                table: "tb_tr_progresses");

            migrationBuilder.DropIndex(
                name: "IX_tb_tr_progresses_account_guid",
                table: "tb_tr_progresses");

            migrationBuilder.DropColumn(
                name: "status",
                table: "tb_m_assignemts");

            migrationBuilder.DropColumn(
                name: "account_guid",
                table: "tb_tr_progresses");

            migrationBuilder.RenameTable(
                name: "tb_tr_progresses",
                newName: "tb_m_progresses");

            migrationBuilder.RenameColumn(
                name: "is_completed",
                table: "tb_m_progresses",
                newName: "check_mark");

            migrationBuilder.RenameIndex(
                name: "IX_tb_tr_progresses_assignment_guid",
                table: "tb_m_progresses",
                newName: "IX_tb_m_progresses_assignment_guid");

            migrationBuilder.AddColumn<bool>(
                name: "is_completed",
                table: "tb_m_assignemts",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<Guid>(
                name: "manager_guid",
                table: "tb_m_assignemts",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "image_profile",
                table: "tb_m_accounts",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "name",
                table: "tb_m_accounts",
                type: "nvarchar(100)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<Guid>(
                name: "assignment_guid",
                table: "tb_m_progresses",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddColumn<string>(
                name: "additional",
                table: "tb_m_progresses",
                type: "nvarchar(255)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "description",
                table: "tb_m_progresses",
                type: "nvarchar(150)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "manager_message",
                table: "tb_m_progresses",
                type: "nvarchar(255)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "status",
                table: "tb_m_progresses",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_tb_m_progresses",
                table: "tb_m_progresses",
                column: "guid");

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
                name: "IX_tb_m_assignemts_manager_guid",
                table: "tb_m_assignemts",
                column: "manager_guid");

            migrationBuilder.CreateIndex(
                name: "IX_tb_m_accounts_email_username",
                table: "tb_m_accounts",
                columns: new[] { "email", "username" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_tb_tr_account_progress_account_guid",
                table: "tb_tr_account_progress",
                column: "account_guid");

            migrationBuilder.CreateIndex(
                name: "IX_tb_tr_account_progress_progress_guid",
                table: "tb_tr_account_progress",
                column: "progress_guid");

            migrationBuilder.AddForeignKey(
                name: "FK_tb_m_assignemts_tb_m_accounts_manager_guid",
                table: "tb_m_assignemts",
                column: "manager_guid",
                principalTable: "tb_m_accounts",
                principalColumn: "guid",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_tb_m_progresses_tb_m_assignemts_assignment_guid",
                table: "tb_m_progresses",
                column: "assignment_guid",
                principalTable: "tb_m_assignemts",
                principalColumn: "guid",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_tb_tr_assign_maps_tb_m_assignemts_assignment_guid",
                table: "tb_tr_assign_maps",
                column: "assignment_guid",
                principalTable: "tb_m_assignemts",
                principalColumn: "guid",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tb_m_assignemts_tb_m_accounts_manager_guid",
                table: "tb_m_assignemts");

            migrationBuilder.DropForeignKey(
                name: "FK_tb_m_progresses_tb_m_assignemts_assignment_guid",
                table: "tb_m_progresses");

            migrationBuilder.DropForeignKey(
                name: "FK_tb_tr_assign_maps_tb_m_assignemts_assignment_guid",
                table: "tb_tr_assign_maps");

            migrationBuilder.DropTable(
                name: "tb_tr_account_progress");

            migrationBuilder.DropIndex(
                name: "IX_tb_m_assignemts_manager_guid",
                table: "tb_m_assignemts");

            migrationBuilder.DropIndex(
                name: "IX_tb_m_accounts_email_username",
                table: "tb_m_accounts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_tb_m_progresses",
                table: "tb_m_progresses");

            migrationBuilder.DropColumn(
                name: "is_completed",
                table: "tb_m_assignemts");

            migrationBuilder.DropColumn(
                name: "manager_guid",
                table: "tb_m_assignemts");

            migrationBuilder.DropColumn(
                name: "image_profile",
                table: "tb_m_accounts");

            migrationBuilder.DropColumn(
                name: "name",
                table: "tb_m_accounts");

            migrationBuilder.DropColumn(
                name: "additional",
                table: "tb_m_progresses");

            migrationBuilder.DropColumn(
                name: "description",
                table: "tb_m_progresses");

            migrationBuilder.DropColumn(
                name: "manager_message",
                table: "tb_m_progresses");

            migrationBuilder.DropColumn(
                name: "status",
                table: "tb_m_progresses");

            migrationBuilder.RenameTable(
                name: "tb_m_progresses",
                newName: "tb_tr_progresses");

            migrationBuilder.RenameColumn(
                name: "check_mark",
                table: "tb_tr_progresses",
                newName: "is_completed");

            migrationBuilder.RenameIndex(
                name: "IX_tb_m_progresses_assignment_guid",
                table: "tb_tr_progresses",
                newName: "IX_tb_tr_progresses_assignment_guid");

            migrationBuilder.AddColumn<int>(
                name: "status",
                table: "tb_m_assignemts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<Guid>(
                name: "assignment_guid",
                table: "tb_tr_progresses",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "account_guid",
                table: "tb_tr_progresses",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_tb_tr_progresses",
                table: "tb_tr_progresses",
                column: "guid");

            migrationBuilder.CreateTable(
                name: "tb_m_divisions",
                columns: table => new
                {
                    guid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    locations = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    modified_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    name = table.Column<string>(type: "nvarchar(100)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_m_divisions", x => x.guid);
                });

            migrationBuilder.CreateTable(
                name: "tb_m_account_divisions",
                columns: table => new
                {
                    guid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    account_guid = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    division_guid = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    modified_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    position = table.Column<string>(type: "nvarchar(120)", nullable: false)
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

            migrationBuilder.CreateIndex(
                name: "IX_tb_m_accounts_email",
                table: "tb_m_accounts",
                column: "email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_tb_tr_progresses_account_guid",
                table: "tb_tr_progresses",
                column: "account_guid");

            migrationBuilder.CreateIndex(
                name: "IX_tb_m_account_divisions_account_guid",
                table: "tb_m_account_divisions",
                column: "account_guid");

            migrationBuilder.CreateIndex(
                name: "IX_tb_m_account_divisions_division_guid",
                table: "tb_m_account_divisions",
                column: "division_guid");

            migrationBuilder.AddForeignKey(
                name: "FK_tb_tr_assign_maps_tb_m_assignemts_assignment_guid",
                table: "tb_tr_assign_maps",
                column: "assignment_guid",
                principalTable: "tb_m_assignemts",
                principalColumn: "guid");

            migrationBuilder.AddForeignKey(
                name: "FK_tb_tr_progresses_tb_m_accounts_account_guid",
                table: "tb_tr_progresses",
                column: "account_guid",
                principalTable: "tb_m_accounts",
                principalColumn: "guid",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_tb_tr_progresses_tb_m_assignemts_assignment_guid",
                table: "tb_tr_progresses",
                column: "assignment_guid",
                principalTable: "tb_m_assignemts",
                principalColumn: "guid",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
