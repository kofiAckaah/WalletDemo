using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class Updatewallet : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Wallets_AspNetUsers_WalletOwnerId1",
                table: "Wallets");

            migrationBuilder.DropIndex(
                name: "IX_Wallets_WalletOwnerId1",
                table: "Wallets");

            migrationBuilder.DropColumn(
                name: "WalletOwnerId1",
                table: "Wallets");

            migrationBuilder.AlterColumn<Guid>(
                name: "WalletOwnerId",
                table: "Wallets",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NumberHash",
                table: "Wallets",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Wallets_WalletOwnerId",
                table: "Wallets",
                column: "WalletOwnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Wallets_AspNetUsers_WalletOwnerId",
                table: "Wallets",
                column: "WalletOwnerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Wallets_AspNetUsers_WalletOwnerId",
                table: "Wallets");

            migrationBuilder.DropIndex(
                name: "IX_Wallets_WalletOwnerId",
                table: "Wallets");

            migrationBuilder.DropColumn(
                name: "NumberHash",
                table: "Wallets");

            migrationBuilder.AlterColumn<string>(
                name: "WalletOwnerId",
                table: "Wallets",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddColumn<Guid>(
                name: "WalletOwnerId1",
                table: "Wallets",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Wallets_WalletOwnerId1",
                table: "Wallets",
                column: "WalletOwnerId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Wallets_AspNetUsers_WalletOwnerId1",
                table: "Wallets",
                column: "WalletOwnerId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
