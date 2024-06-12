using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FanDuel.DepthChart.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ApiControllerBase : ControllerBase
    {
        private ISender _mediator;
        protected virtual ISender Mediator => _mediator ??= HttpContext.RequestServices.GetService<ISender>();
    }
}
