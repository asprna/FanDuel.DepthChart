using FanDuel.DepthChart.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FanDuel.DepthChart.Domain.Dtos
{
    public class AddDepthChartDto
    {
        public int WeekId { get; set; }
        public int TeamId { get; set; }
    }
}
