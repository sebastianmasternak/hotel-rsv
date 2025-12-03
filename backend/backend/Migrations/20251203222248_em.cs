using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class em : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Id", "Login", "Name", "PasswordHash", "PasswordSalt" },
                values: new object[,]
                {
                    { 1, "maras", "Marek Papszun", new byte[] { 188, 50, 133, 34, 158, 138, 133, 214, 249, 242, 76, 74, 155, 16, 186, 68, 217, 212, 65, 181, 254, 247, 27, 66, 56, 139, 153, 64, 122, 27, 191, 50 }, new byte[] { 0, 164, 56, 246, 123, 9, 35, 141, 113, 240, 102, 237, 191, 155, 183, 29 } },
                    { 2, "szwagier", "Dawid Szwarga", new byte[] { 79, 50, 29, 117, 193, 135, 225, 14, 186, 254, 102, 13, 215, 127, 166, 239, 178, 199, 8, 189, 25, 112, 12, 250, 89, 160, 58, 60, 26, 209, 0, 79 }, new byte[] { 5, 126, 21, 48, 230, 239, 118, 169, 211, 110, 201, 203, 54, 116, 26, 192 } },
                    { 3, "jayz", "Jacek Zielinski", new byte[] { 37, 253, 231, 10, 6, 35, 123, 227, 195, 186, 67, 208, 111, 37, 212, 80, 246, 95, 231, 76, 244, 7, 155, 233, 231, 120, 161, 195, 68, 127, 81, 254 }, new byte[] { 143, 109, 164, 211, 46, 120, 204, 170, 48, 137, 151, 242, 47, 216, 68, 184 } }
                });
        }
    }
}
