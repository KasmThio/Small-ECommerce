using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Small_E_Commerce.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class order_item_id_hilow : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateSequence(
                name: "SubOrderIdSequence",
                incrementBy: 10);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropSequence(
                name: "SubOrderIdSequence");
        }
    }
}
