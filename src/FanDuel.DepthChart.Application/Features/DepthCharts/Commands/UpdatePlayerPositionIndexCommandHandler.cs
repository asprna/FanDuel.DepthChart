using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FanDuel.DepthChart.Application.Contracts.Persistence;
using FanDuel.DepthChart.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FanDuel.DepthChart.Application.Features.DepthCharts.Commands
{
    public class UpdatePlayerPositionIndexCommand : IRequest
    {
        public int ChartId { get; set; }
        public int PositionId { get; set; }
        public int PlayerId { get; set; }
        public int? Rank { get; set; }
    }

    public class UpdatePlayerPositionIndexCommandHandler : IRequestHandler<UpdatePlayerPositionIndexCommand>
    {
        private readonly IApplicationDbContext _context;

        public UpdatePlayerPositionIndexCommandHandler(IApplicationDbContext context) => _context = context;

        public async Task Handle(UpdatePlayerPositionIndexCommand request, CancellationToken cancellationToken)
        {
            var teamDepthChart = await _context.TeamDepthCharts
                .Include(i => i.PlayerChartIndexs)
                .Where(x => x.Id == request.ChartId)
                .FirstOrDefaultAsync();

            if (request.Rank == null)
            {
                request.Rank = teamDepthChart.PlayerChartIndexs.Any(i => i.PositionId == request.PositionId)
                    ? teamDepthChart.PlayerChartIndexs.Where(i => i.PositionId == request.PositionId).Max(pci => pci.Rank) + 1
                    : 1;
            }

            int newIndexAddition = 1;
            foreach (
                var pci in teamDepthChart.PlayerChartIndexs
                .Where(pci => pci.Rank >= request.Rank && pci.PositionId == request.PositionId && pci.PayerId != request.PlayerId)
                .OrderBy(pci => pci.Rank)
             )
            {
                pci.Rank = (int)request.Rank + newIndexAddition;
                newIndexAddition++;
            }

            //If Player is already exists, do not add
            if (!teamDepthChart.PlayerChartIndexs.Any(i => i.PositionId == request.PositionId && i.PayerId == request.PlayerId))
            {
                var newPlayerIndex = new PlayerChartIndex
                {
                    PayerId = request.PlayerId,
                    PositionId = request.PositionId,
                    Rank = (int)request.Rank
                };

                teamDepthChart.PlayerChartIndexs.Add(newPlayerIndex);
            }
            else
            {
                teamDepthChart.PlayerChartIndexs
                    .Where(i => i.PositionId == request.PositionId && i.PayerId == request.PlayerId)
                    .FirstOrDefault()
                    .Rank = (int)request.Rank;
            }
            
            //AddPlayerChartIndex(teamDepthChart, newPlayerIndex, request.Rank, request.PositionId);

            await _context.SaveChangesAsync(cancellationToken);
        }

        public static void AddPlayerChartIndex(TeamDepthChart teamDepthChart, PlayerChartIndex newPlayerChartIndex, int? newRank, int positionId)
        {
            if (newRank == null)
            {
                newRank = teamDepthChart.PlayerChartIndexs.Any(i => i.PositionId == positionId)
                    ? teamDepthChart.PlayerChartIndexs.Where(i => i.PositionId == positionId).Max(pci => pci.Rank) + 1
                    : 1;
            }

            newPlayerChartIndex.Rank = newRank.Value;

            foreach (var pci in teamDepthChart.PlayerChartIndexs.Where(pci => pci.Rank >= newRank.Value && pci.PositionId == positionId).OrderBy(pci => pci.Rank))
            {
                pci.Rank++;
            }

            teamDepthChart.PlayerChartIndexs.Add(newPlayerChartIndex);
        }
    }

}
