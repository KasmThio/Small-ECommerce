using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Small_E_Commerce.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updated_order_id_mapper : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OrderIdentifier_Value",
                table: "Orders");

            migrationBuilder.DropSequence(
                name: "OrderIdSequence");

            migrationBuilder.AddColumn<string>(
                name: "OrderIdentifier",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OrderIdentifier",
                table: "Orders");

            migrationBuilder.CreateSequence(
                name: "OrderIdSequence",
                startValue: 1000L,
                incrementBy: 10);

            migrationBuilder.AddColumn<long>(
                name: "OrderIdentifier_Value",
                table: "Orders",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }
    }
}
