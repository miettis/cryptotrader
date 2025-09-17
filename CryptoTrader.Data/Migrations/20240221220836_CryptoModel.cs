using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace CryptoTrader.Data.Migrations
{
    /// <inheritdoc />
    public partial class CryptoModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "day_rank12",
                table: "price_prediction",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "day_rank2",
                table: "price_prediction",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "day_rank3",
                table: "price_prediction",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "day_rank4",
                table: "price_prediction",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "day_rank6",
                table: "price_prediction",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "day_rank8",
                table: "price_prediction",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "twoday_rank12",
                table: "price_prediction",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "twoday_rank2",
                table: "price_prediction",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "twoday_rank3",
                table: "price_prediction",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "twoday_rank4",
                table: "price_prediction",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "twoday_rank6",
                table: "price_prediction",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "twoday_rank8",
                table: "price_prediction",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "week_rank12",
                table: "price_prediction",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "week_rank2",
                table: "price_prediction",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "week_rank3",
                table: "price_prediction",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "week_rank4",
                table: "price_prediction",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "week_rank6",
                table: "price_prediction",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "week_rank8",
                table: "price_prediction",
                type: "integer",
                nullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "sc",
                table: "price_cycles",
                type: "numeric(35,15)",
                precision: 35,
                scale: 15,
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric(35,15)",
                oldPrecision: 35,
                oldScale: 15);

            migrationBuilder.AlterColumn<decimal>(
                name: "glco",
                table: "price_cycles",
                type: "numeric(35,15)",
                precision: 35,
                scale: 15,
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric(35,15)",
                oldPrecision: 35,
                oldScale: 15);

            migrationBuilder.CreateTable(
                name: "crypto_model",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    crypto_id = table.Column<int>(type: "integer", nullable: false),
                    start_time = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    end_time = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    output = table.Column<string>(type: "text", nullable: false),
                    model_name = table.Column<string>(type: "text", nullable: false),
                    accuracy = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_crypto_model", x => x.id);
                    table.ForeignKey(
                        name: "fk_crypto_model_cryptos_crypto_id",
                        column: x => x.crypto_id,
                        principalTable: "crypto",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_crypto_model_crypto_id",
                table: "crypto_model",
                column: "crypto_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "crypto_model");

            migrationBuilder.DropColumn(
                name: "day_rank12",
                table: "price_prediction");

            migrationBuilder.DropColumn(
                name: "day_rank2",
                table: "price_prediction");

            migrationBuilder.DropColumn(
                name: "day_rank3",
                table: "price_prediction");

            migrationBuilder.DropColumn(
                name: "day_rank4",
                table: "price_prediction");

            migrationBuilder.DropColumn(
                name: "day_rank6",
                table: "price_prediction");

            migrationBuilder.DropColumn(
                name: "day_rank8",
                table: "price_prediction");

            migrationBuilder.DropColumn(
                name: "twoday_rank12",
                table: "price_prediction");

            migrationBuilder.DropColumn(
                name: "twoday_rank2",
                table: "price_prediction");

            migrationBuilder.DropColumn(
                name: "twoday_rank3",
                table: "price_prediction");

            migrationBuilder.DropColumn(
                name: "twoday_rank4",
                table: "price_prediction");

            migrationBuilder.DropColumn(
                name: "twoday_rank6",
                table: "price_prediction");

            migrationBuilder.DropColumn(
                name: "twoday_rank8",
                table: "price_prediction");

            migrationBuilder.DropColumn(
                name: "week_rank12",
                table: "price_prediction");

            migrationBuilder.DropColumn(
                name: "week_rank2",
                table: "price_prediction");

            migrationBuilder.DropColumn(
                name: "week_rank3",
                table: "price_prediction");

            migrationBuilder.DropColumn(
                name: "week_rank4",
                table: "price_prediction");

            migrationBuilder.DropColumn(
                name: "week_rank6",
                table: "price_prediction");

            migrationBuilder.DropColumn(
                name: "week_rank8",
                table: "price_prediction");

            migrationBuilder.AlterColumn<decimal>(
                name: "sc",
                table: "price_cycles",
                type: "numeric(35,15)",
                precision: 35,
                scale: 15,
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "numeric(35,15)",
                oldPrecision: 35,
                oldScale: 15,
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "glco",
                table: "price_cycles",
                type: "numeric(35,15)",
                precision: 35,
                scale: 15,
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "numeric(35,15)",
                oldPrecision: 35,
                oldScale: 15,
                oldNullable: true);
        }
    }
}
