using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class emp : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Id", "Login", "Name", "PasswordHash", "PasswordSalt" },
                values: new object[,]
                {
                    { 1, "maras", "Marek Papszun", new byte[] { 70, 107, 184, 77, 119, 220, 171, 254, 59, 136, 58, 85, 57, 94, 205, 209, 110, 205, 57, 187, 104, 25, 99, 240, 176, 27, 16, 116, 39, 196, 189, 247 }, new byte[] { 69, 113, 94, 107, 236, 248, 111, 100, 41, 66, 30, 111, 185, 212, 152, 34 } },
                    { 2, "szwagier", "Dawid Szwarga", new byte[] { 171, 234, 9, 159, 172, 132, 88, 182, 159, 24, 195, 216, 229, 215, 29, 39, 12, 14, 8, 195, 197, 59, 118, 169, 187, 228, 4, 39, 196, 123, 70, 250 }, new byte[] { 52, 109, 242, 45, 63, 208, 110, 93, 9, 152, 78, 149, 24, 219, 117, 19 } },
                    { 3, "jayz", "Jacek Zielinski", new byte[] { 228, 227, 201, 131, 19, 64, 156, 172, 119, 113, 227, 105, 210, 133, 56, 145, 38, 154, 11, 199, 65, 39, 239, 225, 253, 47, 27, 166, 166, 132, 178, 213 }, new byte[] { 71, 10, 176, 193, 36, 5, 79, 107, 46, 23, 143, 96, 22, 49, 169, 163 } }
                });

            migrationBuilder.UpdateData(
                table: "RoomTypes",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Description", "Name" },
                values: new object[] { "Basic Room with bed and bathroom", "Basic" });

            migrationBuilder.UpdateData(
                table: "RoomTypes",
                keyColumn: "Id",
                keyValue: 2,
                column: "Description",
                value: "Double bed for two guests and bathroom.");

            migrationBuilder.UpdateData(
                table: "RoomTypes",
                keyColumn: "Id",
                keyValue: 3,
                column: "Description",
                value: "Larger room with Tv and wifi.");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.UpdateData(
                table: "RoomTypes",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Description", "Name" },
                values: new object[] { "Single bed, suitable for one guest.", "Single" });

            migrationBuilder.UpdateData(
                table: "RoomTypes",
                keyColumn: "Id",
                keyValue: 2,
                column: "Description",
                value: "Double bed for two guests.");

            migrationBuilder.UpdateData(
                table: "RoomTypes",
                keyColumn: "Id",
                keyValue: 3,
                column: "Description",
                value: "Larger room with extra amenities.");
        }
    }
}
