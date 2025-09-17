using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace CryptoTrader.Data.Migrations
{
    /// <inheritdoc />
    public partial class Analyzer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_price_feature_features_feature_id",
                table: "price_feature");

            migrationBuilder.DropForeignKey(
                name: "fk_price_feature_prices_price_id",
                table: "price_feature");

            migrationBuilder.DropTable(
                name: "features");

            migrationBuilder.DropPrimaryKey(
                name: "pk_price_feature",
                table: "price_feature");

            migrationBuilder.DropIndex(
                name: "ix_price_feature_price_id",
                table: "price_feature");

            migrationBuilder.DropColumn(
                name: "boolean",
                table: "feature");

            migrationBuilder.DropColumn(
                name: "my_property",
                table: "feature");

            migrationBuilder.DropColumn(
                name: "number",
                table: "feature");

            migrationBuilder.DropColumn(
                name: "price_id",
                table: "price_feature");

            migrationBuilder.RenameTable(
                name: "price_feature",
                newName: "price_features");

            migrationBuilder.RenameIndex(
                name: "ix_price_feature_feature_id",
                table: "price_features",
                newName: "ix_price_features_feature_id");

            migrationBuilder.AlterColumn<long>(
                name: "id",
                table: "price_features",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint")
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddPrimaryKey(
                name: "pk_price_features",
                table: "price_features",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_price_features_feature_feature_id",
                table: "price_features",
                column: "feature_id",
                principalTable: "feature",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_price_features_price_id",
                table: "price_features",
                column: "id",
                principalTable: "price",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_price_features_feature_feature_id",
                table: "price_features");

            migrationBuilder.DropForeignKey(
                name: "fk_price_features_price_id",
                table: "price_features");

            migrationBuilder.DropPrimaryKey(
                name: "pk_price_features",
                table: "price_features");

            migrationBuilder.RenameTable(
                name: "price_features",
                newName: "price_feature");

            migrationBuilder.RenameIndex(
                name: "ix_price_features_feature_id",
                table: "price_feature",
                newName: "ix_price_feature_feature_id");

            migrationBuilder.AddColumn<bool>(
                name: "boolean",
                table: "feature",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "my_property",
                table: "feature",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<float>(
                name: "number",
                table: "feature",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AlterColumn<long>(
                name: "id",
                table: "price_feature",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddColumn<long>(
                name: "price_id",
                table: "price_feature",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddPrimaryKey(
                name: "pk_price_feature",
                table: "price_feature",
                column: "id");

            migrationBuilder.CreateTable(
                name: "features",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    category = table.Column<int>(type: "integer", nullable: false),
                    indicator_type = table.Column<int>(type: "integer", nullable: false),
                    key = table.Column<string>(type: "text", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_features", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "ix_price_feature_price_id",
                table: "price_feature",
                column: "price_id");

            migrationBuilder.AddForeignKey(
                name: "fk_price_feature_features_feature_id",
                table: "price_feature",
                column: "feature_id",
                principalTable: "features",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_price_feature_prices_price_id",
                table: "price_feature",
                column: "price_id",
                principalTable: "price",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
