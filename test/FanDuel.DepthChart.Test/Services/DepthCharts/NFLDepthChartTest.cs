﻿using FanDuel.DepthChart.Application.Exceptions;
using FanDuel.DepthChart.Application.Features.DepthCharts.Commands;
using FanDuel.DepthChart.Application.Features.Players.Commands;
using FanDuel.DepthChart.Application.Features.Sports.Queries;
using FanDuel.DepthChart.Application.Features.Teams.Queries;
using FanDuel.DepthChart.Application.Services.DepthCharts;
using FanDuel.DepthChart.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FanDuel.DepthChart.Test.Services.DepthCharts
{
    public class NFLDepthChartTest
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly NFLDepthChart _nflDepthChart;


        public NFLDepthChartTest()
        {
            _mediatorMock = new Mock<IMediator>();
            _nflDepthChart = new NFLDepthChart(_mediatorMock.Object);
        }

        [Fact]
        public async Task CreateDepthChart_ShouldReturnDepthChartId_WhenTeamAndSportExist()
        {
            // Arrange
            var teamId = 1;
            var weekId = 1;
            var expectedDepthChartId = 123;

            _mediatorMock.Setup(m => m.Send(It.IsAny<GetTeamQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Team { Id = teamId });

            _mediatorMock.Setup(m => m.Send(It.IsAny<GetSportByNameQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Sport { Name = "NFL" });

            _mediatorMock.Setup(m => m.Send(It.IsAny<AddDepthChartCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new TeamDepthChart { Id = expectedDepthChartId });

            // Act
            var result = await _nflDepthChart.CreateDepthChart(teamId, weekId);

            // Assert
            Assert.Equal(expectedDepthChartId, result);
        }

        [Fact]
        public async Task CreateDepthChart_ShouldThrowNoContentException_WhenTeamDoesNotExist()
        {
            // Arrange
            var teamId = 1;
            var weekId = 1;

            _mediatorMock.Setup(m => m.Send(It.IsAny<GetTeamQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((Team)null);

            // Act & Assert
            await Assert.ThrowsAsync<NoContentException>(() => _nflDepthChart.CreateDepthChart(teamId, weekId));
        }

        [Fact]
        public async Task CreateDepthChart_ShouldThrowNoContentException_WhenSportDoesNotExist()
        {
            // Arrange
            var teamId = 1;
            var weekId = 1;

            _mediatorMock.Setup(m => m.Send(It.IsAny<GetTeamQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Team { Id = teamId });

            _mediatorMock.Setup(m => m.Send(It.IsAny<GetSportByNameQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((Sport)null);

            // Act & Assert
            await Assert.ThrowsAsync<NoContentException>(() => _nflDepthChart.CreateDepthChart(teamId, weekId));
        }

        [Fact]
        public async Task CreateDepthChart_ShouldUseCurrentWeekNumber_WhenWeekIdIsNull()
        {
            // Arrange
            var teamId = 1;
            int? weekId = null;  // No WeekId provided
            var expectedWeekNumber = GetWeekNumber(DateTime.UtcNow);

            _mediatorMock.Setup(m => m.Send(It.IsAny<GetTeamQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Team { Id = teamId });

            _mediatorMock.Setup(m => m.Send(It.IsAny<GetSportByNameQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Sport { Name = "NFL" });

            _mediatorMock.Setup(m => m.Send(It.IsAny<AddDepthChartCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new TeamDepthChart { Id = 123 });

            // Act
            var result = await _nflDepthChart.CreateDepthChart(teamId, weekId);

            // Assert
            _mediatorMock.Verify(m => m.Send(It.Is<AddDepthChartCommand>(cmd => cmd.WeekId == expectedWeekNumber), It.IsAny<CancellationToken>()), Times.Once);

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
