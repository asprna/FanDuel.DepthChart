using FanDuel.DepthChart.Application.Features.Players.Commands;
using FanDuel.DepthChart.Application.Features.Sports.Commands;
using FanDuel.DepthChart.Application.Features.Teams.Commands;
using FanDuel.DepthChart.Domain.Dtos;
using FanDuel.DepthChart.Test.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FanDuel.DepthChart.Test.IntegrationTest
{
    public class NFLIntegrationAddPlayerTest : IClassFixture<InMemoryApiTestBase>
    {
        private readonly HttpClient _client;
        public NFLIntegrationAddPlayerTest(InMemoryApiTestBase testBase) => _client = testBase.Client;

        //[Fact]
        //public async Task NFL_SingleTeam_CurrentWeek_DepthChartTest()
        //{
        //    //Assert
        //    // 1. Create a Sport NFL with positions QB, LWR
        //    var sportCommand = new AddSportsCommand { Name = "NFL", Positions = new List<string> { "QB", "LWR" } };
        //    int sportId = await _client.PostAsJsonAsync<AddSportsCommand, int>("Sport", sportCommand);

        //    // 2. Create a Team for NFL
        //    var teamCommand = new AddTeamsCommand { Name = "Buccaneers", SportId = sportId };
        //    var teamId = await _client.PostAsJsonAsync<AddTeamsCommand, int>("Team", teamCommand);

        //    // 3. Create a DepthChart for the Current week, so use null values for chartId
        //    var depthChartCommand = new AddDepthChartDto { TeamId = teamId, WeekId = null };
        //    var depthChartId = await _client.PostAsJsonAsync<AddDepthChartDto, int>("/NFL/CreateDepthChart", depthChartCommand);

        //    // 4. Create players
        //    var players = new List<AddPlayersCommand>
        //    {
        //        new AddPlayersCommand { Number = 12, Name = "Tom Brady", TeamId = teamId },
        //        new AddPlayersCommand { Number = 11, Name = "Blaine Gabbert", TeamId = teamId },
        //        new AddPlayersCommand { Number = 2, Name = "Kyle Trask", TeamId = teamId },
        //        new AddPlayersCommand { Number = 13, Name = "Mike Evans", TeamId = teamId },
        //        new AddPlayersCommand { Number = 1, Name = "Jaelon Darden", TeamId = teamId },
        //        new AddPlayersCommand { Number = 10, Name = "Scott Miller", TeamId = teamId }
        //    };

        //    var playerIds = new Dictionary<int, int>();

        //    foreach (var player in players)
        //    {
        //        var playerAddResult = await _client.PostAsJsonAsync<AddPlayersCommand, int>("Player", player);
        //        playerIds.Add(player.Number, playerAddResult);
        //    }

        //    // 5. Add Players to DepthChart
        //    var addPlayerCommands = new List<AddPlayerToDepthChartDto>
        //    {
        //        new AddPlayerToDepthChartDto { Position = "QB", PlayerId = playerIds[12], Rank = 1, ChartId = depthChartId },
        //        new AddPlayerToDepthChartDto { Position = "QB", PlayerId = playerIds[11], Rank = 2, ChartId = depthChartId },
        //        new AddPlayerToDepthChartDto { Position = "QB", PlayerId = playerIds[2], Rank = 3, ChartId = depthChartId },
        //        new AddPlayerToDepthChartDto { Position = "LWR", PlayerId = playerIds[13], Rank = 1, ChartId = depthChartId },
        //        new AddPlayerToDepthChartDto { Position = "LWR", PlayerId = playerIds[1], Rank = 2, ChartId = depthChartId },
        //        new AddPlayerToDepthChartDto { Position = "LWR", PlayerId = playerIds[10], Rank = 3, ChartId = depthChartId }
        //    };

        //    foreach (var addPlayer in addPlayerCommands)
        //    {
        //        await _client.PostAsJsonAsync<AddPlayerToDepthChartDto>("/NFL/AddPlayerToDepthChart", addPlayer);
        //    }

        //    // 6. Call getBackups("QB", TomBrady)
        //    var backups = await _client.GetFromJsonAsync<List<PlayerDto>>($"/NFL/GetBackups?position=QB&playerId={playerIds[12]}");
        //    Assert.Equal(2, backups.Count);
        //    Assert.Equal(11, backups[0].Number);
        //    Assert.Equal("Blaine Gabbert", backups[0].Name);
        //    Assert.Equal(2, backups[1].Number);
        //    Assert.Equal("Kyle Trask", backups[1].Name);

        //    // 7. Call getBackups("LWR", TomBrady)
        //    backups = await _client.GetFromJsonAsync<List<PlayerDto>>($"/NFL/GetBackups?position=LWR&playerId={playerIds[1]}");
        //    Assert.Single(backups);
        //    Assert.Equal(10, backups[0].Number);
        //    Assert.Equal("Scott Miller", backups[0].Name);

        //    // 8. call getBackups(“QB”, MikeEvans)
        //    backups = await _client.GetFromJsonAsync<List<PlayerDto>>($"/NFL/GetBackups?position=QB&playerId={playerIds[13]}");
        //    Assert.Empty(backups);

        //    // 9. call getBackups(“QB”, BlaineGabbert)
        //    backups = await _client.GetFromJsonAsync<List<PlayerDto>>($"/NFL/GetBackups?position=QB&playerId={playerIds[11]}");
        //    Assert.Single(backups);
        //    Assert.Equal(2, backups[0].Number);
        //    Assert.Equal("Kyle Trask", backups[0].Name);

        //    // 10. call getBackups(“QB”, Kyle Trask)
        //    backups = await _client.GetFromJsonAsync<List<PlayerDto>>($"/NFL/GetBackups?position=QB&playerId={playerIds[2]}");
        //    Assert.Empty(backups);

        //    // 11. getFullDepthChart()
        //    Dictionary<string, List<PlayerDto>> depthChart = await _client.GetFromJsonAsync<Dictionary<string, List<PlayerDto>>>($"/NFL/GetFullDepthChart");
        //    Assert.Equal(2, depthChart.Count);
        //    Assert.Equal(12, depthChart["QB"][0].Number);
        //    Assert.Equal("Tom Brady", depthChart["QB"][0].Name);
        //    Assert.Equal(11, depthChart["QB"][1].Number);
        //    Assert.Equal("Blaine Gabbert", depthChart["QB"][1].Name);
        //    Assert.Equal(2, depthChart["QB"][2].Number);
        //    Assert.Equal("Kyle Trask", depthChart["QB"][2].Name);
        //    Assert.Equal(13, depthChart["LWR"][0].Number);
        //    Assert.Equal("Mike Evans", depthChart["LWR"][0].Name);
        //    Assert.Equal(1, depthChart["LWR"][1].Number);
        //    Assert.Equal("Jaelon Darden", depthChart["LWR"][1].Name);
        //    Assert.Equal(10, depthChart["LWR"][2].Number);
        //    Assert.Equal("Scott Miller", depthChart["LWR"][2].Name);

        //    // 12. removePlayerFromDepthChart(“LWR”, MikeEvans)
        //    var removePlayer = new RemovePlayerFromDepthChartDto { PlayerId = playerIds[13], Position = "LWR" };
        //    var depthChartRemovePlayer = await _client.PostAsJsonAsync<RemovePlayerFromDepthChartDto, PlayerDto>("/NFL/RemovePlayerFromDepthChart", removePlayer);
        //    Assert.NotNull(depthChartRemovePlayer);
        //    Assert.Equal(13, depthChartRemovePlayer.Number);
        //    Assert.Equal("Mike Evans", depthChartRemovePlayer.Name);

        //    // 13. getFullDepthChart()
        //    depthChart = await _client.GetFromJsonAsync<Dictionary<string, List<PlayerDto>>>($"/NFL/GetFullDepthChart");
        //    Assert.Equal(2, depthChart.Count);
        //    Assert.Equal(12, depthChart["QB"][0].Number);
        //    Assert.Equal("Tom Brady", depthChart["QB"][0].Name);
        //    Assert.Equal(11, depthChart["QB"][1].Number);
        //    Assert.Equal("Blaine Gabbert", depthChart["QB"][1].Name);
        //    Assert.Equal(2, depthChart["QB"][2].Number);
        //    Assert.Equal("Kyle Trask", depthChart["QB"][2].Name);
        //    Assert.Equal(1, depthChart["LWR"][0].Number);
        //    Assert.Equal("Jaelon Darden", depthChart["LWR"][0].Name);
        //    Assert.Equal(10, depthChart["LWR"][1].Number);
        //    Assert.Equal("Scott Miller", depthChart["LWR"][1].Name);
        //}
    }
}
