using FanDuel.DepthChart.Domain.Dtos;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FanDuel.DepthChart.Application.Validation
{
    public class AddDepthChartDtoValidator : AbstractValidator<AddDepthChartDto>
    {
        public AddDepthChartDtoValidator()
        {
            RuleFor(p => p.WeekId)
                .Must(rank => !rank.HasValue || rank > 0)
                .WithMessage("WeekId must be either null or greater than 0.");
            
            RuleFor(p => p.TeamId)
                .GreaterThan(0).WithMessage("Invalid Team id");
        }
    }
}
