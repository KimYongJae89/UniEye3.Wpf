using DynMvp.Cad;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace DynMvp.Component.DepthSystem.DepthViewer
{
    internal class CadViewPanel// : SubPanel
    {
        private Cad3dModel cad3dModel;

        internal void SetCadData(Cad3dModel cad3dModel)
        {
            this.cad3dModel = cad3dModel;
        }

        private void CreateCadDrawList()
        {
            //if (cad3dModel == null)
            //    return;

            //float objectHeight = (maxValue - minValue) + 1;

            //cadDrawList = GL.GenLists(1);

            //GL.NewList(cadDrawList, ListMode.Compile);
            //GL.TexEnv(TextureEnvTarget.TextureEnv, TextureEnvParameter.TextureEnvMode, (Int32)TextureEnvModeCombine.Modulate);

            //GL.Color3(1.0f, 1.0f, 1.0f);
            //float zPos = 0;

            //foreach (Triangle triangle in cad3dModel.TriangleList)
            //{
            //    if (polygonMode == PolygonMode.Point)
            //        GL.Begin(PrimitiveType.Points);
            //    else
            //        GL.Begin(PrimitiveType.TriangleStrip);

            //    if (polygonMode == PolygonMode.Fill)
            //    {
            //        Point3d normal = triangle.NormalVector;

            //        GL.Normal3(normal.X, normal.Y, normal.Z);

            //        GL.TexCoord1(triangle.Vertex[0].Z / objectHeight);
            //        GL.Vertex3(triangle.Vertex[0].X, triangle.Vertex[0].Y, triangle.Vertex[0].Z);

            //        GL.TexCoord1(triangle.Vertex[1].Z / objectHeight);
            //        GL.Vertex3(triangle.Vertex[1].X, triangle.Vertex[1].Y, triangle.Vertex[1].Z);

            //        GL.TexCoord1(triangle.Vertex[2].Z / objectHeight);
            //        GL.Vertex3(triangle.Vertex[2].X, triangle.Vertex[2].Y, triangle.Vertex[2].Z);
            //    }
            //    else
            //    {
            //        GL.Vertex3(triangle.Vertex[0].X, triangle.Vertex[0].Y, triangle.Vertex[0].Z);
            //        GL.Vertex3(triangle.Vertex[1].X, triangle.Vertex[1].Y, triangle.Vertex[1].Z);
            //        GL.Vertex3(triangle.Vertex[2].X, triangle.Vertex[2].Y, triangle.Vertex[2].Z);
            //    }
            //    GL.End();
            //}
            //GL.EndList();

            //currentTextureDimType = textureDimType;
            //cadDrawListCreated = true;
        }
    }
}
