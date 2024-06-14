using FanDuel.DepthChart.Application.Contracts.Persistence;
using FanDuel.DepthChart.Application.Exceptions;
using FanDuel.DepthChart.Application.Features.Sports.Queries;
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
    public class GetChartByIdAndPositionIdQuery : IRequest<TeamDepthChart>
    {
        public int? ChartId { get; set; }
        public int PositionId { get; set; }
    }

    public class GetChartByIdAndPositionQueryHandler : IRequestHandler<GetChartByIdAndPositionIdQuery, TeamDepthChart>
    {
        private readonly IApplicationDbContext _context;

        public GetChartByIdAndPositionQueryHandler(IApplicationDbContext context) => _context = context;

        public Task<TeamDepthChart> Handle(GetChartByIdAndPositionIdQuery request, CancellationToken cancellationToken)
        {
            return _context.TeamDepthCharts
               .Include(i => i.PlayerChartIndexs)
               .Where(t => t.Id == request.ChartId)
               .FirstOrDefaultAsync(cancellationToken);
        }
    }
}
