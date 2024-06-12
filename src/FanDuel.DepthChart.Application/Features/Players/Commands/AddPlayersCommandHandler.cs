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

namespace FanDuel.DepthChart.Application.Features.Players.Commands
{
    public class AddPlayersCommand : IRequest<int>
    {
        public int Number { get; set; }
        public string Name { get; set; }
        public int TeamId { get; set; }
    }

    public class AddPlayersCommandValidator : AbstractValidator<AddPlayersCommand>
    {
        public AddPlayersCommandValidator()
        {
            RuleFor(p => p.Name)
                .NotEmpty().WithMessage("Name is required");

            RuleFor(p => p.Number)
                .GreaterThan(0).WithMessage("Invalid Player Number");

            RuleFor(p => p.TeamId)
                .GreaterThan(0).WithMessage("Invalid Team Id");

        }
    }

    public class AddPlayersCommandHandler : IRequestHandler<AddPlayersCommand, int>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<AddPlayersCommandHandler> _logger;

        public AddPlayersCommandHandler(IApplicationDbContext context, IMapper mapper, ILogger<AddPlayersCommandHandler> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<int> Handle(AddPlayersCommand request, CancellationToken cancellationToken)
        {
            //Check if Team is valid
            _ = await _context.Teams.Where(x => x.Id == request.TeamId).FirstOrDefaultAsync()
                ?? throw new NoContentException("Team does not exists");

            //Check if the Player's Number is unique
            if ( await _context.Players.Where(x => x.TeamId == request.TeamId && x.Number == request.Number).AnyAsync())
                throw new ConflictException("Player number conflits with existing player");

            var player = _mapper.Map<Player>(request);
            var newPlayer = await _context.Players.AddAsync(player);

            var result = await _context.SaveChangesAsync(cancellationToken) > 0;

            if (!result)
            {
                _logger.LogError("Unable to save changes to DB");
                throw new DataContextException("Unable to save changes to DB");
            }

            return newPlayer.Entity.Id;
        }
    }
}
