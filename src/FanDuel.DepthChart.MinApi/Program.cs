using FanDuel.DepthChart.Application.Features.Players.Commands;
using FanDuel.DepthChart.Domain.Entities;
using MediatR;
using FanDuel.DepthChart.Application;
using FanDuel.DepthChart.Application.Contracts.Persistence;
using FanDuel.DepthChart.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using FanDuel.DepthChart.Application.Features.Sports.Commands;
using FanDuel.DepthChart.Application.Features.Teams.Commands;
using FanDuel.DepthChart.MinApi.EndPoints;
using FanDuel.DepthChart.Application.Contracts.Business;
using FanDuel.DepthChart.Application.Services.DepthCharts;
using FanDuel.DepthChart.Application.Exceptions;
//using HellangNamespace = Hellang.Middleware.ProblemDetails;
//using Hellang.Middleware.ProblemDetails;
using FanDuel.DepthChart.MinApi.Extensions;
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
//builder.Services.AddProblemDetails(option => ConfigureProblemDetails(option));

builder.Services.AddApplication();

var app = builder.Build();

//Global Exception Handling Middleware
//app.UseProblemDetails();

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
    var result = await _mediator.Send(player);
    return Results.Ok();
})
.WithName("AddPlayer")
.WithOpenApi();

app.MapPost("Sport", async (AddSportsCommand sport, ISender _mediator) =>
{
    var result = await _mediator.Send(sport);
    return Results.Ok();
})
.WithName("AddSport")
.WithOpenApi();

app.MapPost("Team", async (AddTeamsCommand team, ISender _mediator) =>
{
    var result = await _mediator.Send(team);
    return Results.Ok();
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

//app.MapNFLEndpoints(app.Services.GetRequiredService<IDepthChartServiceFactory>());
app.Run();

//static void ConfigureProblemDetails(Hellang.Middleware.ProblemDetails.ProblemDetailsOptions options)
//{
//    // Custom mapping function for FluentValidation's ValidationException.
//    options.MapFluentValidationException();

//    options.IncludeExceptionDetails = (ctx, ex) =>
//    {
//        // Fetch services from HttpContext.RequestServices
//        var env = ctx.RequestServices.GetRequiredService<IHostEnvironment>();
//        return env.IsDevelopment(); //&& !(ex is ValidationException);
//    };

//    options.ShouldLogUnhandledException = (context, ex, problem) => false;
//    options.GetTraceId = ctx => null;

//    options.MapToStatusCode<NotImplementedException>(StatusCodes.Status501NotImplemented);
//    options.MapToStatusCode<HttpRequestException>(StatusCodes.Status503ServiceUnavailable);

//    options.Map<AppException>(ex => new Microsoft.AspNetCore.Mvc.ProblemDetails
//    {
//        Title = ex.Title,
//        Status = ex.StatusCode,
//        Detail = ex.Details
//    });
//}