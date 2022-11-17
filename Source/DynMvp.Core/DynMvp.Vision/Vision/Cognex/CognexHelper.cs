using Cognex.VisionPro;
using DynMvp.Base;
using DynMvp.UI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;

namespace DynMvp.Vision.Cognex
{
    public class CognexHelper
    {
        private static string cogtoolResult = "";

        public static List<PointF> Convert(CogPolygon polygon)
        {
            var pointList = new List<PointF>();
            for (int i = 0; i < polygon.GetVertices().GetLength(0); i++)
            {
                pointList.Add(new PointF((float)polygon.GetVertexX(i), (float)polygon.GetVertexY(i)));
            }

            return pointList;
        }

        public static bool LicenseExist(string subLibraryType)
        {
            if (string.IsNullOrEmpty(cogtoolResult) == true)
            {
                string enviromentPath = System.Environment.GetEnvironmentVariable("PATH", EnvironmentVariableTarget.User)
                    + System.Environment.GetEnvironmentVariable("PATH", EnvironmentVariableTarget.Machine);

                Console.WriteLine(enviromentPath);
                string[] paths = enviromentPath.Split(';');
                string exePath = paths.Select(x => Path.Combine(x, "cogtool.exe"))
                                   .Where(x => File.Exists(x))
                                   .FirstOrDefault();

                if (string.IsNullOrWhiteSpace(exePath) == false)
                {
                    var p = new Process();
                    p.StartInfo.UseShellExecute = false;
                    p.StartInfo.RedirectStandardOutput = true;
                    p.StartInfo.FileName = exePath;
                    p.StartInfo.Arguments = "-p";
                    p.Start();

                    cogtoolResult = p.StandardOutput.ReadToEnd();
                    p.WaitForExit();
                }
                else
                {
                    cogtoolResult = "Not Found CogTool";
                }
            }

            return cogtoolResult.Contains(subLibraryType);
        }
    }
}
