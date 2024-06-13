using FanDuel.DepthChart.Domain.Dtos;
using FluentValidation;

namespace FanDuel.DepthChart.API.Validation
{
    public class AddDepthChartDtoValidator : AbstractValidator<AddDepthChartDto>
    {
        public AddDepthChartDtoValidator() 
        {
            RuleFor(p => p.WeekId)
                .GreaterThan(0).WithMessage("Invalid week id");
        }
    }
}
