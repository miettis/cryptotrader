using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CryptoTrader.Data.Migrations
{
    /// <inheritdoc />
    public partial class PriceUnique : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_order_relation_order_sell_order_id",
                table: "order_relation");

            migrationBuilder.DropIndex(
                name: "ix_price_crypto_id",
                table: "price");

            migrationBuilder.RenameColumn(
                name: "quantity_remaining",
                table: "order",
                newName: "unmatched_quantity");

            migrationBuilder.RenameColumn(
                name: "quantity_owned",
                table: "order",
                newName: "remaining_quantity");

            migrationBuilder.AlterColumn<string>(
                name: "side",
                table: "order",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(10)",
                oldMaxLength: 10);

            migrationBuilder.CreateIndex(
                name: "ix_price_crypto_id_time_open",
                table: "price",
                columns: new[] { "crypto_id", "time_open" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "fk_order_relation_order_sell_order_id",
                table: "order_relation",
                column: "sell_order_id",
                principalTable: "order",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_order_relation_order_sell_order_id",
                table: "order_relation");

            migrationBuilder.DropIndex(
                name: "ix_price_crypto_id_time_open",
                table: "price");

            migrationBuilder.RenameColumn(
                name: "unmatched_quantity",
                table: "order",
                newName: "quantity_remaining");

            migrationBuilder.RenameColumn(
                name: "remaining_quantity",
                table: "order",
                newName: "quantity_owned");

            migrationBuilder.AlterColumn<string>(
                name: "side",
                table: "order",
                type: "character varying(10)",
                maxLength: 10,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.CreateIndex(
                name: "ix_price_crypto_id",
                table: "price",
                column: "crypto_id");

            migrationBuilder.AddForeignKey(
                name: "fk_order_relation_order_sell_order_id",
                table: "order_relation",
                column: "sell_order_id",
                principalTable: "order",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
