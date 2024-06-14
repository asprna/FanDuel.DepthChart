﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FanDuel.DepthChart.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddDateTimeToCharts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDateTimeUtc",
                table: "TeamDepthCharts",
                type: "TEXT",
                nullable: false,
                defaultValueSql: "GETUTCDATE()",
                oldClrType: typeof(DateTime),
                oldType: "TEXT",
                oldDefaultValue: new DateTime(2024, 6, 13, 11, 9, 53, 595, DateTimeKind.Utc).AddTicks(2077));

            migrationBuilder.AlterColumn<DateTime>(
                name: "ModifiedDateTimeUtc",
                table: "PlayerChartIndexes",
                type: "TEXT",
                nullable: false,
                defaultValueSql: "GETUTCDATE()",
                oldClrType: typeof(DateTime),
                oldType: "TEXT",
                oldDefaultValue: new DateTime(2024, 6, 13, 11, 9, 53, 595, DateTimeKind.Utc).AddTicks(2551));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDateTimeUtc",
                table: "PlayerChartIndexes",
                type: "TEXT",
                nullable: false,
                defaultValueSql: "GETUTCDATE()",
                oldClrType: typeof(DateTime),
                oldType: "TEXT",
                oldDefaultValue: new DateTime(2024, 6, 13, 11, 9, 53, 595, DateTimeKind.Utc).AddTicks(2355));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDateTimeUtc",
                table: "TeamDepthCharts",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(2024, 6, 13, 11, 9, 53, 595, DateTimeKind.Utc).AddTicks(2077),
                oldClrType: typeof(DateTime),
                oldType: "TEXT",
                oldDefaultValueSql: "GETUTCDATE()");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ModifiedDateTimeUtc",
                table: "PlayerChartIndexes",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(2024, 6, 13, 11, 9, 53, 595, DateTimeKind.Utc).AddTicks(2551),
                oldClrType: typeof(DateTime),
                oldType: "TEXT",
                oldDefaultValueSql: "GETUTCDATE()");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDateTimeUtc",
                table: "PlayerChartIndexes",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(2024, 6, 13, 11, 9, 53, 595, DateTimeKind.Utc).AddTicks(2355),
                oldClrType: typeof(DateTime),
                oldType: "TEXT",
                oldDefaultValueSql: "GETUTCDATE()");
        }
    }
}
