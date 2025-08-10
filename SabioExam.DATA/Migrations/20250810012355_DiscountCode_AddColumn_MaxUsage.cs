using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SabioExam.DATA.Migrations
{
    /// <inheritdoc />
    public partial class DiscountCode_AddColumn_MaxUsage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "MaxUsage",
                table: "DiscountCodes",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MaxUsage",
                table: "DiscountCodes");
        }
    }
}
