using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CarSale.Migrations
{
    /// <inheritdoc />
    public partial class NotNull : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_TypeAutosMototors_TypeMotorId",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_TypeAutos_TypeAutoId",
                table: "Products");

            migrationBuilder.AlterColumn<int>(
                name: "TypeMototrId",
                table: "Products",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "TypeMotorId",
                table: "Products",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

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
                name: "FK_Products_TypeAutosMototors_TypeMotorId",
                table: "Products",
                column: "TypeMotorId",
                principalTable: "TypeAutosMototors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_TypeAutos_TypeAutoId",
                table: "Products",
                column: "TypeAutoId",
                principalTable: "TypeAutos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
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

            migrationBuilder.AlterColumn<int>(
                name: "TypeMototrId",
                table: "Products",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<int>(
                name: "TypeMotorId",
                table: "Products",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<int>(
                name: "TypeAutoId",
                table: "Products",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

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
    }
}
