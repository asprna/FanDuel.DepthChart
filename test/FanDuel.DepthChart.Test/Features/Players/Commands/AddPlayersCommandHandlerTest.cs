using AutoMapper;
using FanDuel.DepthChart.Application.Contracts.Persistence;
using FanDuel.DepthChart.Application.Exceptions;
using FanDuel.DepthChart.Application.Features.Players.Commands;
using FanDuel.DepthChart.Application.Features.Sports.Commands;
using FanDuel.DepthChart.Domain.Entities;
using FanDuel.DepthChart.Test.Helper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FanDuel.DepthChart.Test.Features.Players.Commands
{
    public class AddPlayersCommandHandlerTest : InMemoryTestBase
    {
        private AddPlayersCommandHandler sut;
        private readonly Mock<ILogger<AddPlayersCommandHandler>> _logger = new Mock<ILogger<AddPlayersCommandHandler>>();
        private readonly Mock<IApplicationDbContext> _contexMoq = new Mock<IApplicationDbContext>();
        private readonly Mock<IMapper> _mapper = new Mock<IMapper>();

        [Fact]
        public async Task HandlerShouldReturnNewPlayerIdIfSuccess()
        {
            //Arrange
            var request = new AddPlayersCommand
            {
                Name = "Tom Brady",
                Number = 12,
                TeamId = 1
            };

            var player = new Player
            {
                Name = request.Name,
                Number = request.Number,
                TeamId = request.TeamId
            };

            _mapper.Setup(x => x.Map<Player>(request)).Returns(player);

            if (!_context.Teams.Where(x => x.Id == request.TeamId).Any())
            {
                var team = new Team
                {
                    Name = "Tampa Bay Buccaneers",
                    SportId = 1
                };

                _context.Teams.Add(team);
                _context.SaveChanges();
            }

            sut = new AddPlayersCommandHandler(_context, _mapper.Object, _logger.Object);

            //Act
            var result = await sut.Handle(request, CancellationToken.None);

            //Assert
            var newPlayer = await _context.Players.Where(x => x.Name == request.Name).FirstOrDefaultAsync();
            Assert.Equal(newPlayer?.Id, result);
        }

        [Fact]
        public async Task HandlerShouldReturnNoContectIdIfTeamNotFound()
        {
            //Arrange
            var request = new AddPlayersCommand
            {
                Name = "Tom Brady",
                Number = 12,
                TeamId = 10
            };

            var player = new Player
            {
                Name = request.Name,
                Number = request.Number,
                TeamId = request.TeamId
            };

            _mapper.Setup(x => x.Map<Player>(request)).Returns(player);
            sut = new AddPlayersCommandHandler(_context, _mapper.Object, _logger.Object);

            //Act & Assert
            NoContentException result = await Assert.ThrowsAsync<NoContentException>(() => sut.Handle(request, CancellationToken.None));
            Assert.Equal("Team does not exists", result.Message);
        }

        [Fact]
        public async Task HandlerShouldReturnConflictIdIfNumberExists()
        {
            //Arrange
            var request = new AddPlayersCommand
            {
                Name = "Tom Brady",
                Number = 12,
                TeamId = 1
            };

            var player = new Player
            {
                Name = request.Name,
                Number = request.Number,
                TeamId = request.TeamId
            };

            _mapper.Setup(x => x.Map<Player>(request)).Returns(player);

            if (!_context.Teams.Where(x => x.Id == request.TeamId).Any())
            {
                var team = new Team
                {
                    Name = "Tampa Bay Buccaneers",
                    SportId = 1
                };

                _context.Teams.Add(team);
                _context.SaveChanges();
            }

            if (!_context.Players.Where(x => x.Number == request.Number).Any())
            {
                var playerExists = new Player
                {
                    Name = "Ash",
                    Number = 12,
                    TeamId = 1
                };

                _context.Players.Add(playerExists);
                _context.SaveChanges();
            }

            sut = new AddPlayersCommandHandler(_context, _mapper.Object, _logger.Object);

            //Act & Assert
            ConflictException result = await Assert.ThrowsAsync<ConflictException>(() => sut.Handle(request, CancellationToken.None));
            Assert.Equal("Player number conflits with existing player", result.Message);
        }
    }
}
