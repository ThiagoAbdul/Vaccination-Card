using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RenameSearchableColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SearchableColumn",
                table: "Person",
                newName: "NameSearchableColumn");

            migrationBuilder.RenameIndex(
                name: "IX_Person_SearchableColumn",
                table: "Person",
                newName: "IX_Person_NameSearchableColumn");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "NameSearchableColumn",
                table: "Person",
                newName: "SearchableColumn");

            migrationBuilder.RenameIndex(
                name: "IX_Person_NameSearchableColumn",
                table: "Person",
                newName: "IX_Person_SearchableColumn");
        }
    }
}
