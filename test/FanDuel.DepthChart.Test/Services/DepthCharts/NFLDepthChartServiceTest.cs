using AutoMapper;
using FanDuel.DepthChart.Application.Contracts.Business;
using FanDuel.DepthChart.Application.Exceptions;
using FanDuel.DepthChart.Application.Features.DepthCharts.Commands;
using FanDuel.DepthChart.Application.Features.DepthCharts.Queries;
using FanDuel.DepthChart.Application.Features.Players.Commands;
using FanDuel.DepthChart.Application.Features.Players.Queries;
using FanDuel.DepthChart.Application.Features.Sports.Queries;
using FanDuel.DepthChart.Application.Features.Teams.Queries;
using FanDuel.DepthChart.Application.Services.DepthCharts;
using FanDuel.DepthChart.Domain.Dtos;
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
    public class NFLDepthChartServiceTest
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly NFLDepthChartService _nflDepthChart;
        private readonly Mock<IMapper> _mapperMock;

        public NFLDepthChartServiceTest()
        {
            _mediatorMock = new Mock<IMediator>();
            _mapperMock = new Mock<IMapper>();
            _nflDepthChart = new NFLDepthChartService(_mediatorMock.Object, _mapperMock.Object);
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

        [Fact]
        public async Task AddPlayerToDepthChart_ShouldThrowBadRequestException_WhenPositionIsInvalid()
        {
            // Arrange
            var player = new Player
            {
                Team = new Team
                {
                    Sport = new Sport
                    {
                        Positions = new List<Position>()
                    }
                }
            };
            _mediatorMock.Setup(m => m.Send(It.IsAny<GetPlayerByIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(player);

            // Act & Assert
            await Assert.ThrowsAsync<BadRequestException>(() =>
                _nflDepthChart.AddPlayerToDepthChart("InvalidPosition", 1, null, null));
        }

        [Fact]
        public async Task AddPlayerToDepthChart_ShouldThrowNoContentException_WhenChartNotFound()
        {
            // Arrange
            var player = new Player
            {
                Team = new Team
                {
                    Sport = new Sport
                    {
                        Positions = new List<Position> { new Position { Name = "QB" } }
                    }
                }
            };
            _mediatorMock.Setup(m => m.Send(It.IsAny<GetPlayerByIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(player);
            _mediatorMock.Setup(m => m.Send(It.IsAny<GetLatestChartByPositionIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((TeamDepthChart)null);

            // Act & Assert
            await Assert.ThrowsAsync<NoContentException>(() =>
                _nflDepthChart.AddPlayerToDepthChart("QB", 1, null, null));
        }

        [Fact]
        public async Task AddPlayerToDepthChart_ShouldNotUpdateRank_WhenPlayerAlreadyAtBottom()
        {
            // Arrange
            var player = new Player
            {
                Team = new Team
                {
                    Sport = new Sport
                    {
                        Positions = new List<Position> { new Position { Name = "QB", Id = 1 } }
                    }
                }
            };
            var chart = new TeamDepthChart
            {
                PlayerChartIndexs = new List<PlayerChartIndex>
            {
                new PlayerChartIndex { PayerId = 1, Rank = 1, PositionId = 1 },
                new PlayerChartIndex { PayerId = 2, Rank = 2, PositionId = 1 }
            }
            };
            _mediatorMock.Setup(m => m.Send(It.IsAny<GetPlayerByIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(player);
            _mediatorMock.Setup(m => m.Send(It.IsAny<GetDepthChartByIdAndPositionQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(chart);

            // Act
            await _nflDepthChart.AddPlayerToDepthChart("QB", 2, null, null);

            // Assert
            _mediatorMock.Verify(m => m.Send(It.IsAny<UpdatePlayerPositionIndexCommand>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task AddPlayerToDepthChart_ShouldUpdateRank_WhenRankIsProvided()
        {
            // Arrange
            var player = new Player
            {
                Team = new Team
                {
                    Sport = new Sport
                    {
                        Positions = new List<Position> { new Position { Name = "QB", Id = 1 } }
                    }
                }
            };
            var chart = new TeamDepthChart
            {
                Id = 1,
                PlayerChartIndexs = new List<PlayerChartIndex>()
            };
            _mediatorMock.Setup(m => m.Send(It.IsAny<GetPlayerByIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(player);
            _mediatorMock.Setup(m => m.Send(It.IsAny<GetDepthChartByIdAndPositionQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(chart);

            // Act
            await _nflDepthChart.AddPlayerToDepthChart("QB", 1, 2, null);

            // Assert
            _mediatorMock.Verify(m => m.Send(It.Is<UpdatePlayerPositionIndexCommand>(cmd =>
                cmd.ChartId == 1 && cmd.PositionId == 1 && cmd.PlayerId == 1 && cmd.Rank == 2), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task RemovePlayerFromDepthChart_ShouldThrowBadRequestException_WhenPositionIsInvalid()
        {
            // Arrange
            var playerId = 1;
            var chartId = (int?)1;
            var position = "WR";

            var player = new Player
            {
                Id = playerId,
                Team = new Team
                {
                    Sport = new Sport
                    {
                        Positions = new List<Position>
                    {
                        new Position { Name = "QB" } // Position "WR" is missing
                    }
                    }
                }
            };

            _mediatorMock.Setup(m => m.Send(It.IsAny<GetPlayerByIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(player);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<BadRequestException>(() => _nflDepthChart.RemovePlayerFromDepthChart(position, playerId, chartId));
            Assert.Equal($"Position {position} invalid for the player id {playerId}", exception.Message);
        }

        [Fact]
        public async Task RemovePlayerFromDepthChart_ShouldThrowNoContentException_WhenChartIsNotFound()
        {
            // Arrange
            var playerId = 1;
            var chartId = (int?)1;
            var position = "WR";

            var player = new Player
            {
                Id = playerId,
                Team = new Team
                {
                    Sport = new Sport
                    {
                        Positions = new List<Position>
                    {
                        new Position { Name = position }
                    }
                    }
                }
            };

            _mediatorMock.Setup(m => m.Send(It.IsAny<GetPlayerByIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(player);

            _mediatorMock.Setup(m => m.Send(It.IsAny<GetDepthChartByIdAndPositionQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((TeamDepthChart)null);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<NoContentException>(() => _nflDepthChart.RemovePlayerFromDepthChart(position, playerId, chartId));
            Assert.Equal("Chart not found", exception.Message);
        }

        [Fact]
        public async Task RemovePlayerFromDepthChart_ShouldRemovePlayer_WhenValidRequest()
        {
            // Arrange
            var playerId = 1;
            var chartId = (int?)1;
            var position = "WR";

            var player = new Player
            {
                Id = playerId,
                Team = new Team
                {
                    Sport = new Sport
                    {
                        Positions = new List<Position>
                    {
                        new Position { Name = position, Id = 1 }
                    }
                    }
                }
            };

            var chart = new TeamDepthChart
            {
                Id = 1,
                PlayerChartIndexs = new List<PlayerChartIndex>
            {
                new PlayerChartIndex { PayerId = playerId, PositionId = 1 }
            }
            };

            _mediatorMock.Setup(m => m.Send(It.IsAny<GetPlayerByIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(player);

            _mediatorMock.Setup(m => m.Send(It.IsAny<GetDepthChartByIdAndPositionQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(chart);

            _mapperMock.Setup(m => m.Map<PlayerDto>(It.IsAny<Player>()))
                .Returns(new PlayerDto { Id = playerId });

            // Act
            var result = await _nflDepthChart.RemovePlayerFromDepthChart(position, playerId, chartId);

            // Assert
            _mediatorMock.Verify(m => m.Send(It.IsAny<RemovePlayerFromDepthChartCommand>(), It.IsAny<CancellationToken>()), Times.Once);
            Assert.Equal(playerId, result.Id);
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
