using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using UniScanC.Algorithm.Base;
using UniScanC.Data;
using UniScanC.Struct;

namespace UniScanC.AlgoTask
{
    public static class AlgoTaskSettingDefault
    {
        /// 생코뱅 ATM 기본값
        public static AlgoTaskManagerSetting SaintGobain_All => new AlgoTaskManagerSetting(
            new INodeParam[]
            {
                new UniScanC.Algorithm.LineCalibrate.LineCalibratorParam(), // 0
                new UniScanC.Algorithm.PatternSizeCheck.PatternSizeCheckerParam(), // 1
                new UniScanC.Algorithm.ColorCheck.ColorCheckerParam(), // 2
                new UniScanC.Algorithm.PlainFilmCheck.PlainFilmCheckerParam(), // 3
                new UniScanC.Algorithm.Base.SetNodeParam<Defect>("UnionNode", UniScanC.Algorithm.Base.ESetNodeType.Union) // 4
            },
            new ILink[]
            {
                new LinkS("ModuleImageDataByte", "ImageDataByte", "LineCalibrator", "ImageDataByte"),
                new LinkS("LineCalibrator", "ImageData", "PatternSizeChecker", "ImageData"),

                new LinkS("LineCalibrator", "ImageData", "ColorChecker", "ImageData"),
                new LinkS("PatternSizeChecker", "RoiMask", "ColorChecker", "RoiMask"),
                new LinkS("ModuleImageDataByte", "FrameNo", "ColorChecker", "FrameNo"),


                new LinkS("LineCalibrator", "ImageData", "PlainFilmChecker", "ImageData"),
                new LinkS("PatternSizeChecker", "RoiMask", "PlainFilmChecker", "RoiMask"),
                new LinkS("ModuleImageDataByte", "FrameNo", "PlainFilmChecker", "FrameNo"),


                new LinkS("ColorChecker", "DefectList", "UnionNode", "List"),
                new LinkS("PlainFilmChecker", "DefectList", "UnionNode", "List"),


                new LinkS("ModuleImageDataByte", "FrameNo", "InspectResult", "FrameIndex"),
                new LinkEx("LineCalibrator", "ImageData", "InspectResult", "FrameImageData", typeof(ImageData), typeof(BitmapSource)),
                new LinkEx("LineCalibrator", "ImageData", "InspectResult", "InspectRegion", typeof(ImageData), typeof(SizeF)),
                new LinkEx("PatternSizeChecker", "RoiMask", "InspectResult", "EdgePos", typeof(RoiMask), typeof(float)),
                new LinkS("PatternSizeChecker", "PatternSize", "InspectResult", "PatternSize"),
                new LinkS("UnionNode", "List", "InspectResult", "DefectList")
            }
            );

        /// 삼성전기 곡면 ATM 기본값
        public static AlgoTaskManagerSetting SamsungScreen_All => new AlgoTaskManagerSetting(
            new INodeParam[]
            {
                new UniScanC.Algorithm.LineCalibrate.LineCalibratorParam(), // 0
                new UniScanC.Algorithm.RoiFind.RoiFinderParam(), // 1
                new UniScanC.Algorithm.PlainFilmCheck.PlainFilmCheckerParam() // 2
            },
            new ILink[]
            {
                new LinkS("ModuleImageDataByte", "ImageDataByte", "LineCalibrator", "ImageDataByte"),

                new LinkS("LineCalibrator", "ImageData", "RoiFinder", "ImageData"),

                new LinkS("LineCalibrator", "ImageData", "PlainFilmChecker", "ImageData"),
                new LinkS("RoiFinder", "RoiMask", "PlainFilmChecker", "RoiMask"),
                new LinkS("ModuleImageDataByte", "FrameNo", "PlainFilmChecker", "FrameNo"),

                new LinkS("ModuleImageDataByte", "FrameNo", "InspectResult", "FrameIndex"),
                new LinkEx("LineCalibrator", "ImageData", "InspectResult", "InspectRegion", typeof(ImageData), typeof(SizeF)),
                new LinkEx("RoiFinder", "RoiMask", "InspectResult", "EdgePos", typeof(RoiMask), typeof(float)),
                new LinkS("RoiFinder", "PatternSize", "InspectResult", "PatternSize"),
                new LinkS("PlainFilmChecker", "DefectList", "InspectResult", "DefectList")
            }
            );

        public static void SaveAll(string v)
        {
            Directory.CreateDirectory(v);
            SaintGobain_All.Save(Path.Combine(v, "SaintGobain_All.json"));
            SamsungScreen_All.Save(Path.Combine(v, "SamsungScreen_All.json"));
        }
    }
}
