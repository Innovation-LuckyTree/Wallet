using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WalletService.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UpdatePromotionIdToLong : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<long>(
                name: "PromotionId",
                table: "BonusWalletTransaction",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "PromotionId",
                table: "BonusWalletTransaction",
                type: "int",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");
        }
    }
}
