using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Task_Management.Migrations
{
    public partial class Additional : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "filetype",
                table: "tb_m_additional");

            migrationBuilder.AlterColumn<string>(
                name: "filedata",
                table: "tb_m_additional",
                type: "nvarchar(255)",
                nullable: false,
                oldClrType: typeof(byte[]),
                oldType: "varbinary(max)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<byte[]>(
                name: "filedata",
                table: "tb_m_additional",
                type: "varbinary(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)");

            migrationBuilder.AddColumn<string>(
                name: "filetype",
                table: "tb_m_additional",
                type: "nvarchar(5)",
                nullable: false,
                defaultValue: "");
        }
    }
}
