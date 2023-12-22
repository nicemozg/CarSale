using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CarSale.Migrations
{
    /// <inheritdoc />
    public partial class AddFilterTruck : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Filters");

            migrationBuilder.CreateTable(
                name: "FilterCars",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CookieId = table.Column<string>(type: "TEXT", nullable: false),
                    CategoryId = table.Column<int>(type: "INTEGER", nullable: true),
                    BrandId = table.Column<int>(type: "INTEGER", nullable: true),
                    MotorId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FilterCars", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FilterTrucks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CookieId = table.Column<string>(type: "TEXT", nullable: false),
                    CategoryId = table.Column<int>(type: "INTEGER", nullable: true),
                    BrandId = table.Column<int>(type: "INTEGER", nullable: true),
                    MotorId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FilterTrucks", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FilterCars");

            migrationBuilder.DropTable(
                name: "FilterTrucks");

            migrationBuilder.CreateTable(
                name: "Filters",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    BrandId = table.Column<int>(type: "INTEGER", nullable: true),
                    CategoryId = table.Column<int>(type: "INTEGER", nullable: true),
                    CookieId = table.Column<string>(type: "TEXT", nullable: false),
                    MotorId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Filters", x => x.Id);
                });
        }
    }
}
