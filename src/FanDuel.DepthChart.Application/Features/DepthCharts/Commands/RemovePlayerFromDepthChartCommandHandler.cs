using FanDuel.DepthChart.Application.Contracts.Persistence;
using FanDuel.DepthChart.Application.Exceptions;
using FanDuel.DepthChart.Application.Features.DepthCharts.Queries;
using FanDuel.DepthChart.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FanDuel.DepthChart.Application.Features.DepthCharts.Commands
{
    public class RemovePlayerFromDepthChartCommand : IRequest
    {
        public int ChartId { get; set; }
        public int PlayerId { get; set; }
        public int PositionId { get; set; }
    }

    public class RemovePlayerFromDepthChartCommandHandler(IApplicationDbContext context, ILogger<GetDepthChartByIdAndPositionQueryHandler> logger)
        : IRequestHandler<RemovePlayerFromDepthChartCommand>
    {
        private readonly IApplicationDbContext _context = context;
        private readonly ILogger<GetDepthChartByIdAndPositionQueryHandler> _logger = logger;

        public async Task Handle(RemovePlayerFromDepthChartCommand request, CancellationToken cancellationToken)
        {
            var chart = await _context.TeamDepthCharts
                .Include(c => c.PlayerChartIndexs)
                .Where(i => i.Id == request.ChartId && i.PlayerChartIndexs.Any(p => p.PositionId == request.PositionId))
                .FirstOrDefaultAsync(cancellationToken);

            if (chart == null) 
            {
                _logger.LogError($"Unable to find Depth Chart for id {request.ChartId} and Position id {request.PositionId}");
                throw new NoContentException($"Unable to find Depth Chart for id {request.ChartId} and Position id {request.PositionId}");
            }

            var playerChartIndex = chart.PlayerChartIndexs.FirstOrDefault(pci => pci.PayerId == request.PlayerId);

            if (playerChartIndex != null)
            {
                chart.PlayerChartIndexs.Remove(playerChartIndex);
            }

            //Reorder the player index
            int newIndex = 1;
            foreach (var pci in chart.PlayerChartIndexs.OrderBy(pci => pci.Rank))
            {
                pci.Rank = newIndex;
                newIndex++;
            }

            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
