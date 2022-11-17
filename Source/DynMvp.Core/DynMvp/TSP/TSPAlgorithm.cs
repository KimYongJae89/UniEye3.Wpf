using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynMvp.TSP
{
    public interface TSPAlgorithm
    {
        void Initialize(PointF[] node);
        Location[] FindPath(Location start, Location[] node);
    }

    internal class TSPAlgorithmParam
    {

    }

    public class TSPImprove
    {
        public static Location[] Improve2Opt(Location[] locations)
        {
            var optiLocations = (Location[])locations.Clone();
            double optiDist = Location.GetTotalDistance(locations);

            int count = locations.Count();
            for (int i = 0; i < count - 2; i++)
            {
                for (int j = i + 1; j < count - 1; j++)
                {
                    Location.SwapLocations(locations, i, j);
                    double dist = Location.GetTotalDistance(locations);
                    if (optiDist > dist)
                    {
                        optiLocations = (Location[])locations.Clone();
                        optiDist = dist;
                    }
                }
            }

            return optiLocations;
        }
    }
}
