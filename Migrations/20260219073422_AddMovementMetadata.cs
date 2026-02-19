using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AcmvInventory.Backend.Migrations
{
    /// <inheritdoc />
    public partial class AddMovementMetadata : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ApprovedBy",
                table: "Transactions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DestinationLocation",
                table: "Transactions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DocumentNo",
                table: "Transactions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DocumentType",
                table: "Transactions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ReasonCode",
                table: "Transactions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SourceLocation",
                table: "Transactions",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ApprovedBy",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "DestinationLocation",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "DocumentNo",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "DocumentType",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "ReasonCode",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "SourceLocation",
                table: "Transactions");
        }
    }
}
