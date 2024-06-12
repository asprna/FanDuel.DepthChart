using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FanDuel.DepthChart.Domain.Entities
{
    public record Team
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public int SportId { get; set; }
        public Sport Sport { get; set; }

        public ICollection<Player> Players { get; } = new List<Player>();
        public ICollection<TeamDepthChart> TeamDepthCharts { get; } = new List<TeamDepthChart>();
    }
}
