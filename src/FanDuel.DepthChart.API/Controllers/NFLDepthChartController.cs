using Microsoft.AspNetCore.Mvc;
using FanDuel.DepthChart.Application.Contracts.Business;
using FanDuel.DepthChart.Application.Features.Players.Commands;
using System.Net;
using FanDuel.DepthChart.Application.Features.DepthCharts.Commands;
using FanDuel.DepthChart.Domain.Dtos;

namespace FanDuel.DepthChart.API.Controllers
{
    public class NFLDepthChartController : ApiControllerBase
    {
        private readonly IDepthChartFactory _depthChartFactory;
        private readonly IDepthChart _depthChart;

        public NFLDepthChartController(IDepthChartFactory depthChartFactory)
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
    }
}
