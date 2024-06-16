using FanDuel.DepthChart.Application.Contracts.Persistence;
using FanDuel.DepthChart.Application.Features.Teams.Queries;
using FanDuel.DepthChart.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FanDuel.DepthChart.Application.Features.Sports.Queries
{
    public class GetSportByNameQuery : IRequest<Sport>
    {
        public string Name { get; set; }
    }

    /// <summary>
    /// This handler is responsible for managing all the logic related to the 'Finding the correct sport for the given name' feature.
    /// </summary>
    public class GetSportByNameQueryHandler : IRequestHandler<GetSportByNameQuery, Sport>
    {
        private readonly IApplicationDbContext _context;

        public GetSportByNameQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public Task<Sport> Handle(GetSportByNameQuery request, CancellationToken cancellationToken)
        {
            return _context.Sports.Where(x => x.Name == request.Name).FirstOrDefaultAsync(cancellationToken);
        }
    }
}
