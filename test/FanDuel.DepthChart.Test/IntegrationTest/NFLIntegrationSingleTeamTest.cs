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
    public class NFLIntegrationSingleTeamTest : IClassFixture<InMemoryApiTestBase>
    {
        private readonly HttpClient _client;
        public NFLIntegrationSingleTeamTest(InMemoryApiTestBase testBase) => _client = testBase.Client;

        [Fact]
        public async Task NFL_SingleTeam_GivenWeek_DepthChartTest()
        {
            //Assert
            var weekNumber = 10;

            // 1. Create a Sport NFL with positions FB, RWR
            var sportCommand = new AddSportsCommand { Name = "NFL", Positions = new List<string> { "FB", "RWR" } };
            int sportId = await _client.PostAsJsonAsync<AddSportsCommand, int>("Sport", sportCommand);

            // 2. Create a Team for NFL
            var teamCommand = new AddTeamsCommand { Name = "Eagles", SportId = sportId };
            var teamId = await _client.PostAsJsonAsync<AddTeamsCommand, int>("Team", teamCommand);

            // 3. Create a DepthChart for the Current week, so use null values for chartId
            var depthChartCommand = new AddDepthChartDto { TeamId = teamId, WeekId = weekNumber };
            var depthChartId = await _client.PostAsJsonAsync<AddDepthChartDto, int>("/NFL/CreateDepthChart", depthChartCommand);

            // 4. Create players
            var players = new List<AddPlayersCommand>
            {
                new AddPlayersCommand { Number = 20, Name = "Tom Brady20", TeamId = teamId },
                new AddPlayersCommand { Number = 21, Name = "Blaine Gabbert21", TeamId = teamId },
                new AddPlayersCommand { Number = 22, Name = "Kyle Trask22", TeamId = teamId },
                new AddPlayersCommand { Number = 23, Name = "Mike Evans23", TeamId = teamId },
                new AddPlayersCommand { Number = 24, Name = "Jaelon Darden24", TeamId = teamId },
                new AddPlayersCommand { Number = 25, Name = "Scott Miller25", TeamId = teamId }
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
                new AddPlayerToDepthChartDto { Position = "FB", PlayerId = playerIds[20], Rank = 1, ChartId = depthChartId },
                new AddPlayerToDepthChartDto { Position = "FB", PlayerId = playerIds[21], Rank = 2, ChartId = depthChartId },
                new AddPlayerToDepthChartDto { Position = "FB", PlayerId = playerIds[22], Rank = 3, ChartId = depthChartId },
                new AddPlayerToDepthChartDto { Position = "RWR", PlayerId = playerIds[23], Rank = 1, ChartId = depthChartId },
                new AddPlayerToDepthChartDto { Position = "RWR", PlayerId = playerIds[24], Rank = 2, ChartId = depthChartId },
                new AddPlayerToDepthChartDto { Position = "RWR", PlayerId = playerIds[25], Rank = 3, ChartId = depthChartId }
            };

            foreach (var addPlayer in addPlayerCommands)
            {
                await _client.PostAsJsonAsync<AddPlayerToDepthChartDto>("/NFL/AddPlayerToDepthChart", addPlayer);
            }

            // 11. getFullDepthChart()
            Dictionary<string, List<PlayerDto>> depthChart = await _client.GetFromJsonAsync<Dictionary<string, List<PlayerDto>>>($"/NFL/GetFullDepthChart/?chartId={depthChartId}");
            Assert.Equal(2, depthChart.Count);
            Assert.Equal(20, depthChart["FB"][0].Number);
            Assert.Equal("Tom Brady20", depthChart["FB"][0].Name);
            Assert.Equal(21, depthChart["FB"][1].Number);
            Assert.Equal("Blaine Gabbert21", depthChart["FB"][1].Name);
            Assert.Equal(22, depthChart["FB"][2].Number);
            Assert.Equal("Kyle Trask22", depthChart["FB"][2].Name);
            Assert.Equal(23, depthChart["RWR"][0].Number);
            Assert.Equal("Mike Evans23", depthChart["RWR"][0].Name);
            Assert.Equal(24, depthChart["RWR"][1].Number);
            Assert.Equal("Jaelon Darden24", depthChart["RWR"][1].Name);
            Assert.Equal(25, depthChart["RWR"][2].Number);
            Assert.Equal("Scott Miller25", depthChart["RWR"][2].Name);
        }

        [Fact]
        public async Task NFL_SingleTeam_MultipleDepthChartByWeekTest()
        {
            //Assert
            int[] weekNumbers = [10, 11, 12];
            List<int> chartsIs = new List<int>();

            // 1. Create a Sport NFL with positions FB, RWR
            var sportCommand = new AddSportsCommand { Name = "NFL", Positions = new List<string> { "FB", "RWR" } };
            int sportId = await _client.PostAsJsonAsync<AddSportsCommand, int>("Sport", sportCommand);

            // 2. Create a Team for NFL
            var teamCommand = new AddTeamsCommand { Name = "Packers", SportId = sportId };
            var teamId = await _client.PostAsJsonAsync<AddTeamsCommand, int>("Team", teamCommand);

            // 3. Create a DepthChart for the Current week, so use null values for chartId
            foreach(var week in weekNumbers)
            {
                var id = await _client.PostAsJsonAsync<AddDepthChartDto, int>("/NFL/CreateDepthChart", new AddDepthChartDto { TeamId = teamId, WeekId = week });
                chartsIs.Add(id);
            }
           
            // 4. Create players
            var players = new List<AddPlayersCommand>
            {
                new AddPlayersCommand { Number = 121, Name = "Tom Brady121", TeamId = teamId },
                new AddPlayersCommand { Number = 111, Name = "Blaine Gabbert111", TeamId = teamId },
                new AddPlayersCommand { Number = 21, Name = "Kyle Trask21", TeamId = teamId },
                new AddPlayersCommand { Number = 131, Name = "Mike Evans131", TeamId = teamId },
                new AddPlayersCommand { Number = 11, Name = "Jaelon Darden11", TeamId = teamId },
                new AddPlayersCommand { Number = 101, Name = "Scott Miller101", TeamId = teamId }
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
                new AddPlayerToDepthChartDto { Position = "FB", PlayerId = playerIds[121], Rank = 1, ChartId = chartsIs[0] },
                new AddPlayerToDepthChartDto { Position = "FB", PlayerId = playerIds[111], Rank = 2, ChartId = chartsIs[1] },
                new AddPlayerToDepthChartDto { Position = "FB", PlayerId = playerIds[21], Rank = 3, ChartId = chartsIs[2] },
                new AddPlayerToDepthChartDto { Position = "RWR", PlayerId = playerIds[131], Rank = 1, ChartId = chartsIs[0] },
                new AddPlayerToDepthChartDto { Position = "RWR", PlayerId = playerIds[11], Rank = 2, ChartId = chartsIs[1] },
                new AddPlayerToDepthChartDto { Position = "RWR", PlayerId = playerIds[101], Rank = 3, ChartId = chartsIs[2] }
            };

            foreach (var addPlayer in addPlayerCommands)
            {
                await _client.PostAsJsonAsync<AddPlayerToDepthChartDto>("/NFL/AddPlayerToDepthChart", addPlayer);
            }

            // 6. getFullDepthChart()
            Dictionary<string, List<PlayerDto>> depthChart = await _client.GetFromJsonAsync<Dictionary<string, List<PlayerDto>>>($"/NFL/GetFullDepthChart/?chartId={chartsIs[0]}");
            Assert.Equal(2, depthChart.Count);
            Assert.Equal(121, depthChart["FB"][0].Number);
            Assert.Equal("Tom Brady121", depthChart["FB"][0].Name);
            Assert.Equal(131, depthChart["RWR"][0].Number);
            Assert.Equal("Mike Evans131", depthChart["RWR"][0].Name);

            depthChart = await _client.GetFromJsonAsync<Dictionary<string, List<PlayerDto>>>($"/NFL/GetFullDepthChart/?chartId={chartsIs[1]}");
            Assert.Equal(111, depthChart["FB"][0].Number);
            Assert.Equal("Blaine Gabbert111", depthChart["FB"][0].Name);
            Assert.Equal(11, depthChart["RWR"][0].Number);
            Assert.Equal("Jaelon Darden11", depthChart["RWR"][0].Name);

            depthChart = await _client.GetFromJsonAsync<Dictionary<string, List<PlayerDto>>>($"/NFL/GetFullDepthChart/?chartId={chartsIs[2]}");
            Assert.Equal(21, depthChart["FB"][0].Number);
            Assert.Equal("Kyle Trask21", depthChart["FB"][0].Name);
            Assert.Equal(101, depthChart["RWR"][0].Number);
            Assert.Equal("Scott Miller101", depthChart["RWR"][0].Name);
        }
    }
}
