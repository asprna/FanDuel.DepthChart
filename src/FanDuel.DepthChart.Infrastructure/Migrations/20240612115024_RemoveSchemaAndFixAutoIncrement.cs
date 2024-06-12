using FanDuel.DepthChart.Domain.Entities;
using Microsoft.EntityFrameworkCore.Migrations;
using System.Collections.Generic;

#nullable disable

namespace FanDuel.DepthChart.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemoveSchemaAndFixAutoIncrement : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Set AUTOINCREMENT start value to 1 for Sports table
            migrationBuilder.Sql("UPDATE sqlite_sequence SET seq = 0 WHERE name = 'Sports'");

            // Set AUTOINCREMENT start value to 1 for Positions table
            migrationBuilder.Sql("UPDATE sqlite_sequence SET seq = 0 WHERE name = 'Positions'");

            // Set AUTOINCREMENT start value to 1 for Player table
            migrationBuilder.Sql("UPDATE sqlite_sequence SET seq = 0 WHERE name = 'Player'");

            // Set AUTOINCREMENT start value to 1 for Player table
            migrationBuilder.Sql("UPDATE sqlite_sequence SET seq = 0 WHERE name = 'Team'");

            // Set AUTOINCREMENT start value to 1 for Player table
            migrationBuilder.Sql("UPDATE sqlite_sequence SET seq = 0 WHERE name = 'TeamDepthChart'");

            // Set AUTOINCREMENT start value to 1 for Player table
            migrationBuilder.Sql("UPDATE sqlite_sequence SET seq = 0 WHERE name = 'PlayerChartIndex'");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            
        }
    }
}
