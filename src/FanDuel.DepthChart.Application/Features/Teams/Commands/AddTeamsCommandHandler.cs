using AutoMapper;
using FanDuel.DepthChart.Application.Contracts.Persistence;
using FanDuel.DepthChart.Application.Exceptions;
using FanDuel.DepthChart.Application.Features.Sports.Commands;
using FanDuel.DepthChart.Domain.Entities;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FanDuel.DepthChart.Application.Features.Teams.Commands
{
    public class AddTeamsCommand : IRequest<int>
    {
        public string Name { get; set; }

        public int SportId { get; set; }
    }

    public class AddTeamsCommandValidator : AbstractValidator<AddTeamsCommand>
    {
        public AddTeamsCommandValidator()
        {
            RuleFor(p => p.Name)
                .NotEmpty().WithMessage("Name is required")
                .NotNull()
                .MaximumLength(50).WithMessage("Name must not exceed 50 characters");

            RuleFor(p => p.SportId)
                .GreaterThan(0).WithMessage("Sport Id should be greater than 0");

        }
    }

    public class AddTeamsCommandHandler : IRequestHandler<AddTeamsCommand, int>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<AddTeamsCommandHandler> _logger;

        public AddTeamsCommandHandler(IApplicationDbContext context, IMapper mapper, ILogger<AddTeamsCommandHandler> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<int> Handle(AddTeamsCommand request, CancellationToken cancellationToken)
        {
            //Check if Sport is valid
            _ = await _context.Sports.Where(x => x.Id == request.SportId).FirstOrDefaultAsync()
                ?? throw new NoContentException("Sport does not exists");

            var team = _mapper.Map<Team>(request);
            var newTeam = await _context.Teams.AddAsync(team);

            var result = await _context.SaveChangesAsync(cancellationToken) > 0;

            if (!result)
            {
                _logger.LogError("Unable to save changes to DB");
                throw new DataContextException("Unable to save changes to DB");
            }

            return newTeam.Entity.Id;
        }
    }
}
