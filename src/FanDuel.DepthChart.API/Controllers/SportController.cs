using FanDuel.DepthChart.Application.Features.Sports.Commands;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace FanDuel.DepthChart.API.Controllers
{
    public class SportController : ApiControllerBase
    {
        [HttpPost(Name = "Add a new sport")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(AddSportsCommand), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> Post([FromBody] AddSportsCommand product)
        {
            var result = await Mediator.Send(product);
            return Ok(result);
        }
    }
}
