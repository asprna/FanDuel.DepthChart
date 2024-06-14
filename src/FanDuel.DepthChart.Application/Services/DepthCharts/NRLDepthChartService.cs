using FanDuel.DepthChart.Application.Contracts.Business;
using FanDuel.DepthChart.Domain.Dtos;
using FanDuel.DepthChart.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FanDuel.DepthChart.Application.Services.DepthCharts
{
    public class NRLDepthChartService : IDepthChartService
    {
        public Task AddPlayerToDepthChart(string Position, int PlayerId, int? rank, int? chartId)
        {
            throw new NotImplementedException();
        }

        public Task<int> CreateDepthChart(int TeamId, int? WeekId)
        {
            throw new NotImplementedException();
        }

        public Task<List<PlayerDto>> GetBackups(string Position, int PlayerId, int? chartId)
        {
            throw new NotImplementedException();
        }

        public Task<Dictionary<string, List<Player>>> GetFullDepthChart(int? chartId)
        {
            throw new NotImplementedException();
        }

        Task<PlayerDto> IDepthChartService.RemovePlayerFromDepthChart(string Position, int PlayerId, int? chartId)
        {
            throw new NotImplementedException();
        }
    }
}
