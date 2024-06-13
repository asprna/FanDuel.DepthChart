using FanDuel.DepthChart.Application.Behaviours;
using FanDuel.DepthChart.Application.Contracts.Persistence;
using FanDuel.DepthChart.Infrastructure.Persistence;
using FanDuel.DepthChart.Application;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Reflection;
using FanDuel.DepthChart.Application.Contracts.Business;
using FanDuel.DepthChart.Application.Services.DepthCharts;
using Microsoft.Extensions.DependencyInjection;
using FanDuel.DepthChart.API.Validation;

namespace FanDuel.DepthChart.API
{
    public class Startup
    {
        private IConfiguration _config;

        public Startup(IConfiguration configuration)
        {
            _config = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //Register DataContext
            services.AddDbContext<DepthChartContext>(opt =>
            {
                opt.UseSqlite(_config.GetConnectionString("DefaultConnection"));
            });

            services.AddScoped<IApplicationDbContext>(provider => provider.GetService<DepthChartContext>());

            services.AddTransient<IDepthChartFactory, DepthChartFactory>();

            //services.AddSingleton<Func<string, IDepthChart>>(serviceProvider => key =>
            //{
            //    var sender = serviceProvider.GetService<ISender>();

            //    return key switch
            //    {
            //        "NFL" => new NFLDepthChart(sender),
            //        "NRL" => new NRLDepthChart(sender),
            //        _ => throw new ArgumentException($"No implementation found for depth chart type: {key}")
            //    };
            //});

            services.AddValidatorsFromAssemblyContaining<AddDepthChartDtoValidator>();
            services.AddApplication();

            services.AddControllers();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "DepthChartAPI", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "DepthChartAPI v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
