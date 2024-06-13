using FanDuel.DepthChart.Application.Contracts.Persistence;
using FanDuel.DepthChart.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FanDuel.DepthChart.Application.Features.Teams.Queries
{
    public class GetTeamQuery : IRequest<Team>
    {
        public int Id { get; set; }
    }

    public class GetTeamQueryHandler : IRequestHandler<GetTeamQuery, Team>
    {
        private readonly IApplicationDbContext _context;

        public GetTeamQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public Task<Team> Handle(GetTeamQuery request, CancellationToken cancellationToken)
        {
            return _context.Teams.Where(x => x.Id == request.Id).FirstOrDefaultAsync(cancellationToken);
        }
    }
}
