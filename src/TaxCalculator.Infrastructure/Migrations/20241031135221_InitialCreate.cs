using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace TaxCalculator.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TaxBands",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    LowerLimit = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    UpperLimit = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    TaxRate = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaxBands", x => x.Id);
                    table.CheckConstraint("CK_TaxBand_Limits", "[LowerLimit] >= 0 AND ([UpperLimit] IS NULL OR [UpperLimit] > [LowerLimit])");
                });

            migrationBuilder.InsertData(
                table: "TaxBands",
                columns: new[] { "Id", "LowerLimit", "Name", "TaxRate", "UpperLimit" },
                values: new object[,]
                {
                    { 1, 0m, "Tax Band A", 0, 5000m },
                    { 2, 5000m, "Tax Band B", 20, 20000m },
                    { 3, 20000m, "Tax Band C", 40, null }
                });

            migrationBuilder.CreateIndex(
                name: "IX_TaxBands_LowerLimit",
                table: "TaxBands",
                column: "LowerLimit");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TaxBands");
        }
    }
}
