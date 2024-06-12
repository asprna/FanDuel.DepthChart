using FanDuel.DepthChart.Application.Features.Sports.Commands;
using FanDuel.DepthChart.Application.Features.Teams.Commands;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace FanDuel.DepthChart.API.Controllers
{
    public class TeamController : ApiControllerBase
    {
        [HttpPost(Name = "Add a team for a sport")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(AddTeamsCommand), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.NoContent)]
        public async Task<IActionResult> Post([FromBody] AddTeamsCommand team)
        {
            var result = await Mediator.Send(team);
            return Ok(result);
        }
    }
}
