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
                .GreaterThan(0).WithMessage("Invalid Week id");

            RuleFor(p => p.TeamId)
                .GreaterThan(0).WithMessage("Invalid Team id");
        }
    }
}
