using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FanDuel.DepthChart.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDepthChartsAndPlayerIndexes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlayerChartIndex_Players_PayerId",
                table: "PlayerChartIndex");

            migrationBuilder.DropForeignKey(
                name: "FK_PlayerChartIndex_Positions_PositionId",
                table: "PlayerChartIndex");

            migrationBuilder.DropForeignKey(
                name: "FK_PlayerChartIndex_TeamDepthChart_TeamDepthChartId",
                table: "PlayerChartIndex");

            migrationBuilder.DropForeignKey(
                name: "FK_TeamDepthChart_Teams_TeamId",
                table: "TeamDepthChart");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TeamDepthChart",
                table: "TeamDepthChart");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PlayerChartIndex",
                table: "PlayerChartIndex");

            migrationBuilder.RenameTable(
                name: "TeamDepthChart",
                newName: "TeamDepthCharts");

            migrationBuilder.RenameTable(
                name: "PlayerChartIndex",
                newName: "PlayerChartIndexs");

            migrationBuilder.RenameIndex(
                name: "IX_TeamDepthChart_TeamId",
                table: "TeamDepthCharts",
                newName: "IX_TeamDepthCharts_TeamId");

            migrationBuilder.RenameIndex(
                name: "IX_PlayerChartIndex_TeamDepthChartId",
                table: "PlayerChartIndexs",
                newName: "IX_PlayerChartIndexs_TeamDepthChartId");

            migrationBuilder.RenameIndex(
                name: "IX_PlayerChartIndex_PositionId",
                table: "PlayerChartIndexs",
                newName: "IX_PlayerChartIndexs_PositionId");

            migrationBuilder.RenameIndex(
                name: "IX_PlayerChartIndex_PayerId",
                table: "PlayerChartIndexs",
                newName: "IX_PlayerChartIndexs_PayerId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TeamDepthCharts",
                table: "TeamDepthCharts",
                column: "Id");

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

            migrationBuilder.AddForeignKey(
                name: "FK_TeamDepthCharts_Teams_TeamId",
                table: "TeamDepthCharts",
                column: "TeamId",
                principalTable: "Teams",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.DropForeignKey(
                name: "FK_TeamDepthCharts_Teams_TeamId",
                table: "TeamDepthCharts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TeamDepthCharts",
                table: "TeamDepthCharts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PlayerChartIndexs",
                table: "PlayerChartIndexs");

            migrationBuilder.RenameTable(
                name: "TeamDepthCharts",
                newName: "TeamDepthChart");

            migrationBuilder.RenameTable(
                name: "PlayerChartIndexs",
                newName: "PlayerChartIndex");

            migrationBuilder.RenameIndex(
                name: "IX_TeamDepthCharts_TeamId",
                table: "TeamDepthChart",
                newName: "IX_TeamDepthChart_TeamId");

            migrationBuilder.RenameIndex(
                name: "IX_PlayerChartIndexs_TeamDepthChartId",
                table: "PlayerChartIndex",
                newName: "IX_PlayerChartIndex_TeamDepthChartId");

            migrationBuilder.RenameIndex(
                name: "IX_PlayerChartIndexs_PositionId",
                table: "PlayerChartIndex",
                newName: "IX_PlayerChartIndex_PositionId");

            migrationBuilder.RenameIndex(
                name: "IX_PlayerChartIndexs_PayerId",
                table: "PlayerChartIndex",
                newName: "IX_PlayerChartIndex_PayerId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TeamDepthChart",
                table: "TeamDepthChart",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PlayerChartIndex",
                table: "PlayerChartIndex",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PlayerChartIndex_Players_PayerId",
                table: "PlayerChartIndex",
                column: "PayerId",
                principalTable: "Players",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PlayerChartIndex_Positions_PositionId",
                table: "PlayerChartIndex",
                column: "PositionId",
                principalTable: "Positions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PlayerChartIndex_TeamDepthChart_TeamDepthChartId",
                table: "PlayerChartIndex",
                column: "TeamDepthChartId",
                principalTable: "TeamDepthChart",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TeamDepthChart_Teams_TeamId",
                table: "TeamDepthChart",
                column: "TeamId",
                principalTable: "Teams",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
