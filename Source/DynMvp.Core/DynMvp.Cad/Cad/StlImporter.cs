using DynMvp.Base;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace DynMvp.Cad
{
    public class StlImporter : CadImporter
    {
        public override Cad3dModel Import(string fileName)
        {
            Cad3dModel cad3dModel;

            byte[] typeTester = new byte[5];
            FileStream fileStream = File.OpenRead(fileName);
            fileStream.Read(typeTester, 0, 5);

            fileStream.Position = 0;

            string typeString = System.Text.Encoding.Default.GetString(typeTester.ToArray());
            if (typeString == "solid")
            {
                cad3dModel = ImportText(fileName);
            }
            else
            {
                cad3dModel = ImportBinary(fileStream);
            }

            return cad3dModel;
        }

        private Cad3dModel ImportBinary(FileStream fileStream)
        {
            var cad3dModel = new Cad3dModel();

            var reader = new BinaryReader(fileStream);

            byte[] header = new byte[80];
            reader.Read(header, 0, 80);

            uint numTriangle = reader.ReadUInt32();
            for (int i = 0; i < numTriangle; i++)
            {
                Triangle triangle = CreateTriangle(reader);
                cad3dModel.AddTriangle(triangle);
            }

            return cad3dModel;
        }

        private Cad3dModel ImportText(string fileName)
        {
            var cad3dModel = new Cad3dModel();

            using (StreamReader reader = File.OpenText(fileName))
            {
                while (true)
                {
                    string line = reader.ReadLine();
                    if (string.IsNullOrEmpty(line))
                    {
                        break;
                    }

                    if (line.Contains("facet"))
                    {
                        Triangle triangle = CreateTriangle(reader, line);
                        cad3dModel.AddTriangle(triangle);
                    }
                }
            }

            return cad3dModel;
        }

        private Triangle CreateTriangle(StreamReader reader, string firstLine)
        {
            var triangle = new Triangle();

            firstLine = firstLine.Trim();

            string[] tokens = firstLine.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            triangle.NormalVector = new Point3d(Convert.ToSingle(tokens[2]), Convert.ToSingle(tokens[3]), Convert.ToSingle(tokens[4]));

            reader.ReadLine(); // outer loop
            triangle.Vertex[0] = CreateVertex(reader.ReadLine());
            triangle.Vertex[1] = CreateVertex(reader.ReadLine());
            triangle.Vertex[2] = CreateVertex(reader.ReadLine());
            reader.ReadLine(); // endloop
            reader.ReadLine(); // endfacet

            return triangle;
        }

        private Point3d CreateVertex(string vertexLine)
        {
            vertexLine = vertexLine.Trim();

            string[] tokens = vertexLine.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            return new Point3d(Convert.ToSingle(tokens[1]), Convert.ToSingle(tokens[2]), Convert.ToSingle(tokens[3]));
        }

        private Triangle CreateTriangle(BinaryReader reader)
        {
            var triangle = new Triangle();
            triangle.Vertex[0] = CreateVertex(reader);
            triangle.Vertex[1] = CreateVertex(reader);
            triangle.Vertex[2] = CreateVertex(reader);
            triangle.NormalVector = CreateVertex(reader);
            triangle.Attribute = reader.ReadUInt16();

            return triangle;
        }

        private Point3d CreateVertex(BinaryReader reader)
        {
            var vertex = new Point3d();
            vertex.X = reader.ReadSingle();
            vertex.Y = reader.ReadSingle();
            vertex.Z = reader.ReadSingle();

            return vertex;
        }
    }
}
