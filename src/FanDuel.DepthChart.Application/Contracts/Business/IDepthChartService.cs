using FanDuel.DepthChart.Domain.Dtos;
using FanDuel.DepthChart.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FanDuel.DepthChart.Application.Contracts.Business
{
    /// <summary>
    /// This is the main Depth Chart interface.
    /// Individual Depth Charts for each sport should implement this interface.
    /// </summary>
    public interface IDepthChartService
    {
        /// <summary>
        /// Creates a Depth Chart for a given team.
        /// </summary>
        /// <param name="teamId">The ID of the team.</param>
        /// <param name="weekId">The ID of the week. If not provided, tries to create a new one for the current week if it has not been done before.</param>
        /// <returns>The ID of the created Depth Chart.</returns>
        Task<int> CreateDepthChart(int TeamId, int? WeekId);
        /// <summary>
        /// Adds an individual player to the given Depth Chart.
        /// </summary>
        /// <param name="position">The position of the player.</param>
        /// <param name="playerId">The ID of the player.</param>
        /// <param name="rank">The rank in the Depth Chart.</param>
        /// <param name="chartId">The ID of the Depth Chart. If not provided, the latest chart will be used.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task AddPlayerToDepthChart(string Position, int PlayerId, int? rank, int? chartId);
        /// <summary>
        /// Removes a given player from a Depth Chart.
        /// </summary>
        /// <param name="position">The position of the player.</param>
        /// <param name="playerId">The ID of the player.</param>
        /// <param name="chartId">The ID of the Depth Chart. If not provided, the latest chart will be used.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task<PlayerDto> RemovePlayerFromDepthChart(string Position, int PlayerId, int? chartId);
        /// <summary>
        /// For a given player and position, gets all the players that are backups.
        /// </summary>
        /// <param name="position">The position of the player.</param>
        /// <param name="playerId">The ID of the player.</param>
        /// <param name="chartId">The ID of the Depth Chart. If not provided, the latest chart will be used.</param>
        /// <returns>A list of backup players.</returns>
        Task<List<PlayerDto>> GetBackups(string Position, int PlayerId, int? chartId);
        /// <summary>
        /// Gets the full Depth Chart for a given ID.
        /// </summary>
        /// <param name="chartId">The ID of the Depth Chart. If not provided, the latest chart will be used.</param>
        /// <returns>A dictionary where the key is the position and the value is a list of players at that position.</returns>
        Task<Dictionary<string, List<Player>>> GetFullDepthChart(int? chartId);

    }
}
