using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynMvp.TSP
{
    public class Genatic : TSPAlgorithm
    {
        private readonly Location _startLocation;
        private readonly KeyValuePair<Location[], double>[] _populationWithDistances;

        public Genatic(Location startLocation, Location[] destinations, int populationCount)
        {
            if (startLocation == null)
            {
                throw new ArgumentNullException("startLocation");
            }

            if (destinations == null)
            {
                throw new ArgumentNullException("destinations");
            }

            if (populationCount < 2)
            {
                throw new ArgumentOutOfRangeException("populationCount");
            }

            if (populationCount % 2 != 0)
            {
                throw new ArgumentException("The populationCount parameter must be an even value.", "populationCount");
            }

            _startLocation = startLocation;
            destinations = (Location[])destinations.Clone();

            foreach (Location destination in destinations)
            {
                if (destination == null)
                {
                    throw new ArgumentException("The destinations array can't contain null values.", "destinations");
                }
            }

            // This commented method uses a search of the kind "look for the nearest non visited location".
            // This is rarely the shortest path, yet it is already a "somewhat good" path.
            //destinations = _GetFakeShortest(destinations);

            _populationWithDistances = new KeyValuePair<Location[], double>[populationCount];

            // Create initial population.
            for (int solutionIndex = 0; solutionIndex < populationCount; solutionIndex++)
            {
                var newPossibleDestinations = (Location[])destinations.Clone();

                // Try commenting the next 2 lines of code while keeping the _GetFakeShortest active.
                // If you avoid the algorithm from running and press reset, you will see that it always
                // start with a path that seems "good" but is not the best.
                for (int randomIndex = 0; randomIndex < newPossibleDestinations.Length; randomIndex++)
                {
                    RandomProvider.FullyRandomizeLocations(newPossibleDestinations);
                }

                double distance = Location.GetTotalDistance(startLocation, newPossibleDestinations);
                var pair = new KeyValuePair<Location[], double>(newPossibleDestinations, distance);

                _populationWithDistances[solutionIndex] = pair;
            }

            Array.Sort(_populationWithDistances, _sortDelegate);
        }

        private Location[] _GetFakeShortest(Location[] destinations)
        {
            var result = new Location[destinations.Length];

            Location currentLocation = _startLocation;
            for (int fillingIndex = 0; fillingIndex < destinations.Length; fillingIndex++)
            {
                int bestIndex = -1;
                double bestDistance = double.MaxValue;

                for (int evaluatingIndex = 0; evaluatingIndex < destinations.Length; evaluatingIndex++)
                {
                    Location evaluatingItem = destinations[evaluatingIndex];
                    if (evaluatingItem == null)
                    {
                        continue;
                    }

                    double distance = currentLocation.GetDistance(evaluatingItem);
                    if (distance < bestDistance)
                    {
                        bestDistance = distance;
                        bestIndex = evaluatingIndex;
                    }
                }

                result[fillingIndex] = destinations[bestIndex];
                currentLocation = destinations[bestIndex];
                destinations[bestIndex] = null;
            }

            return result;
        }

        private static readonly Comparison<KeyValuePair<Location[], double>> _sortDelegate = _Sort;
        private static int _Sort(KeyValuePair<Location[], double> value1, KeyValuePair<Location[], double> value2)
        {
            return value1.Value.CompareTo(value2.Value);
        }

        public IEnumerable<Location> GetBestSolutionSoFar()
        {
            foreach (Location location in _populationWithDistances[0].Key)
            {
                yield return location;
            }
        }

        public bool MustMutateFailedCrossovers { get; set; }
        public bool MustDoCrossovers { get; set; }

        public void Reproduce()
        {
            KeyValuePair<Location[], double> bestSoFar = _populationWithDistances[0];

            int halfCount = _populationWithDistances.Length / 2;
            for (int i = 0; i < halfCount; i++)
            {
                Location[] parent = _populationWithDistances[i].Key;
                Location[] child1 = _Reproduce(parent);
                Location[] child2 = _Reproduce(parent);

                var pair1 = new KeyValuePair<Location[], double>(child1, Location.GetTotalDistance(_startLocation, child1));
                var pair2 = new KeyValuePair<Location[], double>(child2, Location.GetTotalDistance(_startLocation, child2));
                _populationWithDistances[i * 2] = pair1;
                _populationWithDistances[i * 2 + 1] = pair2;
            }

            // We keep the best alive from one generation to the other.
            _populationWithDistances[_populationWithDistances.Length - 1] = bestSoFar;

            Array.Sort(_populationWithDistances, _sortDelegate);
        }

        public void MutateDuplicates()
        {
            bool needToSortAgain = false;
            int countDuplicates = 0;

            KeyValuePair<Location[], double> previous = _populationWithDistances[0];
            for (int i = 1; i < _populationWithDistances.Length; i++)
            {
                KeyValuePair<Location[], double> current = _populationWithDistances[i];
                if (!previous.Key.SequenceEqual(current.Key))
                {
                    previous = current;
                    continue;
                }

                countDuplicates++;

                needToSortAgain = true;
                RandomProvider.MutateRandomLocations(current.Key);
                _populationWithDistances[i] = new KeyValuePair<Location[], double>(current.Key, Location.GetTotalDistance(_startLocation, current.Key));
            }

            if (needToSortAgain)
            {
                Array.Sort(_populationWithDistances, _sortDelegate);
            }
        }

        private Location[] _Reproduce(Location[] parent)
        {
            var result = (Location[])parent.Clone();

            if (!MustDoCrossovers)
            {
                // When we are not using cross-overs, we always apply mutations.
                RandomProvider.MutateRandomLocations(result);
                return result;
            }

            // if you want, you can ignore the next three lines of code and the next
            // if, keeping the call to RandomProvider.MutateRandomLocations(result); always
            // invoked and without crossovers. Doing that you will not promove evolution through
            // "sexual reproduction", yet the good result will probably be found.
            int otherIndex = RandomProvider.GetRandomValue(_populationWithDistances.Length / 2);
            Location[] other = _populationWithDistances[otherIndex].Key;
            RandomProvider._CrossOver(result, other, MustMutateFailedCrossovers);

            if (!MustMutateFailedCrossovers)
            {
                if (RandomProvider.GetRandomValue(10) == 0)
                {
                    RandomProvider.MutateRandomLocations(result);
                }
            }

            return result;
        }

        public void Initialize(PointF[] node)
        {

        }

        public Location[] FindPath(Location start, Location[] node)
        {
            Location fidLocation = start;

            var fovLocationList = new List<Location>();
            fovLocationList.AddRange(node);

            Location[] fovLocationArray = fovLocationList.ToArray();

            var algorithm = new Genatic(fidLocation, fovLocationArray, 500);
            algorithm.MustMutateFailedCrossovers = false;
            algorithm.MustDoCrossovers = true;

            int matchCount = 0;
            while (matchCount < 100)
            {
                algorithm.Reproduce();
                algorithm.MutateDuplicates();
                Location[] newFovLocationArray = algorithm.GetBestSolutionSoFar().ToArray();
                long distance = (long)Location.GetTotalDistance(fidLocation, newFovLocationArray);
                if (newFovLocationArray.SequenceEqual(fovLocationArray))
                {
                    matchCount++;
                }
                else
                {
                    Debug.WriteLine(string.Format("foun new optimal Path. {0}", distance));
                    matchCount = 0;
                    fovLocationArray = newFovLocationArray;
                }
            }

            //List<PointF> foundPath = new List<PointF>();
            //foreach (Location location in fovLocationArray)
            //{
            //    foundPath.Add(new PointF(location.X, location.Y));
            //}

            return (Location[])fovLocationArray.Clone();

        }
    }
}
