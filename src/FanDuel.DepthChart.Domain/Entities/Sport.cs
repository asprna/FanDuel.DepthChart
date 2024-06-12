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

        public ICollection<Position> Positions { get; } = new List<Position>();
    }
}
