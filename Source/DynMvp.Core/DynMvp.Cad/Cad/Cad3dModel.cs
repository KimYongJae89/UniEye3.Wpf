using DynMvp.Base;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace DynMvp.Cad
{
    public class Triangle
    {
        public Point3d[] Vertex { get; } = new Point3d[3];
        public Point3d NormalVector { get; set; }
        public uint Attribute { get; set; }
    }

    public class Cad3dModel
    {
        public List<Triangle> TriangleList { get; set; } = new List<Triangle>();
        public Point3d CenterPt { get; private set; }
        public SizeF Size { get; private set; }
        public float MinValue { get; private set; }
        public float MaxValue { get; private set; }

        internal void AddTriangle(Triangle triangle)
        {
            TriangleList.Add(triangle);
        }

        public void UpdateData()
        {
            float minX = float.MaxValue, minY = float.MaxValue, minZ = float.MaxValue;
            float maxX = float.MinValue, maxY = float.MinValue, maxZ = float.MinValue;

            foreach (Triangle triangle in TriangleList)
            {
                Point3d[] vertex = triangle.Vertex;
                minX = (float)Math.Min(minX, Math.Min(Math.Min(vertex[0].X, vertex[1].X), vertex[2].X));
                minY = (float)Math.Min(minY, Math.Min(Math.Min(vertex[0].Y, vertex[1].Y), vertex[2].Y));
                minZ = (float)Math.Min(minZ, Math.Min(Math.Min(vertex[0].Z, vertex[1].Z), vertex[2].Z));
                maxX = (float)Math.Max(maxX, Math.Max(Math.Max(vertex[0].X, vertex[1].X), vertex[2].X));
                maxY = (float)Math.Max(maxY, Math.Max(Math.Max(vertex[0].Y, vertex[1].Y), vertex[2].Y));
                maxZ = (float)Math.Max(maxZ, Math.Max(Math.Max(vertex[0].Z, vertex[1].Z), vertex[2].Z));
            }

            float width = (maxX - minX);
            float height = (maxY - minY);
            Size = new SizeF(width, height);
            MinValue = minZ;
            MaxValue = maxZ;
            CenterPt = new Point3d((maxX + minX) / 2, (maxY + minY) / 2, (maxZ + minZ) / 2);
        }
    }
}
