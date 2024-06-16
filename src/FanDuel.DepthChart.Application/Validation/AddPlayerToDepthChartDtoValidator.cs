using FanDuel.DepthChart.Domain.Dtos;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FanDuel.DepthChart.Application.Validation
{
    public class AddPlayerToDepthChartDtoValidator : AbstractValidator<AddPlayerToDepthChartDto>
    {
        public AddPlayerToDepthChartDtoValidator()
        {
            Include(new BasedPlayerDtoValidator());

            RuleFor(p => p.Rank)
                .Must(rank => !rank.HasValue || rank > 0)
                .WithMessage("Rank must be either null or greater than 0.");
        }
    }
}
