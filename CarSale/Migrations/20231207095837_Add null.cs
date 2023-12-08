using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CarSale.Migrations
{
    /// <inheritdoc />
    public partial class Addnull : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_TypeAutos_TypeAutoId",
                table: "Products");

            migrationBuilder.AlterColumn<int>(
                name: "TypeAutoId",
                table: "Products",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AddColumn<int>(
                name: "TypeMotorId",
                table: "Products",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TypeMototrId",
                table: "Products",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Products_TypeMotorId",
                table: "Products",
                column: "TypeMotorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_TypeAutosMototors_TypeMotorId",
                table: "Products",
                column: "TypeMotorId",
                principalTable: "TypeAutosMototors",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_TypeAutos_TypeAutoId",
                table: "Products",
                column: "TypeAutoId",
                principalTable: "TypeAutos",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_TypeAutosMototors_TypeMotorId",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_TypeAutos_TypeAutoId",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_TypeMotorId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "TypeMotorId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "TypeMototrId",
                table: "Products");

            migrationBuilder.AlterColumn<int>(
                name: "TypeAutoId",
                table: "Products",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_TypeAutos_TypeAutoId",
                table: "Products",
                column: "TypeAutoId",
                principalTable: "TypeAutos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
