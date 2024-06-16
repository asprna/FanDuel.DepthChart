using FanDuel.DepthChart.Application.Contracts.Business;
using FanDuel.DepthChart.Domain.Dtos;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using System;
//using System.ComponentModel.DataAnnotations;

namespace FanDuel.DepthChart.MinApi.EndPoints
{
    public static class NFLEndPoints
    {
        public static void MapNFLEndpoints(this WebApplication app, IDepthChartServiceFactory depthChartFactory)
        {
            var depthChartService = depthChartFactory.CreateDepthChart("NFL");

            //app.MapPost("/NFL/CreateDepthChart", async (AddDepthChartDto depthChart, IValidator<AddDepthChartDto> validator) => 
            //{
            //    ValidationResult validationResult = await validator.ValidateAsync(depthChart);

            //    if (!validationResult.IsValid)
            //    {
            //        return Results.ValidationProblem(validationResult.ToDictionary());
            //    }

            //    return Results.Ok(await depthChartService.CreateDepthChart(depthChart.TeamId, depthChart.WeekId)); 
            //})
            //.WithName("CreateDepthChart")
            //.WithOpenApi();

            app.MapPost("/NFL/AddPlayerToDepthChart", async (AddPlayerToDepthChartDto player, IValidator<AddPlayerToDepthChartDto> validator) =>
            {
                ValidationResult validationResult = await validator.ValidateAsync(player);

                if (!validationResult.IsValid)
                {
                    return Results.ValidationProblem(validationResult.ToDictionary());
                }

                await depthChartService.AddPlayerToDepthChart(player.Position, player.PlayerId, player.Rank, player.ChartId);
                return Results.Ok();
            })
            .WithName("AddPlayerToDepthChart")
            .WithOpenApi();

            app.MapPost("/NFL/RemovePlayerFromDepthChart", async (RemovePlayerFromDepthChartDto player, IValidator<RemovePlayerFromDepthChartDto> validator) =>
            {
                ValidationResult validationResult = await validator.ValidateAsync(player);

                if (!validationResult.IsValid)
                {
                    return Results.ValidationProblem(validationResult.ToDictionary());
                }

                return Results.Ok(await depthChartService.RemovePlayerFromDepthChart(player.Position, player.PlayerId, player.ChartId));
            })
            .WithName("RemovePlayerFromDepthChart")
            .WithOpenApi();

            app.MapGet("/NFL/GetBackups", async (
                [FromQuery] string position,
                [FromQuery] int playerId,
                [FromQuery] int? chartId) =>
            {
                var errors = new Dictionary<string, string[]>();

                if (chartId.HasValue && chartId < 1)
                {
                    errors.Add(nameof(chartId), new[] { "ChartId must be either null or greater than 0." });
                }

                if (playerId < 1)
                {
                    errors.Add(nameof(playerId), new[] { "PlayerId must be greater than 0." });
                }

                if (errors.Count > 1)
                {
                    return Results.ValidationProblem(errors);
                }

                return Results.Ok(await depthChartService.GetBackups(position, playerId, chartId));
            })
            .WithName("GetBackups")
            .WithOpenApi();

            app.MapGet("/NFL/GetFullDepthChart", async ([FromQuery] int? chartId) =>
            {
                if (chartId.HasValue && chartId < 1)
                {
                    var errors = new Dictionary<string, string[]>
                    {
                        { nameof(chartId), new[] { "ChartId must be either null or greater than 0." } }
                    };
                    return Results.ValidationProblem(errors);
                }

                return Results.Ok(await depthChartService.GetFullDepthChart(chartId));
            })
            .WithName("GetFullDepthChart")
            .WithOpenApi();
        }
    }
}
