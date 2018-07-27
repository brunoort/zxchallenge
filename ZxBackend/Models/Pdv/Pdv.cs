using GeoJSON.Net.Geometry;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ZxBackend.Models.Pdv
{
    public class Pdv
    {
        public int Id { get; set; }
        public string TradingName { get; set; }
        public string OwnerName { get; set; }
        public string Document { get; set; }
        public string CoverageArea { get; set; }
        public string Address { get; set; }

        [NotMapped]
        public double Distance { get; internal set; } 
    }

}
