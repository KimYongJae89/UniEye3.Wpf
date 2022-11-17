using DynMvp.Base;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynMvp.Devices.FrameGrabber
{
    public interface IImageSequence
    {
        void Dispose();

        void MoveNext();
        void MovePrev();
        void ResetIndex();

        void SetImageNameFormat(string imageNameFormat);
        bool SetImagePath(string rootImagePath);

        Image2D GetImage(int cameraIndex);

        Image2D GetImage(int cameraIndex, int stepIndex, int lightIndex);
        void SetImageIndex(int imageIndex);
        void SetImageIndex(int stepIndex, int lightIndex);

        void SetImage(int cameraIndex, int stepIndex, int lightIndex, Image2D image);
        void LoadAll();
    }

    /// <summary>
    /// Folder 단위로 가상 이미지가 생성되는 프로그램에서 사용
    /// </summary>
    public class ImageSequenceFolder : IImageSequence
    {
        private Image2D[] image2Ds = null;
        private string[] imagePathArr = null;
        private string imageNameFormat = "";
        private int imageIndex;
        private int stepIndex;
        private int lightIndex;

        public void Dispose()
        {
            if (image2Ds != null)
            {
                lock (image2Ds)
                {
                    Array.ForEach(image2Ds, f => f?.Dispose());
                }
            }
            image2Ds = null;
        }

        public void MoveNext()
        {
            imageIndex = (imageIndex + 1) % imagePathArr.Count();
            //if (imageIndex < (imagePathArr.Count() - 1))
            //    imageIndex++;
        }

        public void MovePrev()
        {
            imageIndex = (imageIndex - 1 + imagePathArr.Count()) % imagePathArr.Count();
            //if (imageIndex > 0)
            //    imageIndex--;
        }

        public void ResetIndex()
        {
            imageIndex = -1;
        }

        public void SetImageIndex(int imageIndex)
        {
            this.imageIndex = imageIndex;
        }

        public void SetImageNameFormat(string imageNameFormat)
        {
            this.imageNameFormat = imageNameFormat;
        }

        public bool SetImagePath(string rootImagePath)
        {
            string dirName = Path.GetDirectoryName(rootImagePath);
            string fileName = Path.GetFileName(rootImagePath);

            var directoryInfo = new DirectoryInfo(dirName);
            var nameList = directoryInfo.GetFiles(fileName).Select(f => new string[] { f.Name.Replace(f.Extension, ""), f.Extension, f.Name.Split('_')[0] }).ToList();
            nameList.Sort((f, g) =>
            {
                if (int.TryParse(f[2], out int i) && int.TryParse(g[2], out int j))
                {
                    return i.CompareTo(j);
                }

                return f[0].CompareTo(g[0]);
            });

            imagePathArr = nameList.Select(f => Path.Combine(directoryInfo.FullName, $"{f[0]}{f[1]}")).ToArray();
            image2Ds = new Image2D[imagePathArr.Length];
            imageIndex = -1;

            //string[] dirs = Directory.GetDirectories(rootImagePath);
            //if (dirs.Count() == 0)
            //    dirs = new string[1] { rootImagePath };

            //this.imagePathArr = dirs;
            //imageIndex = 0;

            return true;
        }

        public void LoadAll()
        {
            System.Diagnostics.Debug.Assert(imagePathArr.Length == image2Ds.Length);
            for (int i = 0; i < imagePathArr.Length; i++)
            {
                image2Ds[i] = new Image2D(imagePathArr[i]);
            }
        }

        public void SetImageIndex(int stepIndex, int lightIndex)
        {
            this.stepIndex = stepIndex;
            this.lightIndex = lightIndex;
        }

        private string GetImageFilePath(int cameraIndex, int stepIndex, int lightIndex)
        {
            string imagePath = imagePathArr[imageIndex];
            if (Directory.Exists(imagePath) == false)
            {
                return "";
            }

            string imageFilePath = Path.Combine(imagePath, string.Format("image{0}_C{1:00}_S{2:00}_L{3:00}.{4}",
                imageNameFormat, cameraIndex, stepIndex, lightIndex, ImageFormat.Bmp.ToString()));

            if (File.Exists(imageFilePath) == false)
            {
                return "";
            }

            return imageFilePath;
        }

        public Image2D GetImage(int cameraIndex)
        {
            if (imageIndex < 0)
            {
                imageIndex = 0;
            }

            if (image2Ds[imageIndex] == null)
            {
                if (!File.Exists(imagePathArr[imageIndex]))
                {
                    throw new FileNotFoundException();
                }

                System.Diagnostics.Debug.WriteLine($"ImageSequenceFolder::GetImage - {imagePathArr[imageIndex]}");
                image2Ds[imageIndex] = new Image2D(imagePathArr[imageIndex]);
            }
            return image2Ds[imageIndex].Clone() as Image2D;
        }

        public Image2D GetImage(int cameraIndex, int stepIndex, int lightIndex)
        {
            string imageFileName = GetImageFilePath(cameraIndex, stepIndex, lightIndex);
            if (string.IsNullOrEmpty(imageFileName))
            {
                return null;
            }

            return new Image2D(imageFileName);
        }

        public void SetImage(int cameraIndex, int stepIndex, int lightIndex, Image2D image)
        {
            string imagePath = imagePathArr[imageIndex];
            if (Directory.Exists(imagePath) == false)
            {
                return;
            }

            string imageFilePath = Path.Combine(imagePath, string.Format("image{0}_C{1:00}_S{2:00}_L{3:00}.{4}",
                imageNameFormat, cameraIndex, stepIndex, lightIndex, ImageFormat.Bmp.ToString()));

            image.SaveImage(imageFilePath, ImageFormat.Bmp);
        }
    }

    /// <summary>
    /// 하나의 폴더에 다수의 가상 이미지가 생성되는 프로그램에서 사용
    /// 단일 카메라, 단일 조명의 H/W Trigger 적용 프로그램
    /// </summary>
    public class ImageSequenceFile : IImageSequence
    {
        private string imagePath = null;
        private string imageNameFormat;
        private int imageIndex;
        private int stepIndex = 0;
        private int lightIndex = 0;

        public void Dispose() { }

        public void MoveNext()
        {
            imageIndex++;

            string imageFilePath = GetImageFilePath(0, 0, 0);
            if (string.IsNullOrEmpty(imageFilePath))
            {
                imageIndex--;
            }
        }

        public void MovePrev()
        {
            if (imageIndex > 0)
            {
                imageIndex--;
            }
        }

        public void ResetIndex()
        {
            imageIndex = 0;
        }

        public void SetImageIndex(int imageIndex)
        {
            throw new NotImplementedException();
        }

        public void SetImageNameFormat(string imageNameFormat)
        {
            this.imageNameFormat = imageNameFormat;
        }

        public bool SetImagePath(string rootImagePath)
        {
            if (Directory.Exists(rootImagePath) == false)
            {
                imagePath = null;
                return false;
            }

            imagePath = rootImagePath;
            imageIndex = 0;

            return true;
        }

        public void LoadAll()
        {

        }

        public void SetImageIndex(int stepIndex, int lightIndex)
        {
            this.stepIndex = stepIndex;
            this.lightIndex = lightIndex;
        }

        private string GetImageFilePath(int cameraIndex, int stepIndex, int lightIndex)
        {
            if (imagePath == null)
            {
                return "";
            }

            string imageFilePath = Path.Combine(imagePath, string.Format("image_C{0:00}_S{1:00}_L{2:00}.{3}",
                imageNameFormat, cameraIndex, stepIndex, lightIndex, ImageFormat.Bmp.ToString()));
            if (File.Exists(imageFilePath) == false)
            {
                return "";
            }

            return imageFilePath;
        }

        public Image2D GetImage(int cameraIndex)
        {
            return GetImage(cameraIndex, stepIndex, lightIndex);
        }

        public Image2D GetImage(int cameraIndex, int stepIndex, int lightIndex)
        {
            string imageFileName = GetImageFilePath(cameraIndex, stepIndex, lightIndex);
            if (string.IsNullOrEmpty(imageFileName))
            {
                return null;
            }

            return new Image2D(imageFileName);
        }

        public void SetImage(int cameraIndex, int stepIndex, int lightIndex, Image2D image)
        {
            string imageFileName = GetImageFilePath(cameraIndex, stepIndex, lightIndex);
            image.SaveImage(imageFileName, ImageFormat.Bmp);
        }
    }

    public class ImageSequenceRoll : IImageSequence
    {
        private Size frameSize;
        private Image2D masterImage;
        private int masterImageYPos;
        private string masterImagePath;
        private Image2D frameImage;

        public ImageSequenceRoll(Size size)
        {
            masterImageYPos = 0;
            frameSize = size;
        }

        public void Dispose()
        {
            masterImage?.Dispose();
            frameImage?.Dispose();
        }

        public Image2D GetImage(int cameraIndex)
        {
            //return (Image2D)frameImage.Clone();
            return frameImage;
        }

        public Image2D GetImage(int cameraIndex, int stepIndex, int lightIndex)
        {
            return (Image2D)frameImage.Clone();
        }

        public void LoadAll()
        {
            throw new NotImplementedException();
        }

        public void MoveNext()
        {
            int length = frameImage.Pitch;
            for (int i = 0; i < frameSize.Height; i++)
            {
                int srcOffset = masterImage.Pitch * masterImageYPos;
                int dstOffset = frameImage.Pitch * i;
                Buffer.BlockCopy(masterImage.Data, srcOffset, frameImage.Data, dstOffset, length);
                masterImageYPos++;
                if (masterImageYPos >= masterImage.Height)
                {
                    masterImageYPos = 0;
                }
            }
        }

        public void MovePrev()
        {
            throw new NotImplementedException();
        }

        public void ResetIndex()
        {
            masterImageYPos = 0;
            frameImage.Clear();
        }

        public void SetImage(int cameraIndex, int stepIndex, int lightIndex, Image2D image)
        {
            throw new NotImplementedException();
        }

        public void SetImageIndex(int imageIndex)
        {
            throw new NotImplementedException();
        }

        public void SetImageIndex(int stepIndex, int lightIndex)
        {
            throw new NotImplementedException();
        }

        public void SetImageNameFormat(string imageNameFormat)
        {
            string[] fileInfos = Directory.GetFiles(masterImagePath, imageNameFormat);
            if (fileInfos.Length > 0)
            {
                masterImage = new Image2D(fileInfos.First());
            }

            frameImage = new Image2D(frameSize.Width, frameSize.Height, 1);
        }

        public bool SetImagePath(string rootImagePath)
        {
            masterImagePath = rootImagePath;
            return true;
        }
    }
}
