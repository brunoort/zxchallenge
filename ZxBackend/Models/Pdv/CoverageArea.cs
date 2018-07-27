using GeoJSON.Net.Geometry;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ZxBackend.Models.Pdv
{
    public class CoverageArea
    {
        public string type { get; set; }
        public List<List<List<List<double>>>> coordinates { get; set; }
    }
}
