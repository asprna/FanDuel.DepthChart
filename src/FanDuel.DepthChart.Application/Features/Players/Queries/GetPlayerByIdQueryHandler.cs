using FanDuel.DepthChart.Application.Contracts.Persistence;
using FanDuel.DepthChart.Application.Features.DepthCharts.Queries;
using FanDuel.DepthChart.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FanDuel.DepthChart.Application.Features.Players.Queries
{
    public class GetPlayerByIdQuery : IRequest<Player>
    {
        public int Id { get; set; }
     
    }

    /// <summary>
    /// This handler is responsible for managing all the logic related to the 'find the player by Id' feature.
    /// </summary>
    public class GetPlayerByIdQueryHandler : IRequestHandler<GetPlayerByIdQuery, Player>
    {
        private readonly IApplicationDbContext _context;

        public GetPlayerByIdQueryHandler(IApplicationDbContext context) => _context = context;

        public Task<Player> Handle(GetPlayerByIdQuery request, CancellationToken cancellationToken)
        {
            return _context.Players
                .Include(p => p.Team)
                .ThenInclude(s => s.Sport)
                .ThenInclude(pos => pos.Positions)
                .Where(p => p.Id == request.Id)
                .FirstOrDefaultAsync(cancellationToken);
        }
    }
}
