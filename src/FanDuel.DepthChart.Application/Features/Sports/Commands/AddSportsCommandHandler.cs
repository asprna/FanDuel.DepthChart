using AutoMapper;
using FanDuel.DepthChart.Application.Contracts.Persistence;
using FanDuel.DepthChart.Application.Exceptions;
using FanDuel.DepthChart.Domain.Entities;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FanDuel.DepthChart.Application.Features.Sports.Commands
{
    public class AddSportsCommand : IRequest<int>
    {
        public string Name { get; set; }
        public List<string> Positions { get; set; }
    }

    public class AddProductCommandValidator : AbstractValidator<AddSportsCommand>
    {
        public AddProductCommandValidator()
        {
            RuleFor(p => p.Name)
                .NotEmpty().WithMessage("Name is required")
                .NotNull()
                .MaximumLength(50).WithMessage("Name must not exceed 50 characters");

            RuleFor(p => p.Positions)
                .NotNull().WithMessage("Positions cannot be null")
                .Must(positions => positions.All(pos => pos.Length <= 3))
                .WithMessage("Each position must be less than or equal to 3 characters.");

        }
    }

    public class AddSportsCommandHandler : IRequestHandler<AddSportsCommand, int>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<AddSportsCommandHandler> _logger;

        public AddSportsCommandHandler(IApplicationDbContext context, IMapper mapper, ILogger<AddSportsCommandHandler> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<int> Handle(AddSportsCommand request, CancellationToken cancellationToken)
        {
            var sport = _mapper.Map<Sport>(request);
            var newSport = await _context.Sports.AddAsync(sport);

            var result = await _context.SaveChangesAsync(cancellationToken) > 0;

            if (!result)
            {
                _logger.LogError("Unable to save changes to DB");
                throw new DataContextException("Unable to save changes to DB");
            }

            return newSport.Entity.Id;
        }
    }
}
