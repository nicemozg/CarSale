using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CarSale.Migrations
{
    /// <inheritdoc />
    public partial class AddColorAuto : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "Products");

            migrationBuilder.AddColumn<int>(
                name: "ColorAutoId",
                table: "Products",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "ColorsAutos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ColorName = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ColorsAutos", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Products_ColorAutoId",
                table: "Products",
                column: "ColorAutoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_ColorsAutos_ColorAutoId",
                table: "Products",
                column: "ColorAutoId",
                principalTable: "ColorsAutos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_ColorsAutos_ColorAutoId",
                table: "Products");

            migrationBuilder.DropTable(
                name: "ColorsAutos");

            migrationBuilder.DropIndex(
                name: "IX_Products_ColorAutoId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "ColorAutoId",
                table: "Products");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Products",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }
    }
}
