using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BulkyBook.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class SeedCompanies : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Companies",
                columns: new[] { "CompanyID", "CompanyCity", "CompanyName", "CompanyPhoneNumber", "CompanyPostalCode", "CompanyState", "CompanyStreetAddress" },
                values: new object[,]
                {
                    { 1, "Anytown", "ABC Corporation", "555-123-4567", "12345", "CA", "123 Main Street" },
                    { 2, "Otherburg", "Acme Widgets", "555-555-1212", "67890", "TX", "789 Oak Road" },
                    { 3, "Bigcity", "MegaCorp", "555-999-8888", "13579", "FL", "101 Pine Street" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Companies",
                keyColumn: "CompanyID",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Companies",
                keyColumn: "CompanyID",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Companies",
                keyColumn: "CompanyID",
                keyValue: 3);
        }
    }
}
