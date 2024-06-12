using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FanDuel.DepthChart.Domain.Entities;

namespace FanDuel.DepthChart.Infrastructure.Persistence
{
    public class DepthChartContext : DbContext
    {
        public DepthChartContext(DbContextOptions<DepthChartContext> options) : base(options)
        {
        }

        public DbSet<Sport> Sports { get; set; }
        public DbSet<Position> Positions { get; set; }
    }
}
