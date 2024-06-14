using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FanDuel.DepthChart.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddDateTimeToChartsDateTimeDotNet : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDateTimeUtc",
                table: "TeamDepthCharts",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(2024, 6, 13, 11, 25, 43, 584, DateTimeKind.Utc).AddTicks(8766),
                oldClrType: typeof(DateTime),
                oldType: "TEXT",
                oldDefaultValueSql: "GETUTCDATE()");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ModifiedDateTimeUtc",
                table: "PlayerChartIndexes",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(2024, 6, 13, 11, 25, 43, 584, DateTimeKind.Utc).AddTicks(9241),
                oldClrType: typeof(DateTime),
                oldType: "TEXT",
                oldDefaultValueSql: "GETUTCDATE()");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDateTimeUtc",
                table: "PlayerChartIndexes",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(2024, 6, 13, 11, 25, 43, 584, DateTimeKind.Utc).AddTicks(9044),
                oldClrType: typeof(DateTime),
                oldType: "TEXT",
                oldDefaultValueSql: "GETUTCDATE()");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDateTimeUtc",
                table: "TeamDepthCharts",
                type: "TEXT",
                nullable: false,
                defaultValueSql: "GETUTCDATE()",
                oldClrType: typeof(DateTime),
                oldType: "TEXT",
                oldDefaultValue: new DateTime(2024, 6, 13, 11, 25, 43, 584, DateTimeKind.Utc).AddTicks(8766));

            migrationBuilder.AlterColumn<DateTime>(
                name: "ModifiedDateTimeUtc",
                table: "PlayerChartIndexes",
                type: "TEXT",
                nullable: false,
                defaultValueSql: "GETUTCDATE()",
                oldClrType: typeof(DateTime),
                oldType: "TEXT",
                oldDefaultValue: new DateTime(2024, 6, 13, 11, 25, 43, 584, DateTimeKind.Utc).AddTicks(9241));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDateTimeUtc",
                table: "PlayerChartIndexes",
                type: "TEXT",
                nullable: false,
                defaultValueSql: "GETUTCDATE()",
                oldClrType: typeof(DateTime),
                oldType: "TEXT",
                oldDefaultValue: new DateTime(2024, 6, 13, 11, 25, 43, 584, DateTimeKind.Utc).AddTicks(9044));
        }
    }
}
