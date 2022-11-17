using DynMvp.Base;
using DynMvp.InspectData;
using DynMvp.Vision;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Unieye.WPF.Base.Controls;
using UniScanC.Algorithm;
using UniScanC.Algorithm.Base;
using UniScanC.Enums;
using UniScanC.Module;

namespace UniScanC.Data
{
    public class InspectResult : ProductResult, TupleElement
    {
        private BitmapSource defectImageData = null;
        public BitmapSource DefectImageData => defectImageData = (defectImageData ?? GetDefectImage());  // Debug 및 Display 용        

        private Dictionary<string, object> keyValueList = new Dictionary<string, object>();
        private Dictionary<string, Type> keyTypeList = new Dictionary<string, Type>();

        public string StartTime { get => GetValue<string>("StartTime"); set => SetValue("StartTime", value); }
        public string EndTime { get => GetValue<string>("EndTime"); set => SetValue("EndTime", value); }
        public string LotNo { get => GetValue<string>("LotNo"); set => SetValue("LotNo", value); }

        public int ModuleNo { get => GetValue<int>("ModuleNo"); set => SetValue("ModuleNo", value); }
        public int FrameIndex { get => GetValue<int>("FrameIndex"); set => SetValue("FrameIndex", value); }

        public SizeF Resolution { get => GetValue<SizeF>("Resolution"); set => SetValue("Resolution", value); }
        public SizeF InspectRegion { get => GetValue<SizeF>("InspectRegion"); set => SetValue("InspectRegion", value); } //검사 영역 (현재 이미지 전체)
        public float EdgePos { get => GetValue<float>("EdgePos"); set => SetValue("EdgePos", value); } //왼쪽 엣지 위치

        public new Judgment Judgment { get => GetValue<Judgment>("Judgment"); set => SetValue("Judgment", value); }
        public SizeF PatternSize { get => GetValue<SizeF>("PatternSize"); set => SetValue("PatternSize", value); } //패턴이 있을 시 사이즈

        public string FrameImagePath { get => GetValue<string>("FrameImagePath"); set => SetValue("FrameImagePath", value); } //프레임 이미지 경로
        public BitmapSource FrameImageData { get => GetValue<BitmapSource>("FrameImageData"); set => SetValue("FrameImageData", value); } //프레임 이미지

        public List<Defect> DefectList => GetValue<List<Defect>>("DefectList");  //불량 데이터

        public Dictionary<string, double> LoadFactorList => GetValue<Dictionary<string, double>>("LoadFactorList");

