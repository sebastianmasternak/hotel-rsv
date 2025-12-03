using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class emp2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "PasswordHash", "PasswordSalt" },
                values: new object[] { new byte[] { 188, 50, 133, 34, 158, 138, 133, 214, 249, 242, 76, 74, 155, 16, 186, 68, 217, 212, 65, 181, 254, 247, 27, 66, 56, 139, 153, 64, 122, 27, 191, 50 }, new byte[] { 0, 164, 56, 246, 123, 9, 35, 141, 113, 240, 102, 237, 191, 155, 183, 29 } });

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "PasswordHash", "PasswordSalt" },
                values: new object[] { new byte[] { 79, 50, 29, 117, 193, 135, 225, 14, 186, 254, 102, 13, 215, 127, 166, 239, 178, 199, 8, 189, 25, 112, 12, 250, 89, 160, 58, 60, 26, 209, 0, 79 }, new byte[] { 5, 126, 21, 48, 230, 239, 118, 169, 211, 110, 201, 203, 54, 116, 26, 192 } });

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "PasswordHash", "PasswordSalt" },
                values: new object[] { new byte[] { 37, 253, 231, 10, 6, 35, 123, 227, 195, 186, 67, 208, 111, 37, 212, 80, 246, 95, 231, 76, 244, 7, 155, 233, 231, 120, 161, 195, 68, 127, 81, 254 }, new byte[] { 143, 109, 164, 211, 46, 120, 204, 170, 48, 137, 151, 242, 47, 216, 68, 184 } });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "PasswordHash", "PasswordSalt" },
                values: new object[] { new byte[] { 70, 107, 184, 77, 119, 220, 171, 254, 59, 136, 58, 85, 57, 94, 205, 209, 110, 205, 57, 187, 104, 25, 99, 240, 176, 27, 16, 116, 39, 196, 189, 247 }, new byte[] { 69, 113, 94, 107, 236, 248, 111, 100, 41, 66, 30, 111, 185, 212, 152, 34 } });

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "PasswordHash", "PasswordSalt" },
                values: new object[] { new byte[] { 171, 234, 9, 159, 172, 132, 88, 182, 159, 24, 195, 216, 229, 215, 29, 39, 12, 14, 8, 195, 197, 59, 118, 169, 187, 228, 4, 39, 196, 123, 70, 250 }, new byte[] { 52, 109, 242, 45, 63, 208, 110, 93, 9, 152, 78, 149, 24, 219, 117, 19 } });

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "PasswordHash", "PasswordSalt" },
                values: new object[] { new byte[] { 228, 227, 201, 131, 19, 64, 156, 172, 119, 113, 227, 105, 210, 133, 56, 145, 38, 154, 11, 199, 65, 39, 239, 225, 253, 47, 27, 166, 166, 132, 178, 213 }, new byte[] { 71, 10, 176, 193, 36, 5, 79, 107, 46, 23, 143, 96, 22, 49, 169, 163 } });
        }
    }
}
