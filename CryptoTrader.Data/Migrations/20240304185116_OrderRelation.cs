using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace CryptoTrader.Data.Migrations
{
    /// <inheritdoc />
    public partial class OrderRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_crypto_model_cryptos_crypto_id",
                table: "crypto_model");

            migrationBuilder.DropColumn(
                name: "close",
                table: "price_prediction");

            migrationBuilder.DropColumn(
                name: "open",
                table: "price_prediction");

            migrationBuilder.DropColumn(
                name: "eacp",
                table: "price_cycles");

            migrationBuilder.DropColumn(
                name: "edftse",
                table: "price_cycles");

            migrationBuilder.AddColumn<decimal>(
                name: "quantity_owned",
                table: "order",
                type: "numeric(35,15)",
                precision: 35,
                scale: 15,
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "created",
                table: "crypto_model",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<int>(
                name: "samples",
                table: "crypto_model",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "symbol",
                table: "crypto",
                type: "character varying(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(10)",
                oldMaxLength: 10);

            migrationBuilder.AddColumn<bool>(
                name: "active",
                table: "crypto",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "order_relation",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    buy_order_id = table.Column<int>(type: "integer", nullable: false),
                    sell_order_id = table.Column<int>(type: "integer", nullable: false),
                    quantity = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_order_relation", x => x.id);
                    table.ForeignKey(
                        name: "fk_order_relation_order_buy_order_id",
                        column: x => x.buy_order_id,
                        principalTable: "order",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_order_relation_order_sell_order_id",
                        column: x => x.sell_order_id,
                        principalTable: "order",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_order_relation_buy_order_id",
                table: "order_relation",
                column: "buy_order_id");

            migrationBuilder.CreateIndex(
                name: "ix_order_relation_sell_order_id",
                table: "order_relation",
                column: "sell_order_id");

            migrationBuilder.AddForeignKey(
                name: "fk_crypto_model_crypto_crypto_id",
                table: "crypto_model",
                column: "crypto_id",
                principalTable: "crypto",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_crypto_model_crypto_crypto_id",
                table: "crypto_model");

            migrationBuilder.DropTable(
                name: "order_relation");

            migrationBuilder.DropColumn(
                name: "quantity_owned",
                table: "order");

            migrationBuilder.DropColumn(
                name: "created",
                table: "crypto_model");

            migrationBuilder.DropColumn(
                name: "samples",
                table: "crypto_model");

            migrationBuilder.DropColumn(
                name: "active",
                table: "crypto");

            migrationBuilder.AddColumn<decimal>(
                name: "close",
                table: "price_prediction",
                type: "numeric(35,15)",
                precision: 35,
                scale: 15,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "open",
                table: "price_prediction",
                type: "numeric(35,15)",
                precision: 35,
                scale: 15,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "eacp",
                table: "price_cycles",
                type: "numeric(35,15)",
                precision: 35,
                scale: 15,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "edftse",
                table: "price_cycles",
                type: "numeric(35,15)",
                precision: 35,
                scale: 15,
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "symbol",
                table: "crypto",
                type: "character varying(10)",
                maxLength: 10,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(20)",
                oldMaxLength: 20);

            migrationBuilder.AddForeignKey(
                name: "fk_crypto_model_cryptos_crypto_id",
                table: "crypto_model",
                column: "crypto_id",
                principalTable: "crypto",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
