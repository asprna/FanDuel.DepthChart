﻿using FanDuel.DepthChart.Application.Contracts.Business;
using FanDuel.DepthChart.Application.Contracts.Persistence;
using FanDuel.DepthChart.Application.Exceptions;
using FanDuel.DepthChart.Application.Features.DepthCharts.Commands;
using FanDuel.DepthChart.Application.Features.Sports.Queries;
using FanDuel.DepthChart.Application.Features.Teams.Queries;
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
    public class NFLDepthChart : IDepthChart
    {
        public NFLDepthChart(IMediator mediator)
        {
            _mediator = mediator;
        }
        private const string SPORT = "NFL";
        private readonly IMediator _mediator;

        public Task AddPlayerToDepthChart(string Position, int PlayerId, int? rank, int? chartId)
        {
            throw new NotImplementedException();
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

        public Task RemovePlayerFromDepthChart(string Position, int PlayerId, int? chartId)
        {
            throw new NotImplementedException();
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