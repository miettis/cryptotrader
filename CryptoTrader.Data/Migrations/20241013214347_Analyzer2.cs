using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace CryptoTrader.Data.Migrations
{
    /// <inheritdoc />
    public partial class Analyzer2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.CreateIndex(
                name: "ix_price_feature_price_id",
                table: "price_feature",
                column: "price_id");

            migrationBuilder.AddForeignKey(
                name: "fk_price_feature_feature_feature_id",
                table: "price_feature",
                column: "feature_id",
                principalTable: "feature",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_price_feature_price_price_id",
                table: "price_feature",
                column: "price_id",
                principalTable: "price",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_price_feature_feature_feature_id",
                table: "price_feature");

            migrationBuilder.DropForeignKey(
                name: "fk_price_feature_price_price_id",
                table: "price_feature");

            migrationBuilder.DropPrimaryKey(
                name: "pk_price_feature",
                table: "price_feature");

            migrationBuilder.DropIndex(
                name: "ix_price_feature_price_id",
                table: "price_feature");

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
    }
}
