using AutoMapper;
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

namespace FanDuel.DepthChart.Application.Features.DepthCharts.Commands
{
    public class AddDepthChartCommand : IRequest<TeamDepthChart>
    {
        public int WeekId { get; set; }
        public int TeamId { get; set; }
    }

    public class AddDepthChartCommandHandler : IRequestHandler<AddDepthChartCommand, TeamDepthChart>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<AddDepthChartCommandHandler> _logger;

        public AddDepthChartCommandHandler(IApplicationDbContext context, IMapper mapper, ILogger<AddDepthChartCommandHandler> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<TeamDepthChart> Handle(AddDepthChartCommand request, CancellationToken cancellationToken)
        {
            var depthChartForGivenWeek = await _context.TeamDepthCharts.FirstOrDefaultAsync(x => x.WeekId == request.WeekId && x.TeamId == request.TeamId);

            if (depthChartForGivenWeek != null) 
            { 
                return depthChartForGivenWeek; 
            }

            var depthChart = _mapper.Map<TeamDepthChart>(request);

            var newDepthChart = await _context.TeamDepthCharts.AddAsync(depthChart);

            var result = await _context.SaveChangesAsync(cancellationToken) > 0;

            if (!result)
            {
                _logger.LogError("Unable to save changes to DB");
                throw new DataContextException("Unable to save changes to DB");
            }

            return newDepthChart.Entity;
        }
    }
}
