using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FanDuel.DepthChart.Domain.Entities
{
    public record Sport
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<Position> Positions { get; set; } = new List<Position>();
        public ICollection<Team> Teams { get; } = new List<Team>();
    }
}
