using AutoMapper;
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
    public class DepthChartServiceFactory : IDepthChartServiceFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public DepthChartServiceFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        /// <summary>
        /// This factory class accommodates depth chart service creation based on the configuration below.
        /// </summary>
        /// <param name="sportType"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public IDepthChartService CreateDepthChart(string sportType)
        {
            switch (sportType)
            {
                case "NFL":
                    return new NFLDepthChartService(_serviceProvider.GetService<IMediator>(), _serviceProvider.GetService<IMapper>());
                case "NRL":
                    return new NRLDepthChartService();
                default:
                    throw new ArgumentException($"No implementation found for depth chart type: {sportType}");
            }
        }
    }
}
