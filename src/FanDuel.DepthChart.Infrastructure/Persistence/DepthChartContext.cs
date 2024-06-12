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
        public const string SchemaName = "DepthChart";
        public const string MigrationTable = "__EFMigrationsHistory";

        public DepthChartContext(DbContextOptions<DepthChartContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.HasDefaultSchema(SchemaName);

            modelBuilder.Entity<Sport>()
                .HasMany(e => e.Positions)
                .WithOne(e => e.Sport)
                .HasForeignKey(e => e.SportId)
                .IsRequired();

            modelBuilder.Entity<Sport>()
                .Property(e => e.Name)
                .IsRequired();

            modelBuilder.Entity<Position>()
                .Property(e => e.Name)
                .IsRequired();

            modelBuilder.Entity<Team>()
                .HasMany(e => e.Players)
                .WithOne(e => e.Team)
                .HasForeignKey(e => e.TeamId)
                .IsRequired();

            modelBuilder.Entity<Team>()
                .HasMany(e => e.TeamDepthCharts)
                .WithOne(e => e.Team)
                .HasForeignKey(e => e.TeamId);

            modelBuilder.Entity<Team>()
                .Property(e => e.Name)
                .IsRequired();

            modelBuilder.Entity<Player>()
                .Property(e => e.Name)
                .IsRequired();

            modelBuilder.Entity<TeamDepthChart>()
                .HasMany(e => e.PlayerChartIndexs)
                .WithOne(e => e.TeamDepthChart)
                .HasForeignKey(e => e.TeamDepthChartId);

            modelBuilder.Entity<Player>()
                .HasMany(e => e.PlayerChartIndexs)
                .WithOne(e => e.Player)
                .HasForeignKey(e => e.PayerId);

            modelBuilder.Entity<Position>()
                .HasMany(e => e.PlayerChartIndexs)
                .WithOne(e => e.Position)
                .HasForeignKey(e => e.PositionId);
        }

        public DbSet<Sport> Sports { get; set; }
        public DbSet<Position> Positions { get; set; }
        public DbSet<Player> Players { get; set; }
        public DbSet<Team> Teams { get; set; }
    }
}
