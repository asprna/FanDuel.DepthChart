using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FanDuel.DepthChart.Domain.Entities
{
    public record PlayerChartIndex
    {
        public int Id { get; set; }
        public int Rank { get; set; }
        public DateTime CreatedDateTimeUtc { get; set; }
        public DateTime ModifiedDateTimeUtc { get; set; }

        public int TeamDepthChartId { get; set; }
        public TeamDepthChart TeamDepthChart { get; set; }

        public int PayerId { get; set; }
        public Player Player { get; set; }

        public int PositionId { get; set; }
        public Position Position { get; set; }
    }
}
