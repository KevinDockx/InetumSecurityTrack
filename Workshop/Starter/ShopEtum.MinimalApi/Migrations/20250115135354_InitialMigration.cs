using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable 

#pragma warning disable IDE0300 
#pragma warning disable IDE0079
#pragma warning disable CA1861

namespace ShopEtum.MinimalApi.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Carts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerIdentifier = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Carts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    CartId = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Orders_Carts_CartId",
                        column: x => x.CartId,
                        principalTable: "Carts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Carts",
                columns: new[] { "Id", "CreatedDate", "CustomerIdentifier", "ModifiedDate" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "kevin.dockx@gmail.com", null },
                    { 2, new DateTime(2025, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "jerry.vanechelpoel@inetum-realdolmen.world", null },
                    { 3, new DateTime(2025, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "dirk.slembrouck@inetum-realdolmen.world", null },
                    { 4, new DateTime(2025, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "kevin.dockx@gmail.com", null }
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "CreatedDate", "Description", "ModifiedDate", "Name", "Price" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "A horror novel by Stephen King", null, "The Shining", 19.99m },
                    { 2, new DateTime(2025, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "A horror novel by Stephen King", null, "It", 24.99m },
                    { 3, new DateTime(2025, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "A psychological horror novel by Stephen King", null, "Misery", 14.99m },
                    { 4, new DateTime(2025, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "A horror novel by Stephen King", null, "Carrie", 12.99m },
                    { 5, new DateTime(2025, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "A dystopian novel by George Orwell", null, "1984", 9.99m },
                    { 6, new DateTime(2025, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "A dystopian novel by Aldous Huxley", null, "Brave New World", 8.99m },
                    { 7, new DateTime(2025, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "A novel by Harper Lee", null, "To Kill a Mockingbird", 7.99m },
                    { 8, new DateTime(2025, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "A novel by F. Scott Fitzgerald", null, "The Great Gatsby", 10.99m },
                    { 9, new DateTime(2025, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "A novel by Herman Melville", null, "Moby Dick", 11.99m },
                    { 10, new DateTime(2025, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "A novel by Leo Tolstoy", null, "War and Peace", 13.99m },
                    { 11, new DateTime(2025, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "Vinyl record by Fred Again..", null, "Fred Again.. - Actual Life 3", 29.99m },
                    { 12, new DateTime(2025, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "Vinyl record by Jamie XX", null, "Jamie XX - In Colour", 27.99m },
                    { 13, new DateTime(2025, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "Vinyl record by The Beatles", null, "Abbey Road - The Beatles", 25.99m },
                    { 14, new DateTime(2025, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "Vinyl record by Pink Floyd", null, "Dark Side of the Moon - Pink Floyd", 26.99m },
                    { 15, new DateTime(2025, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "Vinyl record by Michael Jackson", null, "Thriller - Michael Jackson", 24.99m },
                    { 16, new DateTime(2025, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "Vinyl record by AC/DC", null, "Back in Black - AC/DC", 23.99m },
                    { 17, new DateTime(2025, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "Vinyl record by Fleetwood Mac", null, "Rumours - Fleetwood Mac", 22.99m },
                    { 18, new DateTime(2025, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "Vinyl record by Eagles", null, "Hotel California - Eagles", 21.99m },
                    { 19, new DateTime(2025, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "Vinyl record by Pink Floyd", null, "The Wall - Pink Floyd", 20.99m },
                    { 20, new DateTime(2025, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "Vinyl record by Led Zeppelin", null, "Led Zeppelin IV - Led Zeppelin", 19.99m }
                });

            migrationBuilder.InsertData(
                table: "Orders",
                columns: new[] { "Id", "CartId", "CreatedDate", "ModifiedDate", "ProductId", "Quantity" },
                values: new object[,]
                {
                    { 1, 1, new DateTime(2025, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 1, 1 },
                    { 2, 1, new DateTime(2025, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 2, 2 },
                    { 3, 1, new DateTime(2025, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 11, 1 },
                    { 4, 2, new DateTime(2025, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 3, 1 },
                    { 5, 2, new DateTime(2025, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 12, 2 },
                    { 6, 3, new DateTime(2025, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 4, 1 },
                    { 7, 3, new DateTime(2025, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 13, 1 },
                    { 8, 3, new DateTime(2025, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 14, 1 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Orders_CartId",
                table: "Orders",
                column: "CartId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "Carts");
        }
    }
}
