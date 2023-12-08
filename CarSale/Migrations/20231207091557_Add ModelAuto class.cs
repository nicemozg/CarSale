using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CarSale.Migrations
{
    /// <inheritdoc />
    public partial class AddModelAutoclass : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_TypeAutos_ModelAutoId",
                table: "Products");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_ModelAutos_ModelAutoId",
                table: "Products",
                column: "ModelAutoId",
                principalTable: "ModelAutos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_ModelAutos_ModelAutoId",
                table: "Products");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_TypeAutos_ModelAutoId",
                table: "Products",
                column: "ModelAutoId",
                principalTable: "TypeAutos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
