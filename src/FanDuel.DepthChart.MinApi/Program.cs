using FanDuel.DepthChart.Application.Features.Players.Commands;
using MediatR;
using FanDuel.DepthChart.Application;
using FanDuel.DepthChart.Application.Contracts.Persistence;
using FanDuel.DepthChart.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using FanDuel.DepthChart.Application.Features.Sports.Commands;
using FanDuel.DepthChart.Application.Features.Teams.Commands;
using FanDuel.DepthChart.Application.Contracts.Business;
using FanDuel.DepthChart.Application.Services.DepthCharts;
using FanDuel.DepthChart.MinApi.Handler;
using FanDuel.DepthChart.Domain.Dtos;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Register DataContext
builder.Services.AddDbContext<DepthChartContext>(opt =>
{
    opt.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddScoped<IApplicationDbContext>(provider => provider.GetService<DepthChartContext>());
builder.Services.AddTransient<IDepthChartServiceFactory, DepthChartServiceFactory>();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

builder.Services.AddApplication();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseExceptionHandler();

app.MapPost("Player", async (AddPlayersCommand player, ISender _mediator) =>
{
    return Results.Ok(await _mediator.Send(player));
})
.WithName("AddPlayer")
.WithOpenApi();

app.MapPost("Sport", async (AddSportsCommand sport, ISender _mediator) =>
{
    return Results.Ok(await _mediator.Send(sport));
})
.WithName("AddSport")
.WithOpenApi();

app.MapPost("Team", async (AddTeamsCommand team, ISender _mediator) =>
{
    return Results.Ok(await _mediator.Send(team));
})
.WithName("AddTeam")
.WithOpenApi();

app.MapPost("/NFL/CreateDepthChart", async (AddDepthChartDto depthChart, IValidator<AddDepthChartDto> validator, IDepthChartServiceFactory depthChartFactory) =>
{
    var depthChartService = depthChartFactory.CreateDepthChart("NFL");

    ValidationResult validationResult = await validator.ValidateAsync(depthChart);

    if (!validationResult.IsValid)
    {
        return Results.ValidationProblem(validationResult.ToDictionary());
    }

    return Results.Ok(await depthChartService.CreateDepthChart(depthChart.TeamId, depthChart.WeekId));
})
.WithName("CreateDepthChart")
.WithOpenApi();

app.MapPost("/NFL/AddPlayerToDepthChart", async (AddPlayerToDepthChartDto player, IValidator<AddPlayerToDepthChartDto> validator, IDepthChartServiceFactory depthChartFactory) =>
{
    var depthChartService = depthChartFactory.CreateDepthChart("NFL");

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

app.MapPost("/NFL/RemovePlayerFromDepthChart", async (RemovePlayerFromDepthChartDto player, IValidator<RemovePlayerFromDepthChartDto> validator, IDepthChartServiceFactory depthChartFactory) =>
{
    var depthChartService = depthChartFactory.CreateDepthChart("NFL");

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
    [FromQuery] int? chartId,
    IDepthChartServiceFactory depthChartFactory) =>
{
    var depthChartService = depthChartFactory.CreateDepthChart("NFL");

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

app.MapGet("/NFL/GetFullDepthChart", async ([FromQuery] int? chartId, IDepthChartServiceFactory depthChartFactory) =>
{
    var depthChartService = depthChartFactory.CreateDepthChart("NFL");

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

app.Run();

public partial class Program { }