using FanDuel.DepthChart.Infrastructure.Persistence;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;

namespace FanDuel.DepthChart.ConsoleApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var app = CreateHostBuilder(args).Build();
        }

        static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    config.SetBasePath(Directory.GetCurrentDirectory());
                    config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
                })
                .ConfigureServices((hostContext, services) =>
                {
                    var configuration = hostContext.Configuration;

                    services.AddDbContext<DepthChartContext>(opt =>
                    {
                        opt.UseSqlite(configuration.GetConnectionString("DefaultConnection"));
                    });

                    services.AddSingleton<IConfiguration>(configuration);
                });
    }
}
