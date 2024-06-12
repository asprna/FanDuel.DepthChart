using FanDuel.DepthChart.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FanDuel.DepthChart.Test.Helper
{
    public class DepthChartInitializer
    {
        public static void Initializer(DepthChartContext context)
        {
            if (context.Sports.Any())
            {
                return;
            }

            Seed(context);
        }

        private static void Seed(DepthChartContext context)
        {
            context.Sports.AddRange(SeedSportData.GetSport());
            context.SaveChanges();
        }
    }
}
