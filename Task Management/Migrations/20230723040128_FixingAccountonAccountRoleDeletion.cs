using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Task_Management.Migrations
{
    public partial class FixingAccountonAccountRoleDeletion : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tb_tr_account_roles_tb_m_accounts_account_guid",
                table: "tb_tr_account_roles");

            migrationBuilder.DropForeignKey(
                name: "FK_tb_tr_account_roles_tb_m_roles_role_guid",
                table: "tb_tr_account_roles");

            migrationBuilder.DropForeignKey(
                name: "FK_tb_tr_assign_maps_tb_m_categories_category_guid",
                table: "tb_tr_assign_maps");

            migrationBuilder.AddForeignKey(
                name: "FK_tb_tr_account_roles_tb_m_accounts_account_guid",
                table: "tb_tr_account_roles",
                column: "account_guid",
                principalTable: "tb_m_accounts",
                principalColumn: "guid",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_tb_tr_account_roles_tb_m_roles_role_guid",
                table: "tb_tr_account_roles",
                column: "role_guid",
                principalTable: "tb_m_roles",
                principalColumn: "guid",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_tb_tr_assign_maps_tb_m_categories_category_guid",
                table: "tb_tr_assign_maps",
                column: "category_guid",
                principalTable: "tb_m_categories",
                principalColumn: "guid",
                onDelete: ReferentialAction.SetNull);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tb_tr_account_roles_tb_m_accounts_account_guid",
                table: "tb_tr_account_roles");

            migrationBuilder.DropForeignKey(
                name: "FK_tb_tr_account_roles_tb_m_roles_role_guid",
                table: "tb_tr_account_roles");

            migrationBuilder.DropForeignKey(
                name: "FK_tb_tr_assign_maps_tb_m_categories_category_guid",
                table: "tb_tr_assign_maps");

            migrationBuilder.AddForeignKey(
                name: "FK_tb_tr_account_roles_tb_m_accounts_account_guid",
                table: "tb_tr_account_roles",
                column: "account_guid",
                principalTable: "tb_m_accounts",
                principalColumn: "guid");

            migrationBuilder.AddForeignKey(
                name: "FK_tb_tr_account_roles_tb_m_roles_role_guid",
                table: "tb_tr_account_roles",
                column: "role_guid",
                principalTable: "tb_m_roles",
                principalColumn: "guid",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_tb_tr_assign_maps_tb_m_categories_category_guid",
                table: "tb_tr_assign_maps",
                column: "category_guid",
                principalTable: "tb_m_categories",
                principalColumn: "guid",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
