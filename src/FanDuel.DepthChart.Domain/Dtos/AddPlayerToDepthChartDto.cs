using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FanDuel.DepthChart.Domain.Dtos
{
    public class AddPlayerToDepthChartDto
    {
        public int? ChartId { get; set; }
        public string Position { get; set; }
        public int PlayerId { get; set; }
        public int? Rank { get; set; }
    }
}
