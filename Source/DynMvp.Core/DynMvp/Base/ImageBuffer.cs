using DynMvp.Base;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynMvp.Base
{
    public class ImageBuffer
    {
        private int numLightType;
        private List<Image2D> image2DList = new List<Image2D>();
        public Image3D Image3D { get; set; }

        public ImageBuffer(int numLightType)
        {
            this.numLightType = numLightType;
        }

        public List<Image2D>.Enumerator GetEnumerator()
        {
            return image2DList.GetEnumerator();
        }

        public void AddImage(Image2D image)
        {
            image2DList.Add(image);
        }

        public Image2D GetImage(int cameraIndex, int lightTypeIndex)
        {
            int index = cameraIndex * numLightType + lightTypeIndex;
            Debug.Assert(index >= 0 && index < image2DList.Count);

            return image2DList[index];
        }

        public void Save(string folder, string prefix, ImageFormat imageFormat)
        {
            for (int index = 0; index < image2DList.Count; index++)
            {
                int cameraIndex = index / numLightType;
                int lightTypeIndex = index % numLightType;

                string fileName = string.Format(@"{0}_{1:000}.{2}", prefix, index, imageFormat.ToString());
                string filePath = Path.Combine(folder, fileName);
                image2DList[index].SaveImage(filePath, imageFormat);
            }
        }

        public void Load(string folder, string prefix, ImageFormat imageFormat)
        {
            for (int index = 0; index < image2DList.Count; index++)
            {
                int cameraIndex = index / numLightType;
                int lightTypeIndex = index % numLightType;

                string searchImageFileName = string.Format(@"{0}_*.{1}", prefix, imageFormat.ToString());

                string[] imageFiles = Directory.GetFiles(folder, searchImageFileName);
                if (imageFiles.Count() > 0)
                {
                    string imageFilePath = Path.Combine(folder, imageFiles[0]);
                    image2DList[index].LoadImage(imageFilePath);
                }
                else
                {
                    image2DList[index].Clear();
                }
            }
        }

        public List<Image2D> CreateImageList(int cameraIndex, int[] lightTypeIndexArr)
        {
            var imageList = new List<Image2D>();

            foreach (int lightTypeIndex in lightTypeIndexArr)
            {
                int index = cameraIndex * numLightType + lightTypeIndex;
                imageList.Add(image2DList[index]);
            }

            return imageList;
        }
    }
}
