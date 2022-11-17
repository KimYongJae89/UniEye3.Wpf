using DynMvp.Base;
using DynMvp.Vision.Euresys;
using DynMvp.Vision.OpenCv;
using Euresys.Open_eVision_1_2;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;

namespace DynMvp.Vision
{
    public class DebugContext
    {
        public bool SaveDebugImage { get; set; }

        public string Path { get; }

        public DebugContext(bool saveDebugImage, string path)
        {
            SaveDebugImage = saveDebugImage;
            Path = System.IO.Path.GetFullPath(path);

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }
    }

    public class DebugHelper
    {
        public static void SaveImage(ImageD image, string fileName, DebugContext debugContext)
        {
            if (debugContext.SaveDebugImage == false)
            {
                return;
            }

            Directory.CreateDirectory(debugContext.Path);

            string filePath = string.Format("{0}\\{1}", debugContext.Path, fileName);
            if (image is Image2D)
            {
                if (((Image2D)image).IsUseIntPtr() != true)
                {
                    image.SaveImage(filePath, ImageFormat.Bmp);
                }
            }
            else
            {
                image.SaveImage(filePath, ImageFormat.Bmp);
            }
        }

        public static void SaveImage(AlgoImage image, string fileName, DebugContext debugContext, bool forceSave = false)
        {
            if (forceSave == false && debugContext.SaveDebugImage == false)
            {
                return;
            }

            Directory.CreateDirectory(debugContext.Path);

            image.Save(fileName, debugContext);
        }

        public static void ExportArray(string fileName, float[] values)
        {
            string fullPath = Path.Combine(BaseConfig.Instance().TempPath, fileName);

            var fs = new FileStream(fullPath, FileMode.Create);
            if (fs != null)
            {
                var sw = new StreamWriter(fs, Encoding.Default);

                foreach (float value in values)
                {
                    sw.WriteLine(value.ToString());
                }

                sw.Close();
                fs.Close();
            }
        }
    }
}
