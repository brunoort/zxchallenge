using GeoJSON.Net.Geometry;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using ZxBackend.Models.Pdv;

namespace ZxBackend.Models.Utils
{
    public class PolyGon
    {
        public List<PdvPoint> myPts = new List<PdvPoint>();
        public PolyGon()
        {
        }

        public PolyGon(List<PdvPoint> points)
        {
            foreach (PdvPoint p in points)
            {
                this.myPts.Add(p);
            }
        }
        public void Add(PdvPoint p)
        {
            this.myPts.Add(p);
        }
        public int Count()
        {
            return myPts.Count;
        }

        public bool FindPoint(double X, double Y)
        {
            int sides = this.Count();

            int j = sides - 1;
            bool pointStatus = false;

            for (int i = 0; i < sides; i++)
            {
                if (myPts[i].Y > Y && myPts[j].Y >= Y || myPts[j].Y > Y && myPts[i].Y >= Y)
                {
                    if (myPts[i].X + (Y - myPts[i].Y) / (myPts[j].Y - myPts[i].Y) * (myPts[j].X - myPts[i].X) < X)
                    {
                        pointStatus = !pointStatus;
                    }
                }
                j = i;
            }
            return pointStatus;
        }
    }
}