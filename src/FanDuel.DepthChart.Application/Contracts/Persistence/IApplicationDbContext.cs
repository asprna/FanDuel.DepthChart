﻿using FanDuel.DepthChart.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FanDuel.DepthChart.Application.Contracts.Persistence
{
    public interface IApplicationDbContext
    {
        public DbSet<Sport> Sports { get; set; }
        public DbSet<Position> Positions { get; set; }
        public DbSet<Player> Players { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<TeamDepthChart> TeamDepthCharts { get; set; }
        public DbSet<PlayerChartIndex> PlayerChartIndexes { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
