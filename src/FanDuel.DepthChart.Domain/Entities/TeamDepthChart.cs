using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FanDuel.DepthChart.Domain.Entities
{
    public record TeamDepthChart
    {
        public int Id { get; set; }
        public int WeekId { get; set; }
        public DateTime CreatedDateTimeUtc { get; set; }

        public int TeamId { get; set; }
        public Team Team { get; set; }

        public ICollection<PlayerChartIndex> PlayerChartIndexs { get; set; } = new List<PlayerChartIndex>();
    }
}
