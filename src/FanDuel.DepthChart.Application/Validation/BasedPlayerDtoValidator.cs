using FanDuel.DepthChart.Domain.Dtos;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FanDuel.DepthChart.Application.Validation
{
    public class BasedPlayerDtoValidator : AbstractValidator<BasedPlayerDto>
    {
        public BasedPlayerDtoValidator()
        {
            RuleFor(p => p.ChartId)
                .Must(chartId => !chartId.HasValue || chartId > 0)
                .WithMessage("ChartId must be either null or greater than 0.");

            RuleFor(x => x.Position)
                .NotEmpty()
                .WithMessage("Position is required.");

            RuleFor(x => x.PlayerId)
                .GreaterThan(0)
                .WithMessage("PlayerId must be greater than 0.");
        }
    }
}
