using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SabioExam.DATA.Migrations
{
    /// <inheritdoc />
    public partial class Transaction_ForeignKey_Correction : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DiscountCodeCode",
                table: "Transactions",
                type: "nvarchar(8)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_DiscountCodeCode",
                table: "Transactions",
                column: "DiscountCodeCode");

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_DiscountCodes_DiscountCodeCode",
                table: "Transactions",
                column: "DiscountCodeCode",
                principalTable: "DiscountCodes",
                principalColumn: "Code");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_DiscountCodes_DiscountCodeCode",
                table: "Transactions");

            migrationBuilder.DropIndex(
                name: "IX_Transactions_DiscountCodeCode",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "DiscountCodeCode",
                table: "Transactions");
        }
    }
}
