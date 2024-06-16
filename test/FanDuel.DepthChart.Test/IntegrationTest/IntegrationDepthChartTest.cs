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
    public class IntegrationDepthChartTest : IClassFixture<InMemoryApiTestBase>
    {
        private readonly HttpClient _client;

        public IntegrationDepthChartTest(InMemoryApiTestBase testBase) => _client = testBase.Client;

        [Fact]
        public async Task ShouldBeAbleToAddDepthChartIfCommandIsValid()
        {
            //Assert
            int sportId = await _client.PostAsJsonAsync<AddSportsCommand, int>("Sport", new AddSportsCommand { Name = "NFL", Positions = new List<string> { "QB", "LWR" } });
            var teamId = await _client.PostAsJsonAsync<AddTeamsCommand, int>("Team", new AddTeamsCommand { Name = "Buccaneers", SportId = sportId });

            //Act
            var depthChartId = await _client.PostAsJsonAsync<AddDepthChartDto, int>("/NFL/CreateDepthChart", new AddDepthChartDto { TeamId = teamId, WeekId = 1 });

            //Assert
            Assert.True(depthChartId > 0);
        }

        [Fact]
        public async Task ShouldBeAbleToCalculateWeekIdIfNotProvided()
        {
            //Assert
            int sportId = await _client.PostAsJsonAsync<AddSportsCommand, int>("Sport", new AddSportsCommand { Name = "NFL", Positions = new List<string> { "QB", "LWR" } });
            var teamId = await _client.PostAsJsonAsync<AddTeamsCommand, int>("Team", new AddTeamsCommand { Name = "Buccaneers", SportId = sportId });

            //Act
            var depthChartId = await _client.PostAsJsonAsync<AddDepthChartDto, int>("/NFL/CreateDepthChart", new AddDepthChartDto { TeamId = teamId, WeekId = null });

            //Assert
            Assert.True(depthChartId > 0);
        }

        [Fact]
        public async Task ShouldNotDuplecateDepthChartForTheSameWeek()
        {
            //Assert
            int sportId = await _client.PostAsJsonAsync<AddSportsCommand, int>("Sport", new AddSportsCommand { Name = "NFL", Positions = new List<string> { "QB", "LWR" } });
            var teamId = await _client.PostAsJsonAsync<AddTeamsCommand, int>("Team", new AddTeamsCommand { Name = "Buccaneers", SportId = sportId });

            //Act
            var depthChartId1 = await _client.PostAsJsonAsync<AddDepthChartDto, int>("/NFL/CreateDepthChart", new AddDepthChartDto { TeamId = teamId, WeekId = null });
            var depthChartId2 = await _client.PostAsJsonAsync<AddDepthChartDto, int>("/NFL/CreateDepthChart", new AddDepthChartDto { TeamId = teamId, WeekId = null });

            //Assert
            Assert.Equal(depthChartId1, depthChartId2);
        }

        [Fact]
        public async Task ShouldNotDuplecateDepthChartForTheGivenSameWeek()
        {
            //Assert
            int sportId = await _client.PostAsJsonAsync<AddSportsCommand, int>("Sport", new AddSportsCommand { Name = "NFL", Positions = new List<string> { "QB", "LWR" } });
            var teamId = await _client.PostAsJsonAsync<AddTeamsCommand, int>("Team", new AddTeamsCommand { Name = "Buccaneers", SportId = sportId });

            //Act
            var depthChartId1 = await _client.PostAsJsonAsync<AddDepthChartDto, int>("/NFL/CreateDepthChart", new AddDepthChartDto { TeamId = teamId, WeekId = 50 });
            var depthChartId2 = await _client.PostAsJsonAsync<AddDepthChartDto, int>("/NFL/CreateDepthChart", new AddDepthChartDto { TeamId = teamId, WeekId = 50 });

            //Assert
            Assert.Equal(depthChartId1, depthChartId2);
        }
    }
}
