using FanDuel.DepthChart.Application.Contracts.Persistence;
using FanDuel.DepthChart.Application.Exceptions;
using FanDuel.DepthChart.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FanDuel.DepthChart.Application.Features.DepthCharts.Queries
{
    public class GetDepthChartByIdQuery : IRequest<TeamDepthChart>
    {
        public int? ChartId { get; set; }
    }

    public class GetDepthChartByIdQueryHandler(IApplicationDbContext context, ILogger<GetDepthChartByIdAndPositionQueryHandler> logger)
        : IRequestHandler<GetDepthChartByIdQuery, TeamDepthChart>
    {
        private readonly IApplicationDbContext _context = context;
        private readonly ILogger<GetDepthChartByIdAndPositionQueryHandler> _logger = logger;

        public async Task<TeamDepthChart> Handle(GetDepthChartByIdQuery request, CancellationToken cancellationToken)
        {
            IQueryable<TeamDepthChart> query = _context.TeamDepthCharts
                .Include(tdc => tdc.Team)
                .Include(tdc => tdc.PlayerChartIndexs)
                    .ThenInclude(pci => pci.Player)
                .Include(tdc => tdc.PlayerChartIndexs)
                    .ThenInclude(pci => pci.Position);

            if (request.ChartId.HasValue)
            {
                query = query.Where(tdc => tdc.Id == request.ChartId.Value);
            }
            else
            {
                query = query.OrderByDescending(tdc => tdc.CreatedDateTimeUtc).Take(1);
            }

            var result = await query.FirstOrDefaultAsync(cancellationToken);

            if (result == null)
            {
                _logger.LogError($"Unable to find Depth Chart for id {request.ChartId}");
                throw new NoContentException($"Unable to find Depth Chart for id {request.ChartId}");
            }

            return result;
        }
    }
}
