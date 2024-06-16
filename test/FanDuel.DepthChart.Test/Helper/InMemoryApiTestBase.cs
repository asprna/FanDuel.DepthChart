using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FanDuel.DepthChart.Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace FanDuel.DepthChart.Test.Helper
{
    public class InMemoryApiTestBase : IDisposable
    {
        public readonly HttpClient Client;
        private readonly WebApplicationFactory<Program> _factory;

        public InMemoryApiTestBase()
        {
            _factory = new WebApplicationFactory<Program>()
                .WithWebHostBuilder(builder =>
                {
                    builder.ConfigureServices(services =>
                    {
                        // Remove the existing DbContext registration
                        var descriptor = services.SingleOrDefault(
                            d => d.ServiceType ==
                                typeof(DbContextOptions<DepthChartContext>));

                        if (descriptor != null)
                        {
                            services.Remove(descriptor);
                        }

                        // Add DbContext using an in-memory database for testing
                        services.AddDbContext<DepthChartContext>(options =>
                        {
                            options.UseInMemoryDatabase("InMemoryDbForTesting");
                        });

                        // Ensure the database is created
                        var sp = services.BuildServiceProvider();
                        using (var scope = sp.CreateScope())
                        {
                            var scopedServices = scope.ServiceProvider;
                            var db = scopedServices.GetRequiredService<DepthChartContext>();
                            db.Database.EnsureCreated();
                        }
                    });
                });

            Client = _factory.CreateClient();
        }

        public void Dispose()
        {
            // Dispose the factory to clean up the resources
            _factory.Dispose();
        }
    }
}
