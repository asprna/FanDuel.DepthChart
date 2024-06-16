using FanDuel.DepthChart.Application.Features.Players.Commands;
using FanDuel.DepthChart.Application.Features.Sports.Commands;
using FanDuel.DepthChart.Application.Features.Teams.Commands;
using FanDuel.DepthChart.Domain.Dtos;
using FanDuel.DepthChart.Test.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace FanDuel.DepthChart.Test.IntegrationTest
{
    public class NFLIntegrationTest : IClassFixture<InMemoryApiTestBase>
    {
        private readonly HttpClient _client;
        public NFLIntegrationTest(InMemoryApiTestBase testBase) => _client = testBase.Client;

        [Fact]
        public async Task NFL_SingleTeam_DepthChartTest()
        {
            //Assert
            // 1. Create a Sport NFL with positions QB, LWR
            var sportCommand = new AddSportsCommand { Name = "NFL", Positions = new List<string> { "QB", "LWR" } };
            var sportContent = new StringContent(JsonSerializer.Serialize(sportCommand), Encoding.UTF8, "application/json");
            var sportResponse = await _client.PostAsync("Sport", sportContent);
            sportResponse.EnsureSuccessStatusCode();
            var sportId = JsonSerializer.Deserialize<int>(await sportResponse.Content.ReadAsStringAsync());

            // 2. Create a Team for NFL
            var teamCommand = new AddTeamsCommand { Name = "Buccaneers", SportId = sportId };
            var teamContent = new StringContent(JsonSerializer.Serialize(teamCommand), Encoding.UTF8, "application/json");
            var teamResponse = await _client.PostAsync("Team", teamContent);
            teamResponse.EnsureSuccessStatusCode();
            var teamId = JsonSerializer.Deserialize<int>(await teamResponse.Content.ReadAsStringAsync());

            // 3. Create a DepthChart for the Current week, so use null values for chartId
            var depthChartCommand = new AddDepthChartDto { TeamId = teamId, WeekId = null };
            var depthChartContent = new StringContent(JsonSerializer.Serialize(depthChartCommand), Encoding.UTF8, "application/json");
            var depthChartResponse = await _client.PostAsync("/NFL/CreateDepthChart", depthChartContent);
            depthChartResponse.EnsureSuccessStatusCode();
            var depthChartId = JsonSerializer.Deserialize<int>(await depthChartResponse.Content.ReadAsStringAsync());

            // 4. Create players
            var players = new List<AddPlayersCommand>
        {
            new AddPlayersCommand { Number = 12, Name = "Tom Brady" },
            new AddPlayersCommand { Number = 11, Name = "Blaine Gabbert" },
            new AddPlayersCommand { Number = 2, Name = "Kyle Trask" },
            new AddPlayersCommand { Number = 13, Name = "Mike Evans" },
            new AddPlayersCommand { Number = 1, Name = "Jaelon Darden" },
            new AddPlayersCommand { Number = 10, Name = "Scott Miller" }
        };

            var playerIds = new List<int>();

            foreach (var player in players)
            {
                var playerContent = new StringContent(JsonSerializer.Serialize(player), Encoding.UTF8, "application/json");
                var playerResponse = await _client.PostAsync("Player", playerContent);
                playerResponse.EnsureSuccessStatusCode();
                playerIds.Add(JsonSerializer.Deserialize<int>(await playerResponse.Content.ReadAsStringAsync()));
            }

            // 5. Add Players to DepthChart
            var addPlayerCommands = new List<AddPlayerToDepthChartDto>
            {
                new AddPlayerToDepthChartDto { Position = "QB", PlayerId = playerIds[0], Rank = 0, ChartId = depthChartId },
                new AddPlayerToDepthChartDto { Position = "QB", PlayerId = playerIds[1], Rank = 1, ChartId = depthChartId },
                new AddPlayerToDepthChartDto { Position = "QB", PlayerId = playerIds[2], Rank = 2, ChartId = depthChartId },
                new AddPlayerToDepthChartDto { Position = "LWR", PlayerId = playerIds[3], Rank = 0, ChartId = depthChartId },
                new AddPlayerToDepthChartDto { Position = "LWR", PlayerId = playerIds[4], Rank = 1, ChartId = depthChartId },
                new AddPlayerToDepthChartDto { Position = "LWR", PlayerId = playerIds[5], Rank = 2, ChartId = depthChartId }
            };

            foreach (var addPlayer in addPlayerCommands)
            {
                var addPlayerContent = new StringContent(JsonSerializer.Serialize(addPlayer), Encoding.UTF8, "application/json");
                var addPlayerResponse = await _client.PostAsync("/NFL/AddPlayerToDepthChart", addPlayerContent);
                addPlayerResponse.EnsureSuccessStatusCode();
            }

            // 6. Call getBackups("QB", TomBrady)
            var getBackupsResponse = await _client.GetAsync($"/NFL/GetBackups?position=QB&playerId={playerIds[0]}");
            getBackupsResponse.EnsureSuccessStatusCode();
            var backups = JsonSerializer.Deserialize<List<PlayerDto>>(await getBackupsResponse.Content.ReadAsStringAsync());
            Assert.Equal(2, backups.Count);
            Assert.Equal(11, backups[0].Number);
            Assert.Equal("Blaine Gabbert", backups[0].Name);
            Assert.Equal(2, backups[1].Number);
            Assert.Equal("Kyle Trask", backups[1].Name);

            // 7. call getBackups(“QB”, MikeEvans)
            getBackupsResponse = await _client.GetAsync($"/NFL/GetBackups?position=QB&playerId={playerIds[4]}");
            getBackupsResponse.EnsureSuccessStatusCode();
            backups = JsonSerializer.Deserialize<List<PlayerDto>>(await getBackupsResponse.Content.ReadAsStringAsync());
            Assert.Equal(2, backups.Count);
            Assert.Equal(11, backups[0].Number);
            Assert.Equal("Blaine Gabbert", backups[0].Name);
            Assert.Equal(2, backups[1].Number);
            Assert.Equal("Kyle Trask", backups[1].Name);

            // 8. call getBackups(“QB”, BlaineGabbert)
            getBackupsResponse = await _client.GetAsync($"/NFL/GetBackups?position=QB&playerId={playerIds[4]}");
            getBackupsResponse.EnsureSuccessStatusCode();
            backups = JsonSerializer.Deserialize<List<PlayerDto>>(await getBackupsResponse.Content.ReadAsStringAsync());
            Assert.Equal(2, backups.Count);
            Assert.Equal(11, backups[0].Number);
            Assert.Equal("Blaine Gabbert", backups[0].Name);
            Assert.Equal(2, backups[1].Number);
            Assert.Equal("Kyle Trask", backups[1].Name);
        }
    }
}
