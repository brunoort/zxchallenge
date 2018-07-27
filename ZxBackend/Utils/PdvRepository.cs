using System;
using System.Collections.Generic;
using GeoJSON.Net.Geometry;
using Newtonsoft.Json;
using ZxBackend.Models.Pdv;
using ZxBackend.Models.Utils;

namespace ZxBackend.Utils
{
    public static class PdvRepository
    {        
        public static Pdv SearchClosestPDV(Pdv result, double lat, double lon, Point testPoint, Pdv pdv)
        {
            var coverageAreaList = JsonConvert.DeserializeObject<CoverageArea>(pdv.CoverageArea);

            var pdvPoints = new List<PdvPoint>();
            foreach (var ptsList in coverageAreaList.coordinates[0])
            {
                foreach (var pts in ptsList)
                {
                    pdvPoints.Add(new PdvPoint { X = pts[1], Y = pts[0] });
                }
            }

            PolyGon myRoute = new PolyGon(pdvPoints);
            bool PdvIsInMultiPolygon = myRoute.FindPoint(lat, lon); //true

            if (PdvIsInMultiPolygon == true)
            {
                var address = JsonConvert.DeserializeObject<Point>(pdv.Address);
                pdv.Distance = GeoUtils.GetDistance(address, testPoint);

                result = pdv;
            }

            return result;
        }

    }
}
