using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FanDuel.DepthChart.Domain.Entities
{
    public record Position
    {
        public int Id { get; set; }
        [MaxLength(3)]
        public string Name { get; set; }

        public int SportId { get; set; }
        public Sport Sport { get; set; }
    }
}
