using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CryptoTrader.Data.Migrations
{
    /// <inheritdoc />
    public partial class Timestamps : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "month_return",
                table: "price_return",
                newName: "twoday_return");

            migrationBuilder.RenameColumn(
                name: "month_interval",
                table: "price_return",
                newName: "twoday_interval");

            migrationBuilder.RenameColumn(
                name: "times_end_slope",
                table: "crypto",
                newName: "slope");

            migrationBuilder.RenameColumn(
                name: "times_end_return",
                table: "crypto",
                newName: "return");

            migrationBuilder.RenameColumn(
                name: "times_end_ma",
                table: "crypto",
                newName: "ma");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "candle",
                table: "crypto",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "cycle",
                table: "crypto",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "momentum",
                table: "crypto",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "other",
                table: "crypto",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "peak",
                table: "crypto",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "trend",
                table: "crypto",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "volatility",
                table: "crypto",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "volume",
                table: "crypto",
                type: "timestamp with time zone",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "candle",
                table: "crypto");

            migrationBuilder.DropColumn(
                name: "cycle",
                table: "crypto");

            migrationBuilder.DropColumn(
                name: "momentum",
                table: "crypto");

            migrationBuilder.DropColumn(
                name: "other",
                table: "crypto");

            migrationBuilder.DropColumn(
                name: "peak",
                table: "crypto");

            migrationBuilder.DropColumn(
                name: "trend",
                table: "crypto");

            migrationBuilder.DropColumn(
                name: "volatility",
                table: "crypto");

            migrationBuilder.DropColumn(
                name: "volume",
                table: "crypto");

            migrationBuilder.RenameColumn(
                name: "twoday_return",
                table: "price_return",
                newName: "month_return");

            migrationBuilder.RenameColumn(
                name: "twoday_interval",
                table: "price_return",
                newName: "month_interval");

            migrationBuilder.RenameColumn(
                name: "slope",
                table: "crypto",
                newName: "times_end_slope");

            migrationBuilder.RenameColumn(
                name: "return",
                table: "crypto",
                newName: "times_end_return");

            migrationBuilder.RenameColumn(
                name: "ma",
                table: "crypto",
                newName: "times_end_ma");
        }
    }
}
