using AutoMapper;
using FanDuel.DepthChart.Application.Features.DepthCharts.Commands;
using FanDuel.DepthChart.Application.Features.Players.Commands;
using FanDuel.DepthChart.Application.Features.Sports.Commands;
using FanDuel.DepthChart.Application.Features.Teams.Commands;
using FanDuel.DepthChart.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FanDuel.DepthChart.Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<AddSportsCommand, Sport>()
            .ForMember(dest => dest.Positions, opt => opt.MapFrom(src => src.Positions.Select(p => new Position { Name = p }).ToList()))
            .ForMember(dest => dest.Id, opt => opt.Ignore());

            CreateMap<AddTeamsCommand, Team>();

            CreateMap<AddPlayersCommand, Player>();

            CreateMap<AddDepthChartCommand, TeamDepthChart>();
        }
    }
}
