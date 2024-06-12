using AutoMapper;
using FanDuel.DepthChart.Application.Contracts.Persistence;
using FanDuel.DepthChart.Application.Exceptions;
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

namespace FanDuel.DepthChart.Test.Features.Sports.Commands
{
    public class AddSportsCommandHandlerTest : InMemoryTestBase
    {
        private AddSportsCommandHandler sut;
        private readonly Mock<ILogger<AddSportsCommandHandler>> _logger = new Mock<ILogger<AddSportsCommandHandler>>();
        private readonly Mock<IApplicationDbContext> _contexMoq = new Mock<IApplicationDbContext>();
        private readonly Mock<IMapper> _mapper = new Mock<IMapper>();

        [Fact]
        public async Task HandlerShouldReturnNewSportIdIfSuccess()
        {
            //Arrange
            var request = new AddSportsCommand
            {
                Name = "AFL",
                Positions = new List<string> { "FB", "HB", "C", "HF", "FF" }
            };

            var sport = new Sport
            {
                Name = request.Name
            };

            sport.Positions.Add(new Position { Name = "FB" });
            sport.Positions.Add(new Position { Name = "HB" });
            sport.Positions.Add(new Position { Name = "C" });
            sport.Positions.Add(new Position { Name = "HF" });
            sport.Positions.Add(new Position { Name = "FF" });

            _mapper.Setup(x => x.Map<Sport>(request)).Returns(sport);

            sut = new AddSportsCommandHandler(_context, _mapper.Object, _logger.Object);

            //Act
            var result = await sut.Handle(request, CancellationToken.None);

            //Assert
            var newSport = await _context.Sports.Where(x => x.Name == request.Name).FirstOrDefaultAsync();
            Assert.Equal(newSport?.Id, result);
        }

        [Fact]
        public async Task HandlerShouldReturnDataContextExceptionIfUnsuccess()
        {
            //Arrange
            var request = new AddSportsCommand
            {
                Name = "AFL",
                Positions = new List<string> { "FB", "HB", "C", "HF", "FF" }
            };

            var sport = new Sport
            {
                Name = request.Name
            };

            sport.Positions.Add(new Position { Name = "FB" });
            sport.Positions.Add(new Position { Name = "HB" });
            sport.Positions.Add(new Position { Name = "C" });
            sport.Positions.Add(new Position { Name = "HF" });
            sport.Positions.Add(new Position { Name = "FF" });

            _mapper.Setup(x => x.Map<Sport>(request)).Returns(sport);
            _contexMoq.Setup(p => p.Sports.AddAsync(sport, default));
            _contexMoq.Setup(p => p.SaveChangesAsync(default)).ReturnsAsync(-1);

            sut = new AddSportsCommandHandler(_contexMoq.Object, _mapper.Object, _logger.Object);

            //Act & Assert
            DataContextException result = await Assert.ThrowsAsync<DataContextException>(() => sut.Handle(request, CancellationToken.None));
            Assert.Equal("Unable to save changes to DB", result.Message);
        }
    }
}
