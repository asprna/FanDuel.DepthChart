﻿using AutoMapper;
using FanDuel.DepthChart.Application.Contracts.Business;
using FanDuel.DepthChart.Application.Contracts.Persistence;
using FanDuel.DepthChart.Application.Exceptions;
using FanDuel.DepthChart.Application.Features.DepthCharts.Commands;
using FanDuel.DepthChart.Application.Features.DepthCharts.Queries;
using FanDuel.DepthChart.Application.Features.Players.Queries;
using FanDuel.DepthChart.Application.Features.Sports.Queries;
using FanDuel.DepthChart.Application.Features.Teams.Queries;
using FanDuel.DepthChart.Domain.Dtos;
using FanDuel.DepthChart.Domain.Entities;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FanDuel.DepthChart.Application.Services.DepthCharts
{
    public class NFLDepthChartService : IDepthChartService
    {
        public NFLDepthChartService(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }
        private const string SPORT = "NFL";
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public async Task AddPlayerToDepthChart(string Position, int PlayerId, int? rank, int? chartId)
        {
            var player = await _mediator.Send(new GetPlayerByIdQuery { Id = PlayerId });

            if (!player.Team.Sport.Positions.Any(p => p.Name == Position))
            {
                throw new BadRequestException("Position invalid for the given player");
            }

            var positionId = player.Team.Sport.Positions.Where(p => p.Name == Position).First().Id;

            var chart = await _mediator.Send(new GetDepthChartByIdAndPositionQuery { ChartId = chartId, PositionId = positionId }) ?? throw new NoContentException($"Chart not found");
            
            //If the Rank is null and Player is already at the bottom, We do not want to change anything
            if (rank == null &&
                chart.PlayerChartIndexs.Where(p => p.PayerId == PlayerId).FirstOrDefault().Rank == chart.PlayerChartIndexs.Where(i => i.PositionId == positionId).Max(pci => pci.Rank)
            )
            {
                return;
            }

            var updateChartRequest = new UpdatePlayerPositionIndexCommand { ChartId = chart.Id, PositionId = positionId, PlayerId = PlayerId, Rank = rank };
            await _mediator.Send(updateChartRequest);
        }

        /// <inheritdoc />
        public async Task<int> CreateDepthChart(int TeamId, int? WeekId)
        {
            //Check if the team exists
            var team = await _mediator.Send(new GetTeamQuery { Id = TeamId })
                ?? throw new NoContentException("Team not found");

            //Identify the week number, this could be different for other sports
            var weeknumber = WeekId ?? GetWeekNumber(DateTime.UtcNow);

            //Get the sport Id
            var sport = await _mediator.Send(new GetSportByNameQuery { Name = SPORT })
                ?? throw new NoContentException("Sport not found");

            var result = await _mediator.Send(new AddDepthChartCommand { TeamId = team.Id, WeekId = weeknumber });

            return result.Id;
        }

        public Task<List<Player>> GetBackups(string Position, int PlayerId, int? chartId)
        {
            throw new NotImplementedException();
        }

        public Task<Dictionary<string, List<Player>>> GetFullDepthChart(int? chartId)
        {
            throw new NotImplementedException();
        }

        public async Task<PlayerDto> RemovePlayerFromDepthChart(string Position, int PlayerId, int? chartId)
        {
            var player = await _mediator.Send(new GetPlayerByIdQuery { Id = PlayerId });

            if (!player.Team.Sport.Positions.Any(p => p.Name == Position))
            {
                throw new BadRequestException($"Position {Position} invalid for the player id {PlayerId}");
            }

            var positionId = player.Team.Sport.Positions.First(p => p.Name == Position).Id;

            var chart = await _mediator.Send(new GetDepthChartByIdAndPositionQuery { ChartId = chartId, PositionId = positionId }) ?? throw new NoContentException($"Chart not found");

            await _mediator.Send(new RemovePlayerFromDepthChartCommand { ChartId = chart.Id, PlayerId = PlayerId, PositionId = positionId });

            return _mapper.Map<PlayerDto>(player);
        }

        private static int GetWeekNumber(DateTime date)
        {
            // Get the calendar instance associated with the current culture.
            Calendar calendar = CultureInfo.CurrentCulture.Calendar;

            // Specify the CalendarWeekRule and the first day of the week according to your preference.
            CalendarWeekRule weekRule = CultureInfo.CurrentCulture.DateTimeFormat.CalendarWeekRule;
            DayOfWeek firstDayOfWeek = CultureInfo.CurrentCulture.DateTimeFormat.FirstDayOfWeek;

            // Get the week number.
            int weekNumber = calendar.GetWeekOfYear(date, weekRule, firstDayOfWeek);

            return weekNumber;
        }
    }
}
