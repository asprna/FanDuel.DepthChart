using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FanDuel.DepthChart.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddNonClusterIndexForCreatedDate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_TeamDepthCharts_CreatedDateTimeUtc",
                table: "TeamDepthCharts",
                column: "CreatedDateTimeUtc");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_TeamDepthCharts_CreatedDateTimeUtc",
                table: "TeamDepthCharts");
        }
    }
}
