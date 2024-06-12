using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FanDuel.DepthChart.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddTeamDepthChartAndPlayerIndex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TeamDepthChart",
                schema: "DepthChart",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    WeekId = table.Column<int>(type: "INTEGER", nullable: false),
                    TeamId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeamDepthChart", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TeamDepthChart_Teams_TeamId",
                        column: x => x.TeamId,
                        principalSchema: "DepthChart",
                        principalTable: "Teams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PlayerChartIndex",
                schema: "DepthChart",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Rank = table.Column<int>(type: "INTEGER", nullable: false),
                    TeamDepthChartId = table.Column<int>(type: "INTEGER", nullable: false),
                    PayerId = table.Column<int>(type: "INTEGER", nullable: false),
                    PositionId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayerChartIndex", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlayerChartIndex_Players_PayerId",
                        column: x => x.PayerId,
                        principalSchema: "DepthChart",
                        principalTable: "Players",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PlayerChartIndex_Positions_PositionId",
                        column: x => x.PositionId,
                        principalSchema: "DepthChart",
                        principalTable: "Positions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PlayerChartIndex_TeamDepthChart_TeamDepthChartId",
                        column: x => x.TeamDepthChartId,
                        principalSchema: "DepthChart",
                        principalTable: "TeamDepthChart",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PlayerChartIndex_PayerId",
                schema: "DepthChart",
                table: "PlayerChartIndex",
                column: "PayerId");

            migrationBuilder.CreateIndex(
                name: "IX_PlayerChartIndex_PositionId",
                schema: "DepthChart",
                table: "PlayerChartIndex",
                column: "PositionId");

            migrationBuilder.CreateIndex(
                name: "IX_PlayerChartIndex_TeamDepthChartId",
                schema: "DepthChart",
                table: "PlayerChartIndex",
                column: "TeamDepthChartId");

            migrationBuilder.CreateIndex(
                name: "IX_TeamDepthChart_TeamId",
                schema: "DepthChart",
                table: "TeamDepthChart",
                column: "TeamId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PlayerChartIndex",
                schema: "DepthChart");

            migrationBuilder.DropTable(
                name: "TeamDepthChart",
                schema: "DepthChart");
        }
    }
}
