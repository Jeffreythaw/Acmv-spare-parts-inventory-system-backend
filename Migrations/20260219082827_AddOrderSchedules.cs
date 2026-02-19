using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AcmvInventory.Backend.Migrations
{
    /// <inheritdoc />
    public partial class AddOrderSchedules : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "OrderSchedules",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ScheduledDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SupplierId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Remark = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastUpdated = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderSchedules", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OrderScheduleLines",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InventoryId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Qty = table.Column<int>(type: "int", nullable: false),
                    OrderScheduleId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderScheduleLines", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderScheduleLines_OrderSchedules_OrderScheduleId",
                        column: x => x.OrderScheduleId,
                        principalTable: "OrderSchedules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderScheduleLines_TKS_Inventory_InventoryId",
                        column: x => x.InventoryId,
                        principalSchema: "dbo",
                        principalTable: "TKS_Inventory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OrderScheduleLines_InventoryId",
                table: "OrderScheduleLines",
                column: "InventoryId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderScheduleLines_OrderScheduleId",
                table: "OrderScheduleLines",
                column: "OrderScheduleId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrderScheduleLines");

            migrationBuilder.DropTable(
                name: "OrderSchedules");
        }
    }
}
