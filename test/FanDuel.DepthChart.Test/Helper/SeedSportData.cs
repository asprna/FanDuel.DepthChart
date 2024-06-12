using FanDuel.DepthChart.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FanDuel.DepthChart.Test.Helper
{
    public static class SeedSportData
    {
        public static Sport GetSport()
        {
            var sport = new Sport
            {
                Id = 1,
                Name = "NFL"
            };

            sport.Positions.Add(new Position { Id = 1, Name = "LWR", SportId = sport.Id, Sport = sport });
            sport.Positions.Add(new Position { Id = 2, Name = "RWR", SportId = sport.Id, Sport = sport });

            return sport;
        }
    }
}
