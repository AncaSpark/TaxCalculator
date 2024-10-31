using Microsoft.EntityFrameworkCore.Migrations;

namespace TaxCalculator.Infrastructure.Data.Migrations
{
    public partial class InitialCreate : Migration
    {
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

            migrationBuilder.CreateIndex(
                name: "IX_TaxBands_LowerLimit",
                table: "TaxBands",
                column: "LowerLimit");

            // Seed initial data
            migrationBuilder.InsertData(
                table: "TaxBands",
                columns: new[] { "Id", "Name", "LowerLimit", "UpperLimit", "TaxRate" },
                values: new object[,]
                {
                    { 1, "Tax Band A", 0m, 5000m, 0 },
                    { 2, "Tax Band B", 5000m, 20000m, 20 },
                    { 3, "Tax Band C", 20000m, null, 40 }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "TaxBands");
        }
    }
}

