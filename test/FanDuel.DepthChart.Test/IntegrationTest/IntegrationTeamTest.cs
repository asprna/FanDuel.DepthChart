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
    public class IntegrationTeamTest : IClassFixture<InMemoryApiTestBase>
    {
        private readonly HttpClient _client;

        public IntegrationTeamTest(InMemoryApiTestBase testBase) => _client = testBase.Client;

        [Fact]
        public async Task ShouldBeAbleToAddATeamIfCommandIsValid()
        {
            //Assert
            int sportId = await _client.PostAsJsonAsync<AddSportsCommand, int>("Sport", new AddSportsCommand { Name = "NFL", Positions = new List<string> { "QB", "LWR" } });

            //Act
            var teamId = await _client.PostAsJsonAsync<AddTeamsCommand, int>("Team", new AddTeamsCommand { Name = "Buccaneers", SportId = sportId });

            //Assert
            Assert.True(teamId > 0);
        }

        [Fact]
        public async Task ShouldThrowValidationErrorIfSportIsInValid()
        {
            //Assert
            
            // Act & Assert
            var exception = await Assert.ThrowsAsync<JsonException>(async () =>
            {
                await _client.PostAsJsonAsync<AddTeamsCommand, int>("Team", new AddTeamsCommand { Name = "Buccaneers", SportId = 100 });

            });
        }

        [Fact]
        public async Task ShouldThrowValidationErrorIfTeamNameIsNullNull()
        {
            //Assert
            int sportId = await _client.PostAsJsonAsync<AddSportsCommand, int>("Sport", new AddSportsCommand { Name = "NFL", Positions = new List<string> { "QB", "LWR" } });

            // Act & Assert
            var exception = await Assert.ThrowsAsync<HttpRequestException>(async () =>
            {
                await _client.PostAsJsonAsync<AddTeamsCommand, int>("Team", new AddTeamsCommand { Name = string.Empty, SportId = sportId });

            });

            Assert.Equal(HttpStatusCode.UnprocessableEntity, exception.StatusCode);
        }

        [Fact]
        public async Task ShouldThrowValidationErrorIfTeamNameIsLargerThan50Chars()
        {
            //Assert
            int sportId = await _client.PostAsJsonAsync<AddSportsCommand, int>("Sport", new AddSportsCommand { Name = "NFL", Positions = new List<string> { "QB", "LWR" } });

            // Act & Assert
            var exception = await Assert.ThrowsAsync<HttpRequestException>(async () =>
            {
                await _client.PostAsJsonAsync<AddTeamsCommand, int>("Team", new AddTeamsCommand { 
                    Name = "sdfsdsdsdddddddddddddddddddddddddddddddddddddddddddddddddddddd", 
                    SportId = sportId 
                });

            });

            Assert.Equal(HttpStatusCode.UnprocessableEntity, exception.StatusCode);
        }

        [Fact]
        public async Task ShouldThrowValidationErrorIfSportIdIsInvalid()
        {
            //Assert
            int sportId = await _client.PostAsJsonAsync<AddSportsCommand, int>("Sport", new AddSportsCommand { Name = "NFL", Positions = new List<string> { "QB", "LWR" } });

            // Act & Assert
            var exception = await Assert.ThrowsAsync<HttpRequestException>(async () =>
            {
                await _client.PostAsJsonAsync<AddTeamsCommand, int>("Team", new AddTeamsCommand { Name = "Buccaneers", SportId = 0 });

            });

            Assert.Equal(HttpStatusCode.UnprocessableEntity, exception.StatusCode);
        }
    }
}
