using FanDuel.DepthChart.Application.Contracts.Persistence;
using FanDuel.DepthChart.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FanDuel.DepthChart.Application.Features.DepthCharts.Queries
{
    public class GetLatestChartByPositionIdQuery : IRequest<TeamDepthChart>
    {
        public int PositionId { get; set; }
    }

    public class GetLatestChartByPositionQueryHandler : IRequestHandler<GetLatestChartByPositionIdQuery, TeamDepthChart>
    {
        private readonly IApplicationDbContext _context;

        public GetLatestChartByPositionQueryHandler(IApplicationDbContext context) => _context = context;

        public Task<TeamDepthChart> Handle(GetLatestChartByPositionIdQuery request, CancellationToken cancellationToken)
        {
            return _context.TeamDepthCharts
                .Include(i => i.PlayerChartIndexs)
                //.Where(t => t.PlayerChartIndexs.Any(pci => pci.PositionId == request.PositionId))
                .OrderByDescending(t => t.CreatedDateTimeUtc)
                .FirstOrDefaultAsync();
        }
    }
}
