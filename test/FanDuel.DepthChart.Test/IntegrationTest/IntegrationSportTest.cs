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
    public class IntegrationSportTest : IClassFixture<InMemoryApiTestBase>
    {
        private readonly HttpClient _client;

        public IntegrationSportTest(InMemoryApiTestBase testBase) => _client = testBase.Client;

        [Fact]
        public async Task ShouldBeAbleToAddASportIfCommandIsValid()
        {
            //Assert
            var sportCommand = new AddSportsCommand { Name = "NFL", Positions = new List<string> { "QB", "LWR" } };

            //Act
            int sportId = await _client.PostAsJsonAsync<AddSportsCommand, int>("Sport", sportCommand);

            //Assert
            Assert.True(sportId > 0);
        }

        [Fact]
        public async Task ShouldThrowValidationErrorIfNameIsNull()
        {
            //Assert
            var sportCommand = new AddSportsCommand { Name = string.Empty, Positions = new List<string> { "QB", "LWR" } };

            // Act & Assert
            var exception = await Assert.ThrowsAsync<HttpRequestException>(async () =>
            {
                var response = await _client.PostAsJsonAsync<AddSportsCommand, int>("Sport", sportCommand);

            });

            Assert.Equal(HttpStatusCode.UnprocessableEntity, exception.StatusCode);
        }

        [Fact]
        public async Task ShouldThrowValidationErrorIfPoisitionsAreNull()
        {
            //Assert
            var sportCommand = new AddSportsCommand { Name = "NFL", Positions = new List<string>() };

            // Act & Assert
            var exception = await Assert.ThrowsAsync<HttpRequestException>(async () =>
            {
                var response = await _client.PostAsJsonAsync<AddSportsCommand, int>("Sport", sportCommand);

            });

            Assert.Equal(HttpStatusCode.UnprocessableEntity, exception.StatusCode);
        }

        [Fact]
        public async Task ShouldThrowValidationErrorIfPositionNameAreTooLongl()
        {
            //Assert
            var sportCommand = new AddSportsCommand { Name = string.Empty, Positions = new List<string> { "QB", "LWRT" } };

            // Act & Assert
            var exception = await Assert.ThrowsAsync<HttpRequestException>(async () =>
            {
                var response = await _client.PostAsJsonAsync<AddSportsCommand, int>("Sport", sportCommand);

            });

            Assert.Equal(HttpStatusCode.UnprocessableEntity, exception.StatusCode);
        }
    }
}
