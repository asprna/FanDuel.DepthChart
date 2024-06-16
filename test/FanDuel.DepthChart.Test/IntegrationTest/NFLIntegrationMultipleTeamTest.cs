using FanDuel.DepthChart.Application.Features.Players.Commands;
using FanDuel.DepthChart.Application.Features.Sports.Commands;
using FanDuel.DepthChart.Application.Features.Teams.Commands;
using FanDuel.DepthChart.Domain.Dtos;
using FanDuel.DepthChart.Domain.Entities;
using FanDuel.DepthChart.Test.Extensions;
using FanDuel.DepthChart.Test.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Numerics;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace FanDuel.DepthChart.Test.IntegrationTest
{
    public class NFLIntegrationMultipleTeamTest : IClassFixture<InMemoryApiTestBase>
    {
        private readonly HttpClient _client;
        public NFLIntegrationMultipleTeamTest(InMemoryApiTestBase testBase) => _client = testBase.Client;

        [Fact]
        public async Task NFL_MultipleTeam_CurrentWeek_DepthChartTest()
        {
            //Assert
            // 1. Create a Sport NFL with positions QB, LWR
            var sportCommand = new AddSportsCommand { Name = "NFL", Positions = new List<string> { "QB", "LWR" } };
            int sportId = await _client.PostAsJsonAsync<AddSportsCommand, int>("Sport", sportCommand);

            // 2. Create a Teams for NFL
            var teamId1 = await _client.PostAsJsonAsync<AddTeamsCommand, int>("Team", new AddTeamsCommand { Name = "Buccaneers", SportId = sportId });
            var teamId2 = await _client.PostAsJsonAsync<AddTeamsCommand, int>("Team", new AddTeamsCommand { Name = "Buccaneers", SportId = sportId });

            // 3. Create a DepthChart for the Current week, so use null values for chartId
            var depthChartId1 = await _client.PostAsJsonAsync<AddDepthChartDto, int>("/NFL/CreateDepthChart", new AddDepthChartDto { TeamId = teamId1, WeekId = null });
            var depthChartId2 = await _client.PostAsJsonAsync<AddDepthChartDto, int>("/NFL/CreateDepthChart", new AddDepthChartDto { TeamId = teamId2, WeekId = null });

            // 4. Create players
            var players = new List<AddPlayersCommand>
            {
                new AddPlayersCommand { Number = 12, Name = "Tom Brady", TeamId = teamId1 },
                new AddPlayersCommand { Number = 11, Name = "Blaine Gabbert", TeamId = teamId2 },
                new AddPlayersCommand { Number = 2, Name = "Kyle Trask", TeamId = teamId1 },
                new AddPlayersCommand { Number = 13, Name = "Mike Evans", TeamId = teamId2 },
                new AddPlayersCommand { Number = 1, Name = "Jaelon Darden", TeamId = teamId1 },
                new AddPlayersCommand { Number = 10, Name = "Scott Miller", TeamId = teamId2 }
            };

            var playerIds = new Dictionary<int, int>();

            foreach (var player in players)
            {
                var playerAddResult = await _client.PostAsJsonAsync<AddPlayersCommand, int>("Player", player);
                playerIds.Add(player.Number, playerAddResult);
            }

            // 5. Add Players to DepthChart
            var addPlayerCommands = new List<AddPlayerToDepthChartDto>
            {
                new AddPlayerToDepthChartDto { Position = "QB", PlayerId = playerIds[12], Rank = 1, ChartId = depthChartId1 },
                new AddPlayerToDepthChartDto { Position = "QB", PlayerId = playerIds[11], Rank = 2, ChartId = depthChartId2 },
                new AddPlayerToDepthChartDto { Position = "QB", PlayerId = playerIds[2], Rank = 3, ChartId = depthChartId1 },
                new AddPlayerToDepthChartDto { Position = "LWR", PlayerId = playerIds[13], Rank = 1, ChartId = depthChartId2 },
                new AddPlayerToDepthChartDto { Position = "LWR", PlayerId = playerIds[1], Rank = 2, ChartId = depthChartId1 },
                new AddPlayerToDepthChartDto { Position = "LWR", PlayerId = playerIds[10], Rank = 3, ChartId = depthChartId2 }
            };

            foreach (var addPlayer in addPlayerCommands)
            {
                await _client.PostAsJsonAsync<AddPlayerToDepthChartDto>("/NFL/AddPlayerToDepthChart", addPlayer);
            }
            
            // 16. getFullDepthChart()
            Dictionary<string, List<PlayerDto>> depthChart = await _client.GetFromJsonAsync<Dictionary<string, List<PlayerDto>>>($"/NFL/GetFullDepthChart/?chartId={depthChartId1}");
            Assert.Equal(2, depthChart.Count);
            Assert.Equal(12, depthChart["QB"][0].Number);
            Assert.Equal("Tom Brady", depthChart["QB"][0].Name);
            Assert.Equal(2, depthChart["QB"][1].Number);
            Assert.Equal("Kyle Trask", depthChart["QB"][1].Name);
            Assert.Equal(1, depthChart["LWR"][0].Number);
            Assert.Equal("Jaelon Darden", depthChart["LWR"][0].Name);
            
            // 13. getFullDepthChart()
            depthChart = await _client.GetFromJsonAsync<Dictionary<string, List<PlayerDto>>>($"/NFL/GetFullDepthChart/?chartId={depthChartId2}");
            Assert.Equal(2, depthChart.Count);
            Assert.Equal(11, depthChart["QB"][0].Number);
            Assert.Equal("Blaine Gabbert", depthChart["QB"][0].Name);
            Assert.Equal(13, depthChart["LWR"][0].Number);
            Assert.Equal("Mike Evans", depthChart["LWR"][0].Name);
            Assert.Equal(10, depthChart["LWR"][1].Number);
            Assert.Equal("Scott Miller", depthChart["LWR"][1].Name);
        }
    }
}
