using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CarSale.Migrations
{
    /// <inheritdoc />
    public partial class AddModelAuto : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ModelAutoId",
                table: "Products",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "ModelAutos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ModelName = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ModelAutos", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Products_ModelAutoId",
                table: "Products",
                column: "ModelAutoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_TypeAutos_ModelAutoId",
                table: "Products",
                column: "ModelAutoId",
                principalTable: "TypeAutos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_TypeAutos_ModelAutoId",
                table: "Products");

            migrationBuilder.DropTable(
                name: "ModelAutos");

            migrationBuilder.DropIndex(
                name: "IX_Products_ModelAutoId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "ModelAutoId",
                table: "Products");
        }
    }
}
