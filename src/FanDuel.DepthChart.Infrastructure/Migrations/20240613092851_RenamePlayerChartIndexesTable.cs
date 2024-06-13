using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FanDuel.DepthChart.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RenamePlayerChartIndexesTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlayerChartIndexs_Players_PayerId",
                table: "PlayerChartIndexs");

            migrationBuilder.DropForeignKey(
                name: "FK_PlayerChartIndexs_Positions_PositionId",
                table: "PlayerChartIndexs");

            migrationBuilder.DropForeignKey(
                name: "FK_PlayerChartIndexs_TeamDepthCharts_TeamDepthChartId",
                table: "PlayerChartIndexs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PlayerChartIndexs",
                table: "PlayerChartIndexs");

            migrationBuilder.RenameTable(
                name: "PlayerChartIndexs",
                newName: "PlayerChartIndexes");

            migrationBuilder.RenameIndex(
                name: "IX_PlayerChartIndexs_TeamDepthChartId",
                table: "PlayerChartIndexes",
                newName: "IX_PlayerChartIndexes_TeamDepthChartId");

            migrationBuilder.RenameIndex(
                name: "IX_PlayerChartIndexs_PositionId",
                table: "PlayerChartIndexes",
                newName: "IX_PlayerChartIndexes_PositionId");

            migrationBuilder.RenameIndex(
                name: "IX_PlayerChartIndexs_PayerId",
                table: "PlayerChartIndexes",
                newName: "IX_PlayerChartIndexes_PayerId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PlayerChartIndexes",
                table: "PlayerChartIndexes",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PlayerChartIndexes_Players_PayerId",
                table: "PlayerChartIndexes",
                column: "PayerId",
                principalTable: "Players",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PlayerChartIndexes_Positions_PositionId",
                table: "PlayerChartIndexes",
                column: "PositionId",
                principalTable: "Positions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PlayerChartIndexes_TeamDepthCharts_TeamDepthChartId",
                table: "PlayerChartIndexes",
                column: "TeamDepthChartId",
                principalTable: "TeamDepthCharts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlayerChartIndexes_Players_PayerId",
                table: "PlayerChartIndexes");

            migrationBuilder.DropForeignKey(
                name: "FK_PlayerChartIndexes_Positions_PositionId",
                table: "PlayerChartIndexes");

            migrationBuilder.DropForeignKey(
                name: "FK_PlayerChartIndexes_TeamDepthCharts_TeamDepthChartId",
                table: "PlayerChartIndexes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PlayerChartIndexes",
                table: "PlayerChartIndexes");

            migrationBuilder.RenameTable(
                name: "PlayerChartIndexes",
                newName: "PlayerChartIndexs");

            migrationBuilder.RenameIndex(
                name: "IX_PlayerChartIndexes_TeamDepthChartId",
                table: "PlayerChartIndexs",
                newName: "IX_PlayerChartIndexs_TeamDepthChartId");

            migrationBuilder.RenameIndex(
                name: "IX_PlayerChartIndexes_PositionId",
                table: "PlayerChartIndexs",
                newName: "IX_PlayerChartIndexs_PositionId");

            migrationBuilder.RenameIndex(
                name: "IX_PlayerChartIndexes_PayerId",
                table: "PlayerChartIndexs",
                newName: "IX_PlayerChartIndexs_PayerId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PlayerChartIndexs",
                table: "PlayerChartIndexs",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PlayerChartIndexs_Players_PayerId",
                table: "PlayerChartIndexs",
                column: "PayerId",
                principalTable: "Players",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PlayerChartIndexs_Positions_PositionId",
                table: "PlayerChartIndexs",
                column: "PositionId",
                principalTable: "Positions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PlayerChartIndexs_TeamDepthCharts_TeamDepthChartId",
                table: "PlayerChartIndexs",
                column: "TeamDepthChartId",
                principalTable: "TeamDepthCharts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
