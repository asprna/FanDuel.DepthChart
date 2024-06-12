using AutoMapper;
using FanDuel.DepthChart.Application.Contracts.Persistence;
using FanDuel.DepthChart.Application.Exceptions;
using FanDuel.DepthChart.Application.Features.Sports.Commands;
using FanDuel.DepthChart.Application.Features.Teams.Commands;
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

namespace FanDuel.DepthChart.Test.Features.Teams.Commands
{
    public class AddTeamsCommandHandlerTest : InMemoryTestBase
    {
        private AddTeamsCommandHandler sut;
        private readonly Mock<ILogger<AddTeamsCommandHandler>> _logger = new Mock<ILogger<AddTeamsCommandHandler>>();
        private readonly Mock<IApplicationDbContext> _contexMoq = new Mock<IApplicationDbContext>();
        private readonly Mock<IMapper> _mapper = new Mock<IMapper>();

        [Fact]
        public async Task HandlerShouldReturnNewTeamIdIfSuccess()
        {
            //Arrange
            var request = new AddTeamsCommand
            {
                Name = "Eagles",
                SportId = 1
            };

            var team = new Team
            {
                Name = request.Name,
                SportId = request.SportId
            };

            _mapper.Setup(x => x.Map<Team>(request)).Returns(team);

            sut = new AddTeamsCommandHandler(_context, _mapper.Object, _logger.Object);

            //Act
            var result = await sut.Handle(request, CancellationToken.None);

            //Assert
            var newTeam = await _context.Teams.Where(x => x.Name == request.Name).FirstOrDefaultAsync();
            Assert.Equal(newTeam?.Id, result);
        }


        [Fact]
        public async Task HandlerShouldNoContentErrorIdIfSportNotFound()
        {
            //Arrange
            var request = new AddTeamsCommand
            {
                Name = "Eagles",
                SportId = 10
            };

            var team = new Team
            {
                Name = request.Name,
                SportId = request.SportId
            };

            _mapper.Setup(x => x.Map<Team>(request)).Returns(team);

            sut = new AddTeamsCommandHandler(_context, _mapper.Object, _logger.Object);

            //Act & Assert
            NoContentException result = await Assert.ThrowsAsync<NoContentException>(() => sut.Handle(request, CancellationToken.None));
            Assert.Equal("Sport does not exists", result.Message);
        }
    }
}
