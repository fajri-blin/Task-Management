using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Task_Management.Migrations
{
    public partial class AddingAdditionalTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tb_m_additional",
                columns: table => new
                {
                    guid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    progress_guid = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    filename = table.Column<string>(type: "nvarchar(255)", nullable: false),
                    filedata = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    filetype = table.Column<string>(type: "nvarchar(5)", nullable: false),
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

            migrationBuilder.CreateIndex(
                name: "IX_tb_m_additional_progress_guid",
                table: "tb_m_additional",
                column: "progress_guid");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tb_m_additional");
        }
    }
}
