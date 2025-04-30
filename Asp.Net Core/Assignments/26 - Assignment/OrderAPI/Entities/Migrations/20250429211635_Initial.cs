using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Entities.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    OrderId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OrderNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CustomerName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    OrderDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TotalAmount = table.Column<decimal>(type: "decimal(18,0)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.OrderId);
                });

            migrationBuilder.CreateTable(
                name: "OrderItems",
                columns: table => new
                {
                    OrderItemId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OrderId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProductName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderItems", x => x.OrderItemId);
                    table.ForeignKey(
                        name: "FK_OrderItems_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "OrderId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Orders",
                columns: new[] { "OrderId", "CustomerName", "OrderDate", "OrderNumber", "TotalAmount" },
                values: new object[,]
                {
                    { new Guid("735886c0-faf3-49ca-9776-8a20b756f1cb"), "Jane Smith", new DateTime(2025, 2, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "ORD002", 225.8m },
                    { new Guid("f4816224-70d6-4491-ac52-34f298ace16f"), "John Doe", new DateTime(2025, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "ORD001", 66.5m }
                });

            migrationBuilder.InsertData(
                table: "OrderItems",
                columns: new[] { "OrderItemId", "OrderId", "ProductName", "Quantity" },
                values: new object[,]
                {
                    { new Guid("24d71ac2-0a9c-4914-9fd3-13bc25d42694"), new Guid("735886c0-faf3-49ca-9776-8a20b756f1cb"), "Product C", 7 },
                    { new Guid("2e27b6a4-469d-4d7f-8b8b-54af129675fd"), new Guid("f4816224-70d6-4491-ac52-34f298ace16f"), "Product B", 3 },
                    { new Guid("ac90b8bc-349d-43fd-87a6-6a7ed8057697"), new Guid("735886c0-faf3-49ca-9776-8a20b756f1cb"), "Product D", 4 },
                    { new Guid("d20882df-7fca-4ee8-88bb-37d2fc75e63f"), new Guid("f4816224-70d6-4491-ac52-34f298ace16f"), "Product A", 2 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_OrderId",
                table: "OrderItems",
                column: "OrderId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrderItems");

            migrationBuilder.DropTable(
                name: "Orders");
        }
    }
}
