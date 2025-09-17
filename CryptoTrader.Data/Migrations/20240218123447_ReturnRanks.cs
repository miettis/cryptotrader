using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CryptoTrader.Data.Migrations
{
    /// <inheritdoc />
    public partial class ReturnRanks : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "volume",
                table: "crypto",
                newName: "end_volume");

            migrationBuilder.RenameColumn(
                name: "volatility",
                table: "crypto",
                newName: "end_volatility");

            migrationBuilder.RenameColumn(
                name: "trend",
                table: "crypto",
                newName: "end_trend");

            migrationBuilder.RenameColumn(
                name: "times_start_data",
                table: "crypto",
                newName: "start_data");

            migrationBuilder.RenameColumn(
                name: "times_start",
                table: "crypto",
                newName: "start");

            migrationBuilder.RenameColumn(
                name: "times_end_data",
                table: "crypto",
                newName: "end_data");

            migrationBuilder.RenameColumn(
                name: "slope",
                table: "crypto",
                newName: "end_slope");

            migrationBuilder.RenameColumn(
                name: "return",
                table: "crypto",
                newName: "end_return");

            migrationBuilder.RenameColumn(
                name: "peak",
                table: "crypto",
                newName: "end_peak");

            migrationBuilder.RenameColumn(
                name: "other",
                table: "crypto",
                newName: "end_other");

            migrationBuilder.RenameColumn(
                name: "momentum",
                table: "crypto",
                newName: "end_momentum");

            migrationBuilder.RenameColumn(
                name: "ma",
                table: "crypto",
                newName: "end_ma");

            migrationBuilder.RenameColumn(
                name: "cycle",
                table: "crypto",
                newName: "end_cycle");

            migrationBuilder.RenameColumn(
                name: "candle",
                table: "crypto",
                newName: "end_candle");

            migrationBuilder.AddColumn<int>(
                name: "day_rank12",
                table: "price_return",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "day_rank2",
                table: "price_return",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "day_rank3",
                table: "price_return",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "day_rank4",
                table: "price_return",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "day_rank6",
                table: "price_return",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "day_rank8",
                table: "price_return",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "twoday_rank12",
                table: "price_return",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "twoday_rank2",
                table: "price_return",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "twoday_rank3",
                table: "price_return",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "twoday_rank4",
                table: "price_return",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "twoday_rank6",
                table: "price_return",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "twoday_rank8",
                table: "price_return",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "week_rank12",
                table: "price_return",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "week_rank2",
                table: "price_return",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "week_rank3",
                table: "price_return",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "week_rank4",
                table: "price_return",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "week_rank6",
                table: "price_return",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "week_rank8",
                table: "price_return",
                type: "integer",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "day_rank12",
                table: "price_return");

            migrationBuilder.DropColumn(
                name: "day_rank2",
                table: "price_return");

            migrationBuilder.DropColumn(
                name: "day_rank3",
                table: "price_return");

            migrationBuilder.DropColumn(
                name: "day_rank4",
                table: "price_return");

            migrationBuilder.DropColumn(
                name: "day_rank6",
                table: "price_return");

            migrationBuilder.DropColumn(
                name: "day_rank8",
                table: "price_return");

            migrationBuilder.DropColumn(
                name: "twoday_rank12",
                table: "price_return");

            migrationBuilder.DropColumn(
                name: "twoday_rank2",
                table: "price_return");

            migrationBuilder.DropColumn(
                name: "twoday_rank3",
                table: "price_return");

            migrationBuilder.DropColumn(
                name: "twoday_rank4",
                table: "price_return");

            migrationBuilder.DropColumn(
                name: "twoday_rank6",
                table: "price_return");

            migrationBuilder.DropColumn(
                name: "twoday_rank8",
                table: "price_return");

            migrationBuilder.DropColumn(
                name: "week_rank12",
                table: "price_return");

            migrationBuilder.DropColumn(
                name: "week_rank2",
                table: "price_return");

            migrationBuilder.DropColumn(
                name: "week_rank3",
                table: "price_return");

            migrationBuilder.DropColumn(
                name: "week_rank4",
                table: "price_return");

            migrationBuilder.DropColumn(
                name: "week_rank6",
                table: "price_return");

            migrationBuilder.DropColumn(
                name: "week_rank8",
                table: "price_return");

            migrationBuilder.RenameColumn(
                name: "start_data",
                table: "crypto",
                newName: "times_start_data");

            migrationBuilder.RenameColumn(
                name: "start",
                table: "crypto",
                newName: "times_start");

            migrationBuilder.RenameColumn(
                name: "end_volume",
                table: "crypto",
                newName: "volume");

            migrationBuilder.RenameColumn(
                name: "end_volatility",
                table: "crypto",
                newName: "volatility");

            migrationBuilder.RenameColumn(
                name: "end_trend",
                table: "crypto",
                newName: "trend");

            migrationBuilder.RenameColumn(
                name: "end_slope",
                table: "crypto",
                newName: "slope");

            migrationBuilder.RenameColumn(
                name: "end_return",
                table: "crypto",
                newName: "return");

            migrationBuilder.RenameColumn(
                name: "end_peak",
                table: "crypto",
                newName: "peak");

            migrationBuilder.RenameColumn(
                name: "end_other",
                table: "crypto",
                newName: "other");

            migrationBuilder.RenameColumn(
                name: "end_momentum",
                table: "crypto",
                newName: "momentum");

            migrationBuilder.RenameColumn(
                name: "end_ma",
                table: "crypto",
                newName: "ma");

            migrationBuilder.RenameColumn(
                name: "end_data",
                table: "crypto",
                newName: "times_end_data");

            migrationBuilder.RenameColumn(
                name: "end_cycle",
                table: "crypto",
                newName: "cycle");

            migrationBuilder.RenameColumn(
                name: "end_candle",
                table: "crypto",
                newName: "candle");
        }
    }
}
