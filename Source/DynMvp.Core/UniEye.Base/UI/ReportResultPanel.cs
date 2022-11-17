using DynMvp.Base;
using DynMvp.Component.DepthSystem.DepthViewer;
using DynMvp.Data.UI;
using DynMvp.Devices;
using DynMvp.Inspect;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UniEye.Base.Config;
using UniEye.Base.Settings;

namespace UniEye.Base.UI
{
    public partial class ReportResultPanel : UserControl, IReportPanel
    {
        public List<DrawBox> DrawBoxList { get; set; } = new List<DrawBox>();
        public int ViewIndex { get; set; }

        public ReportResultPanel()
        {
            InitializeComponent();

            AddPictureBox();
        }

        public void ClearPanel()
        {
            foreach (DrawBox drawBox in DrawBoxList)
            {
                drawBox.UpdateImage(null);
            }
        }

        private void AddPictureBox()
        {
            int numOfResultView = DeviceConfig.Instance().GetNumCamera();

            resultImageTable.ColumnStyles.Clear();
            resultImageTable.RowStyles.Clear();

            int numCount = (int)Math.Ceiling(Math.Sqrt(numOfResultView));
            resultImageTable.ColumnCount = numCount;
            resultImageTable.RowCount = (int)Math.Ceiling(Math.Sqrt(numOfResultView));

            for (int i = 0; i < resultImageTable.ColumnCount; i++)
            {
                resultImageTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100 / resultImageTable.ColumnCount));
            }

            for (int i = 0; i < resultImageTable.RowCount; i++)
            {
                resultImageTable.RowStyles.Add(new RowStyle(SizeType.Percent, 100 / resultImageTable.RowCount));
            }

            float widthPct = (float)100 / numOfResultView;

            resultImageTable.Controls.Clear();
            resultImageTable.Dock = DockStyle.None;

            for (int i = 0; i < numOfResultView; i++)
            {
                DrawBoxList.Add(new DrawBox());

                DrawBoxList[i].Size = new Size(1024, 768);
                DrawBoxList[i].Dock = DockStyle.Fill;

                int rowIndex = i / numCount;
                int colIndex = i % numCount;

                resultImageTable.Controls.Add(DrawBoxList[i], colIndex, rowIndex);
            }

            resultImageTable.Dock = DockStyle.Fill;
        }

        private List<string> GetSnapShotPathList(string resultPath, string surfix = "")
        {
            var searchPathList = new List<string>();

            var imagePathList = new List<string>();
            string imageName = "Result.jpg";

            imagePathList.Add(string.Format("{0}\\{1}", resultPath, imageName));


            string imagePath = "";
            foreach (string searchImagePath in imagePathList)
            {
                if (File.Exists(searchImagePath) == true)
                {
                    LogHelper.Debug(LoggerType.Operation, "Search Path - " + searchImagePath);
                    searchPathList.Add(searchImagePath);
                }
            }
            imagePathList.Clear();

            LogHelper.Debug(LoggerType.Operation, "Found path - " + imagePath);

            return searchPathList;
        }

        private List<string> GetImagePathList(string resultPath, int stepNo, string surfix = "")
        {
            var searchPathList = new List<string>();

            var imagePathList = new List<string>();
            for (int i = 0; i < ViewIndex; i++)
            {
                string imageName = string.Format("Image_C{0:00}_S{1:000}_L00{2}", i, stepNo, surfix);
                imagePathList.Add(string.Format("{0}\\{1}.bmp", resultPath, imageName));
                imagePathList.Add(string.Format("{0}\\{1}.jpeg", resultPath, imageName));
                imagePathList.Add(string.Format("{0}\\{1}.png", resultPath, imageName));
                imagePathList.Add(string.Format("{0}\\Image_C{1:00}_S{2:000}.3d", resultPath, i, stepNo));
            }

            string imagePath = "";
            foreach (string searchImagePath in imagePathList)
            {
                if (File.Exists(searchImagePath) == true)
                {
                    LogHelper.Debug(LoggerType.Operation, "Search Path - " + searchImagePath);
                    searchPathList.Add(searchImagePath);
                }
            }
            imagePathList.Clear();

            LogHelper.Debug(LoggerType.Operation, "Found path - " + imagePath);

            return searchPathList;
        }

        public void ShowResult(string resultPath, int stepNo)
        {
            var imagePathList = new List<string>();
            imagePathList = GetImagePathList(resultPath, stepNo, "R");
            if (imagePathList.Count == 0) //그려진   이미지가 없을 시 원본 이미지를 출력한다
            {
                imagePathList = GetImagePathList(resultPath, stepNo);
            }

            if (imagePathList.Count > 0)
            {
                int numOfResultView = DeviceConfig.Instance().GetNumCamera();

                for (int i = 0; i < Math.Min(numOfResultView, imagePathList.Count); i++)
                {
                    if (File.Exists(imagePathList[i]))
                    {
                        if (imagePathList[i].Contains(".3d"))
                        {
                            DrawBoxList[i].Image3d = new Image3D(imagePathList[i]);
                            DrawBoxList[i].MouseDblClicked = drawBox_MouseDoubleClicked;
                        }
                        else
                        {
                            DrawBoxList[i].UpdateImage((Bitmap)Image.FromFile(imagePathList[i]));
                            DrawBoxList[i].MouseDblClicked = null;
                        }
                    }
                }
            }
        }

        private void drawBox_MouseDoubleClicked(UserControl sender)
        {

        }
    }
}
