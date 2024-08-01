using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WalletService.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddBonusWallet : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BonusAccount",
                columns: table => new
                {
                    BonusAccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BonusAccountType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Balance = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DateUpdated = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BonusAccount", x => x.BonusAccountId);
                });

            migrationBuilder.CreateTable(
                name: "BonusWalletTransaction",
                columns: table => new
                {
                    BonusWalletTransactionId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TransactionNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TransactionType = table.Column<int>(type: "int", nullable: false),
                    PromotionId = table.Column<int>(type: "int", nullable: true),
                    DateStarted = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DateExpired = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TransactionReference = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Credit = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TransactionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PreviousBalance = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ModeOfTransaction = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BonusWalletTransaction", x => x.BonusWalletTransactionId);
                    table.ForeignKey(
                        name: "FK_BonusWalletTransaction_BonusAccount_AccountId",
                        column: x => x.AccountId,
                        principalTable: "BonusAccount",
                        principalColumn: "BonusAccountId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BonusWalletTransaction_AccountId",
                table: "BonusWalletTransaction",
                column: "AccountId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BonusWalletTransaction");

            migrationBuilder.DropTable(
                name: "BonusAccount");
        }
    }
}
