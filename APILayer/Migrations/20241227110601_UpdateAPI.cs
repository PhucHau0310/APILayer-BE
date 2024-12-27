using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace APILayer.Migrations
{
    /// <inheritdoc />
    public partial class UpdateAPI : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IntegrationGuide",
                table: "APIDocumentation",
                newName: "LogoUrl");

            migrationBuilder.RenameColumn(
                name: "DocContent",
                table: "APIDocumentation",
                newName: "DocumentUrl");

            migrationBuilder.RenameColumn(
                name: "TechnicalSpecs",
                table: "API",
                newName: "PricingUrl");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "LogoUrl",
                table: "APIDocumentation",
                newName: "IntegrationGuide");

            migrationBuilder.RenameColumn(
                name: "DocumentUrl",
                table: "APIDocumentation",
                newName: "DocContent");

            migrationBuilder.RenameColumn(
                name: "PricingUrl",
                table: "API",
                newName: "TechnicalSpecs");
        }
    }
}
