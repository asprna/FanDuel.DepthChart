using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FanDuel.DepthChart.Domain.Dtos
{
    public class AddPlayerToDepthChartDto : BasedPlayerDto
    {
        public int? Rank { get; set; }
    }
}
