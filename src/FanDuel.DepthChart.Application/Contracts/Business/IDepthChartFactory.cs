using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FanDuel.DepthChart.Application.Contracts.Business
{
    public interface IDepthChartFactory
    {
        IDepthChart CreateDepthChart(string sportType);
    }
}
