using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynMvp.TSP
{
    public sealed class Location
    {
        public Location(int index, float x, float y)
        {
            Index = index;
            X = x;
            Y = y;
        }

        // We could add other properties, like the name, the description
        // or anything similar that we consider useful.

        public int Index { get; private set; }
        public float X { get; private set; }
        public float Y { get; private set; }

        public double GetDistance(Location other)
        {
            float diffX = X - other.X;
            float diffY = Y - other.Y;
            return Math.Sqrt(diffX * diffX + diffY * diffY);
        }

        public static double GetTotalDistance(Location[] locations)
        {
            return GetTotalDistance(locations[0], locations);
        }

        public static double GetTotalDistance(Location startLocation, Location[] locations)
        {
            if (startLocation == null)
            {
                throw new ArgumentNullException("startLocation");
            }

            if (locations == null)
            {
                throw new ArgumentNullException("locations");
            }

            if (locations.Length == 0)
            {
                throw new ArgumentException("The locations array must have at least one element.", "locations");
            }

            foreach (Location location in locations)
            {
                if (location == null)
                {
                    throw new ArgumentException("The locations array can't contain null values.");
                }
            }

            double result = startLocation.GetDistance(locations[0]);
            int countLess1 = locations.Length - 1;
            for (int i = 0; i < countLess1; i++)
            {
                Location actual = locations[i];
                Location next = locations[i + 1];

                double distance = actual.GetDistance(next);
                result += distance;
            }

            //result += locations[locations.Length - 1].GetDistance(startLocation);

            return result;
        }

        public static void SwapLocations(Location[] locations, int index1, int index2)
        {
            if (locations == null)
            {
                throw new ArgumentNullException("locations");
            }

            if (index1 < 0 || index1 >= locations.Length)
            {
                throw new ArgumentOutOfRangeException("index1");
            }

            if (index2 < 0 || index2 >= locations.Length)
            {
                throw new ArgumentOutOfRangeException("index2");
            }

            Location location1 = locations[index1];
            Location location2 = locations[index2];
            locations[index1] = location2;
            locations[index2] = location1;
        }

        // Moves an item in the list. That is, if we go from position 1 to 5, the items
        // that were previously 2, 3, 4 and 5 become 1, 2, 3 and 4.
        public static void MoveLocations(Location[] locations, int fromIndex, int toIndex)
        {
            if (locations == null)
            {
                throw new ArgumentNullException("locations");
            }

            if (fromIndex < 0 || fromIndex >= locations.Length)
            {
                throw new ArgumentOutOfRangeException("fromIndex");
            }

            if (toIndex < 0 || toIndex >= locations.Length)
            {
                throw new ArgumentOutOfRangeException("toIndex");
            }

            Location temp = locations[fromIndex];

            if (fromIndex < toIndex)
            {
                for (int i = fromIndex + 1; i <= toIndex; i++)
                {
                    locations[i - 1] = locations[i];
                }
            }
            else
            {
                for (int i = fromIndex; i > toIndex; i--)
                {
                    locations[i] = locations[i - 1];
                }
            }

            locations[toIndex] = temp;
        }

        public static void ReverseRange(Location[] locations, int startIndex, int endIndex)
        {
            if (locations == null)
            {
                throw new ArgumentNullException("locations");
            }

            if (startIndex < 0 || startIndex >= locations.Length)
            {
                throw new ArgumentOutOfRangeException("startIndex");
            }

            if (endIndex < 0 || endIndex >= locations.Length)
            {
                throw new ArgumentOutOfRangeException("endIndex");
            }

            if (endIndex < startIndex)
            {
                int temp = endIndex;
                endIndex = startIndex;
                startIndex = temp;
            }

            while (startIndex < endIndex)
            {
                Location temp = locations[endIndex];
                locations[endIndex] = locations[startIndex];
                locations[startIndex] = temp;

                startIndex++;
                endIndex--;
            }
        }

        public override string ToString()
        {
            return X + ", " + Y;
        }
    }
}
