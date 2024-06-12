using FanDuel.DepthChart.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FanDuel.DepthChart.Test.Helper
{
    public class InMemoryTestBase : IDisposable
    {
        protected readonly DepthChartContext _context;

        public InMemoryTestBase()
        {
            var options = new DbContextOptionsBuilder<DepthChartContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new DepthChartContext(options);

            _context.Database.EnsureCreated();

            DepthChartInitializer.Initializer(_context);
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();

            _context.Dispose();
        }
    }
}
