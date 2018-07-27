using GeoJSON.Net.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ZxBackend.Utils
{
    public static class GeoUtils
    {
        public static bool IsPointInMultiPolygon(Point testPoint, MultiPolygon coverageArea)
        {
            foreach (Polygon polygon in coverageArea.Coordinates)
            {
                var points = new List<Point>();
                foreach (LineString linestr in polygon.Coordinates)
                {
                    foreach (var position in linestr.Coordinates)
                    {
                        points.Add(new Point(position));
                    }
                }
                if (IsPointInPolygon(points.ToArray(), testPoint)) return true;
            }
            return false;
        }

        public static bool IsPointInPolygon(Point[] polygon, Point testPoint)
        {
            bool result = false;
            int j = polygon.Count() - 1;
            for (int i = 0; i < polygon.Count(); i++)
            {
                if (polygon[i].Coordinates.Latitude < testPoint.Coordinates.Latitude && polygon[j].Coordinates.Latitude >= testPoint.Coordinates.Latitude
                    || polygon[j].Coordinates.Latitude < testPoint.Coordinates.Latitude && polygon[i].Coordinates.Latitude >= testPoint.Coordinates.Latitude)
                {
                    if (polygon[i].Coordinates.Longitude + (testPoint.Coordinates.Latitude - polygon[i].Coordinates.Latitude)
                        / (polygon[j].Coordinates.Latitude - polygon[i].Coordinates.Latitude) *
                            (polygon[j].Coordinates.Longitude - polygon[i].Coordinates.Longitude) < testPoint.Coordinates.Longitude)
                    {
                        result = !result;
                    }
                }
                j = i;
            }
            return result;
        }

        public static double GetDistance(Point a, Point b)
        {
            return GetDistance(a.Coordinates.Latitude, a.Coordinates.Longitude, b.Coordinates.Latitude, b.Coordinates.Longitude);
        }

        public static double deg2rad(double deg) {
            return (deg * Math.PI / 180.0);
        }

        public static double rad2deg(double rad) {
        return (rad / Math.PI * 180.0);
        }

        public static double GetDistance(double x1, double y1, double x2, double y2)
        {
            var unit = 'K';
            double theta = x1 - y1;
            double dist = Math.Sin(deg2rad(x1)) * Math.Sin(deg2rad(x2)) + Math.Cos(deg2rad(x1)) * Math.Cos(deg2rad(x2)) * Math.Cos(deg2rad(theta));
            dist = Math.Acos(dist);
            dist = rad2deg(dist);
            dist = dist * 60 * 1.1515;
            if (unit == 'K') {
                dist = dist * 1.609344;
            } else if (unit == 'N') {
                dist = dist * 0.8684;
                }
            return (Math.Round(dist/1000,1));

            //return Math.Sqrt(Math.Pow((x2 - x1), 2) + Math.Pow((y2 - y1), 2));
        }

    }
}
