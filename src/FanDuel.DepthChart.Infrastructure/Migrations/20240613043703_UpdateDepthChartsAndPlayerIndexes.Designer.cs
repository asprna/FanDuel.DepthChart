﻿// <auto-generated />
using FanDuel.DepthChart.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace FanDuel.DepthChart.Infrastructure.Migrations
{
    [DbContext(typeof(DepthChartContext))]
    [Migration("20240613043703_UpdateDepthChartsAndPlayerIndexes")]
    partial class UpdateDepthChartsAndPlayerIndexes
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.6");

            modelBuilder.Entity("FanDuel.DepthChart.Domain.Entities.Player", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("Number")
                        .HasColumnType("INTEGER");

                    b.Property<int>("TeamId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("TeamId");

                    b.ToTable("Players");
                });

            modelBuilder.Entity("FanDuel.DepthChart.Domain.Entities.PlayerChartIndex", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("PayerId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("PositionId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Rank")
                        .HasColumnType("INTEGER");

                    b.Property<int>("TeamDepthChartId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("PayerId");

                    b.HasIndex("PositionId");

                    b.HasIndex("TeamDepthChartId");

                    b.ToTable("PlayerChartIndexs");
                });

            modelBuilder.Entity("FanDuel.DepthChart.Domain.Entities.Position", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(3)
                        .HasColumnType("TEXT");

                    b.Property<int>("SportId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("SportId");

                    b.ToTable("Positions");
                });

            modelBuilder.Entity("FanDuel.DepthChart.Domain.Entities.Sport", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Sports");
                });

            modelBuilder.Entity("FanDuel.DepthChart.Domain.Entities.Team", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("SportId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("SportId");

                    b.ToTable("Teams");
                });

            modelBuilder.Entity("FanDuel.DepthChart.Domain.Entities.TeamDepthChart", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("TeamId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("WeekId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("TeamId");

                    b.ToTable("TeamDepthCharts");
                });

            modelBuilder.Entity("FanDuel.DepthChart.Domain.Entities.Player", b =>
                {
                    b.HasOne("FanDuel.DepthChart.Domain.Entities.Team", "Team")
                        .WithMany("Players")
                        .HasForeignKey("TeamId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Team");
                });

            modelBuilder.Entity("FanDuel.DepthChart.Domain.Entities.PlayerChartIndex", b =>
                {
                    b.HasOne("FanDuel.DepthChart.Domain.Entities.Player", "Player")
                        .WithMany("PlayerChartIndexs")
                        .HasForeignKey("PayerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("FanDuel.DepthChart.Domain.Entities.Position", "Position")
                        .WithMany("PlayerChartIndexs")
                        .HasForeignKey("PositionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("FanDuel.DepthChart.Domain.Entities.TeamDepthChart", "TeamDepthChart")
                        .WithMany("PlayerChartIndexs")
                        .HasForeignKey("TeamDepthChartId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Player");

                    b.Navigation("Position");

                    b.Navigation("TeamDepthChart");
                });

            modelBuilder.Entity("FanDuel.DepthChart.Domain.Entities.Position", b =>
                {
                    b.HasOne("FanDuel.DepthChart.Domain.Entities.Sport", "Sport")
                        .WithMany("Positions")
                        .HasForeignKey("SportId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Sport");
                });

            modelBuilder.Entity("FanDuel.DepthChart.Domain.Entities.Team", b =>
                {
                    b.HasOne("FanDuel.DepthChart.Domain.Entities.Sport", "Sport")
                        .WithMany("Teams")
                        .HasForeignKey("SportId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Sport");
                });

            modelBuilder.Entity("FanDuel.DepthChart.Domain.Entities.TeamDepthChart", b =>
                {
                    b.HasOne("FanDuel.DepthChart.Domain.Entities.Team", "Team")
                        .WithMany("TeamDepthCharts")
                        .HasForeignKey("TeamId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Team");
                });

            modelBuilder.Entity("FanDuel.DepthChart.Domain.Entities.Player", b =>
                {
                    b.Navigation("PlayerChartIndexs");
                });

            modelBuilder.Entity("FanDuel.DepthChart.Domain.Entities.Position", b =>
                {
                    b.Navigation("PlayerChartIndexs");
                });

            modelBuilder.Entity("FanDuel.DepthChart.Domain.Entities.Sport", b =>
                {
                    b.Navigation("Positions");

                    b.Navigation("Teams");
                });

            modelBuilder.Entity("FanDuel.DepthChart.Domain.Entities.Team", b =>
                {
                    b.Navigation("Players");

                    b.Navigation("TeamDepthCharts");
                });

            modelBuilder.Entity("FanDuel.DepthChart.Domain.Entities.TeamDepthChart", b =>
                {
                    b.Navigation("PlayerChartIndexs");
                });
#pragma warning restore 612, 618
        }
    }
}
