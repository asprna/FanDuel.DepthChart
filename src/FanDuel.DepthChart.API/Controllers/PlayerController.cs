using FanDuel.DepthChart.Application.Features.Players.Commands;
using FanDuel.DepthChart.Application.Features.Teams.Commands;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace FanDuel.DepthChart.API.Controllers
{
    public class PlayerController : ApiControllerBase
    {
        [HttpPost(Name = "Add a player for a sport")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(AddPlayersCommand), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.Conflict)]
        public async Task<IActionResult> Post([FromBody] AddPlayersCommand team)
        {
            var result = await Mediator.Send(team);
            return Ok(result);
        }
    }
}