        public InspectResult()
        {
            AddKeyValue<string>("StartTime", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            AddKeyValue<string>("EndTime", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            AddKeyValue<string>("LotNo", null);

            AddKeyValue<int>("ModuleNo", 0);
            AddKeyValue<int>("FrameIndex", 0);

            AddKeyValue<SizeF>("Resolution", SizeF.Empty);
            AddKeyValue<SizeF>("InspectRegion", SizeF.Empty);
            AddKeyValue<float>("EdgePos", 0f);

            AddKeyValue<Judgment>("Judgment", Judgment.OK);
            AddKeyValue<SizeF>("PatternSize", SizeF.Empty);

            AddKeyValue<string>("FrameImagePath", null);
            AddKeyValue<BitmapSource>("FrameImageData", null);

            AddKeyValue<List<Defect>>("DefectList", new List<Defect>());

            AddKeyValue<Dictionary<string, double>>("LoadFactorList", new Dictionary<string, double>());
        }

        public static string[] GetPropNames()
        {
            return new string[] {
                "StartTime", "EndTime", "LotNo", "ModuleNo", "FrameIndex",
                "Resolution", "InspectRegion", "EdgePos",
                "Judgment", "PatternSize",
                "FrameImagePath", "FrameImageData",
                "DefectList" };
        }

        public (string, Type)[] GetPropNameTypes()
        {
            return keyTypeList.Select(f => (f.Key, f.Value)).ToArray();
        }

        public void AddKeyValue<T>(string key, T value)
        {
            AddKey<T>(key);
            SetValue(key, value);
        }

        public void AddKey<T>(string key)
        {
            keyValueList.Add(key, null);
            keyTypeList.Add(key, typeof(T));
        }

        public Type GetType(int i)
        {
            return keyTypeList.ElementAt(i).Value;
        }

        public Type GetType(string prop)
        {
            return keyTypeList[prop];
        }

        public void SetValue<T>(int i, T value)
        {
            string key = keyValueList.ElementAt(i).Key;
            SetValue(key, value);
        }

        public void SetValue(int i, Type type, object value)
        {
            string key = keyValueList.ElementAt(i).Key;
            SetValue(key, type, value);
        }

        public void SetValue<T>(string key, T value)
        {
            bool b = keyTypeList[key].IsEquivalentTo(typeof(T));
            bool b2 = keyTypeList[key].IsSubclassOf(typeof(T));
            if (!b && !b2)
            {
                throw new InvalidCastException();
            }

            keyValueList[key] = value;
        }

        public void SetValue(string key, Type type, object value)
        {
            bool b = keyTypeList[key].IsEquivalentTo(type);
            if (!b)
            {
                throw new InvalidCastException();
            }

            System.Reflection.MethodInfo[] methodInfos = type.GetMethods();
            System.Reflection.MethodInfo methodInfo = Array.Find(methodInfos, f => f.Name == "Add");
            keyValueList[key] = value;
        }

        public T GetValue<T>(int i)
        {
            string key = keyValueList.ElementAt(i).Key;
            return GetValue<T>(key);
        }

        public T GetValue<T>(string key)
        {
            return (T)keyValueList[key];
        }

        public override void CopyFrom(ProductResult result)
        {
            base.CopyFrom(result);

            var inspectResult = result as InspectResult;
            StartTime = inspectResult.StartTime;
            EndTime = inspectResult.EndTime;
            LotNo = inspectResult.LotNo;

            ModuleNo = inspectResult.ModuleNo;
            FrameIndex = inspectResult.FrameIndex;

            Resolution = inspectResult.Resolution;
            InspectRegion = inspectResult.InspectRegion;
            EdgePos = inspectResult.EdgePos;

            Judgment = inspectResult.Judgment;
            PatternSize = inspectResult.PatternSize;

            FrameImagePath = inspectResult.FrameImagePath;
            FrameImageData = inspectResult.FrameImageData?.Clone();

            foreach (Defect defect in inspectResult.DefectList)
            {
                Defect newDefect = defect.Clone();
                DefectList.Add(newDefect);
            }

            foreach (KeyValuePair<string, double> loadFactor in inspectResult.LoadFactorList)
            {
                LoadFactorList[loadFactor.Key] = loadFactor.Value;
            }
        }

        public void AddDefect(Defect defect)
        {
            DefectList.Add(defect);
        }

        public override bool IsGood()
        {
            return Judgment == Judgment.OK;
        }

        //Postgre DB 에서 데이터 읽어오는 기능
        //Frame 별로 데이터 위치 정렬도 한번에 진행하여 넘겨줌
        public static List<InspectResult> Parse(InspectDataImporter dataImporter, List<Dictionary<string, object>> frameDatas, int lastDefectNo, bool loadFrameImage = false, bool loadDefectImage = true, ProgressSource progressSource = null)
        {
            float[] moduleStartPosDic = new float[ModuleManager.Instance.ModuleStateList.Count];
            foreach (InspectModuleState moduleState in ModuleManager.Instance.ModuleStateList.Where(x => x is InspectModuleState))
            {
                moduleStartPosDic[moduleState.ModuleIndex] = Convert.ToSingle(moduleState.StartPos);
            }

            var dataList = new List<InspectResult>();
            foreach (Dictionary<string, object> frameData in frameDatas)
            {
                var inspectResult = new InspectResult();

                DateTime startTime = DateTime.Now;
                if (frameData.TryGetValue("start_time", out object startTimeObj))
                {
                    if (DateTime.TryParse(Convert.ToString(startTimeObj), out DateTime startTimeParse))
                    {
                        startTime = startTimeParse;
                    }
                }
                inspectResult.StartTime = startTime.ToString("HH:mm:ss:fff");
                DateTime endTime = DateTime.Now;
                if (frameData.TryGetValue("end_time", out object endTimeObj))
                {
                    if (DateTime.TryParse(Convert.ToString(endTimeObj), out DateTime endTimeParse))
                    {
                        endTime = endTimeParse;
                    }
                }
                inspectResult.EndTime = endTime.ToString("HH:mm:ss:fff");

                inspectResult.LotNo = Convert.ToString(frameData["lot_name"]);
                inspectResult.FrameIndex = Convert.ToInt32(frameData["frame_index"]);
                inspectResult.ModuleNo = Convert.ToInt32(frameData["module_index"]);
                inspectResult.Judgment = (Judgment)Enum.Parse(typeof(Judgment), Convert.ToString(frameData["judgement"]));
                inspectResult.EdgePos = Convert.ToSingle(frameData["pos_edge"]);
                float regionWidth = Convert.ToSingle(frameData["frame_width"]);
                float regionHeight = Convert.ToSingle(frameData["frame_height"]);
                var inspectRegion = new SizeF(regionWidth, regionHeight);
                inspectResult.InspectRegion = inspectRegion;
                float resolution = Convert.ToSingle(frameData["resolution"]);
                var sizeResolution = new SizeF(resolution, resolution);
                inspectResult.Resolution = sizeResolution;
                float patternWidth = Convert.ToSingle(frameData["pattern_width"]);
                float patternHeight = Convert.ToSingle(frameData["pattern_length"]);
                var patternSize = new SizeF(patternWidth, patternHeight);
                inspectResult.PatternSize = patternSize;
                if (loadFrameImage)
                {
                    inspectResult.FrameImagePath = Convert.ToString(frameData["image_path"]);
                    inspectResult.FrameImageData = dataImporter.ImportFrameImage(inspectResult.FrameImagePath);
                }

                // 각 모듈의 위치 별로 X 값을 Offset 시켜주기위한 변수
                float startPosX = moduleStartPosDic[inspectResult.ModuleNo];
                // 각 프레임의 위치 별로 Y 값을 Offset 시켜주기위한 변수
                float startPosY = Convert.ToSingle(inspectResult.FrameIndex * inspectResult.InspectRegion.Height);
                List<Dictionary<string, object>> defectDatas = dataImporter.ImportDefectData(inspectResult.LotNo, inspectResult.FrameIndex, inspectResult.ModuleNo);
                foreach (Dictionary<string, object> defectData in defectDatas)
                {
                    var defect = new Defect();
                    defect.Tag = inspectResult;
                    defect.InspectTime = inspectResult.EndTime;
                    defect.FrameIndex = Convert.ToInt32(defectData["frame_index"]);
                    defect.ModuleNo = Convert.ToInt32(defectData["module_index"]);
                    defect.DefectNo = lastDefectNo++;
                    defect.DefectTypeName = Convert.ToString(defectData["defect_type"]);
                    float posX = Convert.ToSingle(defectData["pos_x"]);
                    float posY = Convert.ToSingle(defectData["pos_y"]);
                    var defectPos = new PointF(posX + startPosX, (posY + startPosY) / 1000.000f);
                    defect.DefectPos = defectPos;
                    float rectWidth = Convert.ToSingle(defectData["rect_width"]);
                    float rectHeight = Convert.ToSingle(defectData["rect_height"]);
                    float rectX = posX - (rectWidth / 2);
                    float rectY = posY - (rectHeight / 2);
                    var boundRect = new RectangleF(rectX, rectY, rectWidth, rectHeight);
                    defect.BoundingRect = boundRect;
                    defect.Area = Convert.ToSingle(defectData["area"]);
                    defect.MinGv = Convert.ToInt32(defectData["min_gv"]);
                    defect.MaxGv = Convert.ToInt32(defectData["max_gv"]);
                    defect.AvgGv = Convert.ToInt32(defectData["avg_gv"]);
                    if (loadDefectImage)
                    {
                        defect.DefectImagePath = Convert.ToString(defectData["image_path"]);
                        if (System.IO.File.Exists(defect.DefectImagePath))
                        {
                            defect.DefectImage = dataImporter.ImportDefectImage(defect.DefectImagePath);
                        }
                    }
                    inspectResult.AddDefect(defect);

                    if (progressSource != null)
                    {
                        if (progressSource.CancellationTokenSource.IsCancellationRequested)
                        {
                            return null;
                        }

                        progressSource.StepIt();
                    }
                }
                dataList.Add(inspectResult);
            }

            return dataList;
        }

        public void UpdateJudgement()
        {
            if (Judgment == Judgment.Skip)
            {
                return;
            }

            bool isNG = false;
            // 스킵 조건이 아닌 불량 항목을 찾는다.
            List<Defect> nonSkipDefectList = DefectList.FindAll(defect => !defect.TopDefectCategory().IsSkip);
            foreach (Defect nonSkipDefect in nonSkipDefectList)
            {
                DefectCategory topCategory = nonSkipDefect.TopDefectCategory();
                // 카테고리가 같은 불량들의 개수를 센다.
                int defectCount = DefectList.Count(defect => defect.TopDefectCategory().Name == topCategory.Name);
                // 카테고리에서 선택한 일정 불량 개수가 넘어가면 NG로 판단한다.
                isNG |= defectCount >= topCategory.DefectCount;
            }

            // 카테고리가 Skip이 아닌게 1개라도 있으면 NG처리. (패치 전 코드)
            //isNG = DefectList.Exists(f => f.DefectCategories.Exists(g => !g.IsSkip));
            Judgment = isNG ? Judgment.NG : Judgment.OK;
        }

        public int GetDefectCount(EDefectType defectType)
        {
            List<Defect> findList = DefectList.FindAll(x => x.DefectType == defectType);
            return findList == null ? 0 : findList.Count;
        }

        public BitmapSource GetDefectImage()
        {
            if (FrameImageData == null)
            {
                return null;
            }

            var imageSize = new Size(FrameImageData.PixelWidth, FrameImageData.PixelHeight);
            SizeF resolution = Resolution;

            // 이미지에 사각형 그리기
            var visual = new DrawingVisual();

            DrawingContext context = visual.RenderOpen();
            context.DrawImage(FrameImageData, new System.Windows.Rect(0, 0, imageSize.Width, imageSize.Height));

            var frameText = new FormattedText($"F{FrameIndex} / D{DefectList.Count}", CultureInfo.CurrentCulture, System.Windows.FlowDirection.LeftToRight, new Typeface("Verdana"), 100, System.Windows.Media.Brushes.Black);
            context.DrawText(frameText, new System.Windows.Point(0, 0));

            DefectList.ForEach(f =>
            {
                RectangleF rect = f.BoundingRect;
                var drawRect = System.Windows.Rect.Inflate(new System.Windows.Rect(rect.X, rect.Y, rect.Width, rect.Height), 50, 50);
                context.DrawRectangle(null, new System.Windows.Media.Pen(System.Windows.Media.Brushes.Red, 5), drawRect);

                string str = $"{rect.Width * resolution.Width:F1} / {rect.Height * resolution.Height:F1} / {f.AvgGv}";
                var text = new FormattedText(str, CultureInfo.CurrentCulture, System.Windows.FlowDirection.LeftToRight, new Typeface("Verdana"), 42, System.Windows.Media.Brushes.Red);
                context.DrawText(text, new System.Windows.Point(rect.X, rect.Y));
            });
            context.Close();

            var renderTargetBitmap = new RenderTargetBitmap(imageSize.Width, imageSize.Height, 96, 96, PixelFormats.Default);
            renderTargetBitmap.Render(visual);

            // 이미지 저장
            BitmapEncoder encoder = new BmpBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(renderTargetBitmap));
            using (Stream stream = new MemoryStream())
            {
                encoder.Save(stream);
                stream.Seek(0, SeekOrigin.Begin);

                var bmpImage = new BitmapImage();
                bmpImage.BeginInit();
                bmpImage.CacheOption = BitmapCacheOption.OnLoad;
                bmpImage.StreamSource = stream;
                bmpImage.EndInit();
                bmpImage.Freeze();
                return bmpImage;
            }
        }
    }
}
