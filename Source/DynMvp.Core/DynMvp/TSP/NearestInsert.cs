using DynMvp.Base;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynMvp.TSP
{
    public class NearestInsert : TSPAlgorithm
    {
        public Location[] FindPath(Location start, Location[] node)
        {
            var nodeList = node.ToList();
            nodeList.Sort((f, g) =>
            {
                float d1 = MathHelper.GetLength(new PointF(start.X, start.Y), new PointF(f.X, f.Y));
                float d2 = MathHelper.GetLength(new PointF(start.X, start.Y), new PointF(g.X, g.Y));
                return d1.CompareTo(d2);
            });

            var foundPath = new List<Location>();

            Location firstNode = nodeList[0];
            foundPath.Add(firstNode);
            nodeList.Remove(firstNode);

            Location lastNode = nodeList[nodeList.Count - 1];
            foundPath.Add(lastNode);
            nodeList.Remove(lastNode);

            while (nodeList.Count > 0)
            {
                List<Location> minCostPath = null;
                double minCost = double.MaxValue;
                int nodeIndex = -1;
                for (int i = 0; i < nodeList.Count; i++)
                {
                    //  임의의 한 점을 경로에 넣어본다.
                    Location insertionNode = nodeList[i];
                    List<Location> insertedPath = AddPoint(foundPath, insertionNode);
                    double cost = Location.GetTotalDistance(insertedPath[0], insertedPath.ToArray());

                    if (cost < minCost)
                    {
                        minCostPath = insertedPath;
                        minCost = cost;
                        nodeIndex = i;
                    }
                }

                // 가장 길이 변화가 작은놈을 선택한다.
                foundPath = minCostPath;
                nodeList.RemoveAt(nodeIndex);
            }

            return foundPath.ToArray();
        }

        private List<Location> AddPoint(List<Location> foundPath, Location insertionNode)
        {
            List<Location> minCostPath = null;
            double minCost = double.MaxValue;

            for (int i = 1; i < foundPath.Count; i++)
            {
                var tempPath = new List<Location>();
                tempPath.AddRange(foundPath);
                tempPath.Insert(i, insertionNode);

                double cost = Location.GetTotalDistance(tempPath[0], tempPath.ToArray());
                if (cost < minCost)
                {
                    minCost = cost;
                    minCostPath = tempPath;
                }
            }

            return minCostPath;
        }

        public void Initialize(PointF[] node)
        {
            throw new NotImplementedException();
        }
    }
}
