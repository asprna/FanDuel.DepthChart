using FanDuel.DepthChart.Application.Contracts.Business;
using FanDuel.DepthChart.Application.Contracts.Persistence;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FanDuel.DepthChart.Application.Services.DepthCharts
{
    public class DepthChartFactory : IDepthChartFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public DepthChartFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public IDepthChart CreateDepthChart(string sportType)
        {
            switch (sportType)
            {
                case "NFL":
                    return new NFLDepthChart(_serviceProvider.GetService<IMediator>());
                case "NRL":
                    return new NRLDepthChart();
                default:
                    throw new ArgumentException($"No implementation found for depth chart type: {sportType}");
            }
        }
    }
}
