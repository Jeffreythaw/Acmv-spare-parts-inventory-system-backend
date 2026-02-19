using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AcmvInventory.Backend.Migrations
{
    /// <inheritdoc />
    public partial class PrefixAllTablesWithTks : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderScheduleLines_OrderSchedules_OrderScheduleId",
                table: "OrderScheduleLines");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderScheduleLines_TKS_Inventory_InventoryId",
                table: "OrderScheduleLines");

            migrationBuilder.DropForeignKey(
                name: "FK_POLines_PurchaseOrders_PurchaseOrderId",
                table: "POLines");

            migrationBuilder.DropForeignKey(
                name: "FK_POLines_TKS_Inventory_InventoryId",
                table: "POLines");

            migrationBuilder.DropForeignKey(
                name: "FK_PRLines_PurchaseRequests_PurchaseRequestId",
                table: "PRLines");

            migrationBuilder.DropForeignKey(
                name: "FK_PRLines_TKS_Inventory_InventoryId",
                table: "PRLines");

            migrationBuilder.DropForeignKey(
                name: "FK_PurchaseOrders_Suppliers_SupplierId",
                table: "PurchaseOrders");

            migrationBuilder.DropForeignKey(
                name: "FK_TransactionLines_TKS_Inventory_InventoryId",
                table: "TransactionLines");

            migrationBuilder.DropForeignKey(
                name: "FK_TransactionLines_Transactions_StockTransactionId",
                table: "TransactionLines");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Transactions",
                table: "Transactions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TransactionLines",
                table: "TransactionLines");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Suppliers",
                table: "Suppliers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PurchaseRequests",
                table: "PurchaseRequests");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PurchaseOrders",
                table: "PurchaseOrders");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PRLines",
                table: "PRLines");

            migrationBuilder.DropPrimaryKey(
                name: "PK_POLines",
                table: "POLines");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OrderSchedules",
                table: "OrderSchedules");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OrderScheduleLines",
                table: "OrderScheduleLines");

            migrationBuilder.RenameTable(
                name: "Transactions",
                newName: "TKS_Transactions",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "TransactionLines",
                newName: "TKS_TransactionLines",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "Suppliers",
                newName: "TKS_Suppliers",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "PurchaseRequests",
                newName: "TKS_PurchaseRequests",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "PurchaseOrders",
                newName: "TKS_PurchaseOrders",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "PRLines",
                newName: "TKS_PRLines",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "POLines",
                newName: "TKS_POLines",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "OrderSchedules",
                newName: "TKS_OrderSchedules",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "OrderScheduleLines",
                newName: "TKS_OrderScheduleLines",
                newSchema: "dbo");

            migrationBuilder.RenameIndex(
                name: "IX_TransactionLines_StockTransactionId",
                schema: "dbo",
                table: "TKS_TransactionLines",
                newName: "IX_TKS_TransactionLines_StockTransactionId");

            migrationBuilder.RenameIndex(
                name: "IX_TransactionLines_InventoryId",
                schema: "dbo",
                table: "TKS_TransactionLines",
                newName: "IX_TKS_TransactionLines_InventoryId");

            migrationBuilder.RenameIndex(
                name: "IX_PurchaseOrders_SupplierId",
                schema: "dbo",
                table: "TKS_PurchaseOrders",
                newName: "IX_TKS_PurchaseOrders_SupplierId");

            migrationBuilder.RenameIndex(
                name: "IX_PRLines_PurchaseRequestId",
                schema: "dbo",
                table: "TKS_PRLines",
                newName: "IX_TKS_PRLines_PurchaseRequestId");

            migrationBuilder.RenameIndex(
                name: "IX_PRLines_InventoryId",
                schema: "dbo",
                table: "TKS_PRLines",
                newName: "IX_TKS_PRLines_InventoryId");

            migrationBuilder.RenameIndex(
                name: "IX_POLines_PurchaseOrderId",
                schema: "dbo",
                table: "TKS_POLines",
                newName: "IX_TKS_POLines_PurchaseOrderId");

            migrationBuilder.RenameIndex(
                name: "IX_POLines_InventoryId",
                schema: "dbo",
                table: "TKS_POLines",
                newName: "IX_TKS_POLines_InventoryId");

            migrationBuilder.RenameIndex(
                name: "IX_OrderScheduleLines_OrderScheduleId",
                schema: "dbo",
                table: "TKS_OrderScheduleLines",
                newName: "IX_TKS_OrderScheduleLines_OrderScheduleId");

            migrationBuilder.RenameIndex(
                name: "IX_OrderScheduleLines_InventoryId",
                schema: "dbo",
                table: "TKS_OrderScheduleLines",
                newName: "IX_TKS_OrderScheduleLines_InventoryId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TKS_Transactions",
                schema: "dbo",
                table: "TKS_Transactions",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TKS_TransactionLines",
                schema: "dbo",
                table: "TKS_TransactionLines",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TKS_Suppliers",
                schema: "dbo",
                table: "TKS_Suppliers",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TKS_PurchaseRequests",
                schema: "dbo",
                table: "TKS_PurchaseRequests",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TKS_PurchaseOrders",
                schema: "dbo",
                table: "TKS_PurchaseOrders",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TKS_PRLines",
                schema: "dbo",
                table: "TKS_PRLines",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TKS_POLines",
                schema: "dbo",
                table: "TKS_POLines",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TKS_OrderSchedules",
                schema: "dbo",
                table: "TKS_OrderSchedules",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TKS_OrderScheduleLines",
                schema: "dbo",
                table: "TKS_OrderScheduleLines",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TKS_OrderScheduleLines_TKS_Inventory_InventoryId",
                schema: "dbo",
                table: "TKS_OrderScheduleLines",
                column: "InventoryId",
                principalSchema: "dbo",
                principalTable: "TKS_Inventory",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TKS_OrderScheduleLines_TKS_OrderSchedules_OrderScheduleId",
                schema: "dbo",
                table: "TKS_OrderScheduleLines",
                column: "OrderScheduleId",
                principalSchema: "dbo",
                principalTable: "TKS_OrderSchedules",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TKS_POLines_TKS_Inventory_InventoryId",
                schema: "dbo",
                table: "TKS_POLines",
                column: "InventoryId",
                principalSchema: "dbo",
                principalTable: "TKS_Inventory",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TKS_POLines_TKS_PurchaseOrders_PurchaseOrderId",
                schema: "dbo",
                table: "TKS_POLines",
                column: "PurchaseOrderId",
                principalSchema: "dbo",
                principalTable: "TKS_PurchaseOrders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TKS_PRLines_TKS_Inventory_InventoryId",
                schema: "dbo",
                table: "TKS_PRLines",
                column: "InventoryId",
                principalSchema: "dbo",
                principalTable: "TKS_Inventory",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TKS_PRLines_TKS_PurchaseRequests_PurchaseRequestId",
                schema: "dbo",
                table: "TKS_PRLines",
                column: "PurchaseRequestId",
                principalSchema: "dbo",
                principalTable: "TKS_PurchaseRequests",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TKS_PurchaseOrders_TKS_Suppliers_SupplierId",
                schema: "dbo",
                table: "TKS_PurchaseOrders",
                column: "SupplierId",
                principalSchema: "dbo",
                principalTable: "TKS_Suppliers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TKS_TransactionLines_TKS_Inventory_InventoryId",
                schema: "dbo",
                table: "TKS_TransactionLines",
                column: "InventoryId",
                principalSchema: "dbo",
                principalTable: "TKS_Inventory",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TKS_TransactionLines_TKS_Transactions_StockTransactionId",
                schema: "dbo",
                table: "TKS_TransactionLines",
                column: "StockTransactionId",
                principalSchema: "dbo",
                principalTable: "TKS_Transactions",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TKS_OrderScheduleLines_TKS_Inventory_InventoryId",
                schema: "dbo",
                table: "TKS_OrderScheduleLines");

            migrationBuilder.DropForeignKey(
                name: "FK_TKS_OrderScheduleLines_TKS_OrderSchedules_OrderScheduleId",
                schema: "dbo",
                table: "TKS_OrderScheduleLines");

            migrationBuilder.DropForeignKey(
                name: "FK_TKS_POLines_TKS_Inventory_InventoryId",
                schema: "dbo",
                table: "TKS_POLines");

            migrationBuilder.DropForeignKey(
                name: "FK_TKS_POLines_TKS_PurchaseOrders_PurchaseOrderId",
                schema: "dbo",
                table: "TKS_POLines");

            migrationBuilder.DropForeignKey(
                name: "FK_TKS_PRLines_TKS_Inventory_InventoryId",
                schema: "dbo",
                table: "TKS_PRLines");

            migrationBuilder.DropForeignKey(
                name: "FK_TKS_PRLines_TKS_PurchaseRequests_PurchaseRequestId",
                schema: "dbo",
                table: "TKS_PRLines");

            migrationBuilder.DropForeignKey(
                name: "FK_TKS_PurchaseOrders_TKS_Suppliers_SupplierId",
                schema: "dbo",
                table: "TKS_PurchaseOrders");

            migrationBuilder.DropForeignKey(
                name: "FK_TKS_TransactionLines_TKS_Inventory_InventoryId",
                schema: "dbo",
                table: "TKS_TransactionLines");

            migrationBuilder.DropForeignKey(
                name: "FK_TKS_TransactionLines_TKS_Transactions_StockTransactionId",
                schema: "dbo",
                table: "TKS_TransactionLines");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TKS_Transactions",
                schema: "dbo",
                table: "TKS_Transactions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TKS_TransactionLines",
                schema: "dbo",
                table: "TKS_TransactionLines");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TKS_Suppliers",
                schema: "dbo",
                table: "TKS_Suppliers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TKS_PurchaseRequests",
                schema: "dbo",
                table: "TKS_PurchaseRequests");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TKS_PurchaseOrders",
                schema: "dbo",
                table: "TKS_PurchaseOrders");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TKS_PRLines",
                schema: "dbo",
                table: "TKS_PRLines");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TKS_POLines",
                schema: "dbo",
                table: "TKS_POLines");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TKS_OrderSchedules",
                schema: "dbo",
                table: "TKS_OrderSchedules");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TKS_OrderScheduleLines",
                schema: "dbo",
                table: "TKS_OrderScheduleLines");

            migrationBuilder.RenameTable(
                name: "TKS_Transactions",
                schema: "dbo",
                newName: "Transactions");

            migrationBuilder.RenameTable(
                name: "TKS_TransactionLines",
                schema: "dbo",
                newName: "TransactionLines");

            migrationBuilder.RenameTable(
                name: "TKS_Suppliers",
                schema: "dbo",
                newName: "Suppliers");

            migrationBuilder.RenameTable(
                name: "TKS_PurchaseRequests",
                schema: "dbo",
                newName: "PurchaseRequests");

            migrationBuilder.RenameTable(
                name: "TKS_PurchaseOrders",
                schema: "dbo",
                newName: "PurchaseOrders");

            migrationBuilder.RenameTable(
                name: "TKS_PRLines",
                schema: "dbo",
                newName: "PRLines");

            migrationBuilder.RenameTable(
                name: "TKS_POLines",
                schema: "dbo",
                newName: "POLines");

            migrationBuilder.RenameTable(
                name: "TKS_OrderSchedules",
                schema: "dbo",
                newName: "OrderSchedules");

            migrationBuilder.RenameTable(
                name: "TKS_OrderScheduleLines",
                schema: "dbo",
                newName: "OrderScheduleLines");

            migrationBuilder.RenameIndex(
                name: "IX_TKS_TransactionLines_StockTransactionId",
                table: "TransactionLines",
                newName: "IX_TransactionLines_StockTransactionId");

            migrationBuilder.RenameIndex(
                name: "IX_TKS_TransactionLines_InventoryId",
                table: "TransactionLines",
                newName: "IX_TransactionLines_InventoryId");

            migrationBuilder.RenameIndex(
                name: "IX_TKS_PurchaseOrders_SupplierId",
                table: "PurchaseOrders",
                newName: "IX_PurchaseOrders_SupplierId");

            migrationBuilder.RenameIndex(
                name: "IX_TKS_PRLines_PurchaseRequestId",
                table: "PRLines",
                newName: "IX_PRLines_PurchaseRequestId");

            migrationBuilder.RenameIndex(
                name: "IX_TKS_PRLines_InventoryId",
                table: "PRLines",
                newName: "IX_PRLines_InventoryId");

            migrationBuilder.RenameIndex(
                name: "IX_TKS_POLines_PurchaseOrderId",
                table: "POLines",
                newName: "IX_POLines_PurchaseOrderId");

            migrationBuilder.RenameIndex(
                name: "IX_TKS_POLines_InventoryId",
                table: "POLines",
                newName: "IX_POLines_InventoryId");

            migrationBuilder.RenameIndex(
                name: "IX_TKS_OrderScheduleLines_OrderScheduleId",
                table: "OrderScheduleLines",
                newName: "IX_OrderScheduleLines_OrderScheduleId");

            migrationBuilder.RenameIndex(
                name: "IX_TKS_OrderScheduleLines_InventoryId",
                table: "OrderScheduleLines",
                newName: "IX_OrderScheduleLines_InventoryId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Transactions",
                table: "Transactions",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TransactionLines",
                table: "TransactionLines",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Suppliers",
                table: "Suppliers",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PurchaseRequests",
                table: "PurchaseRequests",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PurchaseOrders",
                table: "PurchaseOrders",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PRLines",
                table: "PRLines",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_POLines",
                table: "POLines",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OrderSchedules",
                table: "OrderSchedules",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OrderScheduleLines",
                table: "OrderScheduleLines",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderScheduleLines_OrderSchedules_OrderScheduleId",
                table: "OrderScheduleLines",
                column: "OrderScheduleId",
                principalTable: "OrderSchedules",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderScheduleLines_TKS_Inventory_InventoryId",
                table: "OrderScheduleLines",
                column: "InventoryId",
                principalSchema: "dbo",
                principalTable: "TKS_Inventory",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_POLines_PurchaseOrders_PurchaseOrderId",
                table: "POLines",
                column: "PurchaseOrderId",
                principalTable: "PurchaseOrders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_POLines_TKS_Inventory_InventoryId",
                table: "POLines",
                column: "InventoryId",
                principalSchema: "dbo",
                principalTable: "TKS_Inventory",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PRLines_PurchaseRequests_PurchaseRequestId",
                table: "PRLines",
                column: "PurchaseRequestId",
                principalTable: "PurchaseRequests",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PRLines_TKS_Inventory_InventoryId",
                table: "PRLines",
                column: "InventoryId",
                principalSchema: "dbo",
                principalTable: "TKS_Inventory",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PurchaseOrders_Suppliers_SupplierId",
                table: "PurchaseOrders",
                column: "SupplierId",
                principalTable: "Suppliers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TransactionLines_TKS_Inventory_InventoryId",
                table: "TransactionLines",
                column: "InventoryId",
                principalSchema: "dbo",
                principalTable: "TKS_Inventory",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TransactionLines_Transactions_StockTransactionId",
                table: "TransactionLines",
                column: "StockTransactionId",
                principalTable: "Transactions",
                principalColumn: "Id");
        }
    }
}
