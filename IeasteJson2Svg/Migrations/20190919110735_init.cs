using Microsoft.EntityFrameworkCore.Migrations;

namespace IeasteJson2Svg.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SvgDocuments",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    DocumentName = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    DocumentPath = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SvgDocuments", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "SvgElements",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    DocumentId = table.Column<int>(nullable: false),
                    AttributeName = table.Column<string>(nullable: true),
                    AttributeInnerText = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SvgElements", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SvgDocuments");

            migrationBuilder.DropTable(
                name: "SvgElements");
        }
    }
}
