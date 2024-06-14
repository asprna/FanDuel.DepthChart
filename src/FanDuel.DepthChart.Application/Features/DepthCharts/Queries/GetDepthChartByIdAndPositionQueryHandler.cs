using FanDuel.DepthChart.Application.Contracts.Persistence;
using FanDuel.DepthChart.Application.Exceptions;
using FanDuel.DepthChart.Application.Features.Teams.Commands;
using FanDuel.DepthChart.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace FanDuel.DepthChart.Application.Features.DepthCharts.Queries
{
    public class GetDepthChartByIdAndPositionQuery : IRequest<TeamDepthChart>
    {
        public int? ChartId { get; set; }
        public int PositionId { get; set; }
    }

    public class GetDepthChartByIdAndPositionQueryHandler(IApplicationDbContext context, ILogger<GetDepthChartByIdAndPositionQueryHandler> logger) 
        : IRequestHandler<GetDepthChartByIdAndPositionQuery, TeamDepthChart>
    {
        private readonly IApplicationDbContext _context = context;
        private readonly ILogger<GetDepthChartByIdAndPositionQueryHandler> _logger = logger;

        public async Task<TeamDepthChart> Handle(GetDepthChartByIdAndPositionQuery request, CancellationToken cancellationToken)
        {

            IQueryable<TeamDepthChart> query = _context.TeamDepthCharts
                .Include(tdc => tdc.Team)
                .Include(tdc => tdc.PlayerChartIndexs)
                    .ThenInclude(pci => pci.Position)
                .Include(tdc => tdc.PlayerChartIndexs)
                    .ThenInclude(pci => pci.Player);

            if (request.ChartId.HasValue)
            {
                query = query.Where(tdc => tdc.Id == request.ChartId.Value);
            }
            else
            {
                query = query.OrderByDescending(tdc => tdc.CreatedDateTimeUtc).Take(1);
            }

            var result = await query
                .Select(tdc => new TeamDepthChart
                {
                    Id = tdc.Id,
                    WeekId = tdc.WeekId,
                    CreatedDateTimeUtc = tdc.CreatedDateTimeUtc,
                    TeamId = tdc.TeamId,
                    Team = tdc.Team,
                    PlayerChartIndexs = tdc.PlayerChartIndexs
                        .Where(pci => pci.Position.Id == request.PositionId)
                        .ToList()
                })
                .FirstOrDefaultAsync(cancellationToken);

            if (result == null) 
            {
                _logger.LogError($"Unable to find Depth Chart for id {request.ChartId} and Position id {request.PositionId}");
                throw new NoContentException($"Unable to find Depth Chart for id {request.ChartId} and Position id {request.PositionId}");
            }

            return result;
        }
    }
}
