using FanDuel.DepthChart.Application.Features.Players.Commands;
using FanDuel.DepthChart.Application.Features.Sports.Commands;
using FanDuel.DepthChart.Application.Features.Teams.Commands;
using FanDuel.DepthChart.Domain.Dtos;
using FanDuel.DepthChart.Domain.Entities;
using FanDuel.DepthChart.Test.Extensions;
using FanDuel.DepthChart.Test.Helper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestPlatform.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Numerics;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit.Abstractions;

namespace FanDuel.DepthChart.Test.IntegrationTest
{
    public class IntegrationPlayerTest : IClassFixture<InMemoryApiTestBase>
    {
        private readonly HttpClient _client;

        public IntegrationPlayerTest(InMemoryApiTestBase testBase) => _client = testBase.Client;

        [Fact]
        public async Task ShouldBeAbleToAddAPlayerIfCommandIsValid()
        {
            //Assert
            int sportId = await _client.PostAsJsonAsync<AddSportsCommand, int>("Sport", new AddSportsCommand { Name = "NFL", Positions = new List<string> { "QB", "LWR" } });
            var teamId = await _client.PostAsJsonAsync<AddTeamsCommand, int>("Team", new AddTeamsCommand { Name = "Buccaneers", SportId = sportId });
            var depthChartId = await _client.PostAsJsonAsync<AddDepthChartDto, int>("/NFL/CreateDepthChart", new AddDepthChartDto { TeamId = teamId, WeekId = null });

            //Act
            var playerAddResult = await _client.PostAsJsonAsync<AddPlayersCommand, int>("Player", new AddPlayersCommand { Number = 5, Name = "Ash", TeamId = teamId });

            //Assert
            Assert.True(playerAddResult > 0);
        }

        [Fact]
        public async Task ShouldThrowValidationErrorIfTeamIsInValid()
        {
            //Assert
            int sportId = await _client.PostAsJsonAsync<AddSportsCommand, int>("Sport", new AddSportsCommand { Name = "NFL", Positions = new List<string> { "QB", "LWR" } });
            var teamId = await _client.PostAsJsonAsync<AddTeamsCommand, int>("Team", new AddTeamsCommand { Name = "Buccaneers", SportId = sportId });
            var depthChartId = await _client.PostAsJsonAsync<AddDepthChartDto, int>("/NFL/CreateDepthChart", new AddDepthChartDto { TeamId = teamId, WeekId = null });

            // Act & Assert
            var exception = await Assert.ThrowsAsync<JsonException>(async () =>
            {
                await _client.PostAsJsonAsync<AddPlayersCommand, int>("Player", new AddPlayersCommand { Number = 5, Name = "Ash", TeamId = 100 });

            });
        }

        [Fact]
        public async Task ShouldThrowValidationErrorIfPlayerNumberIsNotUnique()
        {
            //Assert
            int sportId = await _client.PostAsJsonAsync<AddSportsCommand, int>("Sport", new AddSportsCommand { Name = "NFL", Positions = new List<string> { "QB", "LWR" } });
            var teamId = await _client.PostAsJsonAsync<AddTeamsCommand, int>("Team", new AddTeamsCommand { Name = "Buccaneers", SportId = sportId });
            var depthChartId = await _client.PostAsJsonAsync<AddDepthChartDto, int>("/NFL/CreateDepthChart", new AddDepthChartDto { TeamId = teamId, WeekId = null });
            await _client.PostAsJsonAsync<AddPlayersCommand, int>("Player", new AddPlayersCommand { Number = 5, Name = "Ash", TeamId = teamId });

            // Act & Assert
            var exception = await Assert.ThrowsAsync<HttpRequestException>(async () =>
            {
                await _client.PostAsJsonAsync<AddPlayersCommand, int>("Player", new AddPlayersCommand { Number = 5, Name = "Nawa", TeamId = teamId });

            });

            Assert.Equal(HttpStatusCode.Conflict, exception.StatusCode);
        }

        [Fact]
        public async Task ShouldThrowValidationErrorIfPlayerNameIsNull()
        {
            //Assert
            int sportId = await _client.PostAsJsonAsync<AddSportsCommand, int>("Sport", new AddSportsCommand { Name = "NFL", Positions = new List<string> { "QB", "LWR" } });
            var teamId = await _client.PostAsJsonAsync<AddTeamsCommand, int>("Team", new AddTeamsCommand { Name = "Buccaneers", SportId = sportId });
            var depthChartId = await _client.PostAsJsonAsync<AddDepthChartDto, int>("/NFL/CreateDepthChart", new AddDepthChartDto { TeamId = teamId, WeekId = null });
            
            // Act & Assert
            var exception = await Assert.ThrowsAsync<HttpRequestException>(async () =>
            {
                await _client.PostAsJsonAsync<AddPlayersCommand, int>("Player", new AddPlayersCommand { Number = 5, Name = string.Empty, TeamId = teamId });

            });

            Assert.Equal(HttpStatusCode.UnprocessableEntity, exception.StatusCode);
        }

        [Fact]
        public async Task ShouldThrowValidationErrorIfPlayerNumberIsInvalid()
        {
            //Assert
            int sportId = await _client.PostAsJsonAsync<AddSportsCommand, int>("Sport", new AddSportsCommand { Name = "NFL", Positions = new List<string> { "QB", "LWR" } });
            var teamId = await _client.PostAsJsonAsync<AddTeamsCommand, int>("Team", new AddTeamsCommand { Name = "Buccaneers", SportId = sportId });
            var depthChartId = await _client.PostAsJsonAsync<AddDepthChartDto, int>("/NFL/CreateDepthChart", new AddDepthChartDto { TeamId = teamId, WeekId = null });

            // Act & Assert
            var exception = await Assert.ThrowsAsync<HttpRequestException>(async () =>
            {
                await _client.PostAsJsonAsync<AddPlayersCommand, int>("Player", new AddPlayersCommand { Number = 0, Name = "Ash", TeamId = teamId });

            });

            Assert.Equal(HttpStatusCode.UnprocessableEntity, exception.StatusCode);
        }

        [Fact]
        public async Task ShouldThrowValidationErrorIfTeamIsIsInvalid()
        {
            //Assert
            int sportId = await _client.PostAsJsonAsync<AddSportsCommand, int>("Sport", new AddSportsCommand { Name = "NFL", Positions = new List<string> { "QB", "LWR" } });
            var teamId = await _client.PostAsJsonAsync<AddTeamsCommand, int>("Team", new AddTeamsCommand { Name = "Buccaneers", SportId = sportId });
            var depthChartId = await _client.PostAsJsonAsync<AddDepthChartDto, int>("/NFL/CreateDepthChart", new AddDepthChartDto { TeamId = teamId, WeekId = null });

            // Act & Assert
            var exception = await Assert.ThrowsAsync<HttpRequestException>(async () =>
            {
                await _client.PostAsJsonAsync<AddPlayersCommand, int>("Player", new AddPlayersCommand { Number = 5, Name = "Ash", TeamId = 0 });

            });

            Assert.Equal(HttpStatusCode.UnprocessableEntity, exception.StatusCode);
        }
    }
}
