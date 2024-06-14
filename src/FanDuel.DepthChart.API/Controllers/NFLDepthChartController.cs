﻿using Microsoft.AspNetCore.Mvc;
using FanDuel.DepthChart.Application.Contracts.Business;
using FanDuel.DepthChart.Application.Features.Players.Commands;
using System.Net;
using FanDuel.DepthChart.Application.Features.DepthCharts.Commands;
using FanDuel.DepthChart.Domain.Dtos;

namespace FanDuel.DepthChart.API.Controllers
{
    public class NFLDepthChartController : ApiControllerBase
    {
        private readonly IDepthChartServiceFactory _depthChartFactory;
        private readonly IDepthChartService _depthChart;

        public NFLDepthChartController(IDepthChartServiceFactory depthChartFactory)
        {
            _depthChartFactory = depthChartFactory;
            _depthChart = _depthChartFactory.CreateDepthChart("NFL");
        }

        [HttpPost(Name = "Create a Depth Chart for a team")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(AddPlayersCommand), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.NoContent)]
        public async Task<IActionResult> Post([FromBody] AddDepthChartDto depthChart)
        {
            return Ok(await _depthChart.CreateDepthChart(depthChart.TeamId, depthChart.WeekId));
        }

        [HttpPost("AddPlayerToDepthChart", Name = "Add player to Depth Chart")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(AddPlayerToDepthChartDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.NoContent)]
        public async Task<IActionResult> AddPlayerToDepthChart([FromBody] AddPlayerToDepthChartDto player)
        {
            await _depthChart.AddPlayerToDepthChart(player.Position, player.PlayerId, player.Rank, player.ChartId);
            return Ok();
        }

        [HttpDelete("RemovePlayerFromDepthChart", Name = "Remove player from a Depth Chart")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(RemovePlayerFromDepthChartDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.NoContent)]
        public async Task<IActionResult> AddPlayerToDepthChart([FromBody] RemovePlayerFromDepthChartDto player)
        {
            return Ok(await _depthChart.RemovePlayerFromDepthChart(player.Position, player.PlayerId, player.ChartId));
        }

        [HttpGet("GetBackups", Name = "Get Player Backups")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(PlayerBacksDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.NoContent)]
        public async Task<IActionResult> GetBackups([FromQuery] PlayerBacksDto player)
        {
            return Ok(await _depthChart.GetBackups(player.Position, player.PlayerId, player.ChartId));
        }
    }
}
