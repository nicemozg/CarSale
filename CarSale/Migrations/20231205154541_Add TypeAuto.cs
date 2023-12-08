using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CarSale.Migrations
{
    /// <inheritdoc />
    public partial class AddTypeAuto : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TypeAutoId",
                table: "Products",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "TypeAutos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    TypeName = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TypeAutos", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Products_TypeAutoId",
                table: "Products",
                column: "TypeAutoId");

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
                name: "FK_Products_TypeAutos_TypeAutoId",
                table: "Products");

            migrationBuilder.DropTable(
                name: "TypeAutos");

            migrationBuilder.DropIndex(
                name: "IX_Products_TypeAutoId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "TypeAutoId",
                table: "Products");
        }
    }
}
