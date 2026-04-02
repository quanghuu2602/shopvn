using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SHOPVN.Migrations
{
    /// <inheritdoc />
    public partial class InitAgain : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 30, 16, 9, 28, 159, DateTimeKind.Utc).AddTicks(9561));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 30, 16, 9, 28, 159, DateTimeKind.Utc).AddTicks(9575));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 30, 16, 9, 28, 159, DateTimeKind.Utc).AddTicks(9579));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 30, 16, 9, 28, 159, DateTimeKind.Utc).AddTicks(9583));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 30, 16, 9, 28, 159, DateTimeKind.Utc).AddTicks(9585));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 6,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 30, 16, 9, 28, 159, DateTimeKind.Utc).AddTicks(9588));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 30, 15, 54, 51, 185, DateTimeKind.Utc).AddTicks(8181));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 30, 15, 54, 51, 185, DateTimeKind.Utc).AddTicks(8195));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 30, 15, 54, 51, 185, DateTimeKind.Utc).AddTicks(8199));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 30, 15, 54, 51, 185, DateTimeKind.Utc).AddTicks(8202));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 30, 15, 54, 51, 185, DateTimeKind.Utc).AddTicks(8205));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 6,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 30, 15, 54, 51, 185, DateTimeKind.Utc).AddTicks(8207));
        }
    }
}
