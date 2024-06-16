using FanDuel.DepthChart.Domain.Dtos;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FanDuel.DepthChart.Application.Validation
{
    public class RemovePlayerFromDepthChartDtoValidator : AbstractValidator<RemovePlayerFromDepthChartDto>
    {
        public RemovePlayerFromDepthChartDtoValidator()
        {
            Include(new BasedPlayerDtoValidator());
        }
    }
}
