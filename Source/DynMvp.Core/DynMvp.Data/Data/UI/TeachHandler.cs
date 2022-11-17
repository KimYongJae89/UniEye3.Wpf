using DynMvp.Base;
using DynMvp.Inspect;
using DynMvp.InspectData;
using DynMvp.UI;
using DynMvp.Vision;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;

namespace DynMvp.Data.UI
{
    public interface ITeachObject
    {
        int[] LightTypeIndexArr { get; }
        Figure ShapeFigure { get; set; }
        RotatedRect BaseRegion { get; }
        bool Selected { get; set; }
        void AppendFigures(PositionAligner positionAligner, FigureGroup workingFigures, FigureGroup backgroundFigures, CanvasPanel.Option option);
        void UpdateRegion(RotatedRect rotatedRect);
        void Offset(SizeF offset);
        object Clone();
        void UpdateTargetImage(Image2D targetImage);
        void AddToParent();
    }

    public class TeachHandler
    {
        protected List<ITeachObject> selectedObjs = new List<ITeachObject>();
        public List<ITeachObject> SelectedObjs
        {
            get => selectedObjs;
            set => selectedObjs = value;
        }

        public int Count => selectedObjs.Count;

        protected Rectangle boundary;
        public Rectangle Boundary
        {
            get => boundary;
            set => boundary = value;
        }

        public void Clear()
        {
            selectedObjs.Clear();
        }

        /// <summary>
        /// deplecated : 
        /// </summary>
        protected bool movable = true;
        public bool Movable
        {
            get => movable;
            set => movable = value;
        }
        public string ProbeTypeName { get; set; }
        public string AlgorithmTypeName { get; set; }

        private PositionAligner positionAligner;
        public PositionAligner PositionAligner
        {
            set => positionAligner = value;
        }

        public bool IsEditable()
        {
            return true;
        }

        public bool IsSingleSelected()
        {
            return selectedObjs.Count == 1;
        }

        public ITeachObject GetSingleSelected()
        {
            lock (selectedObjs)
            {
                if (selectedObjs.Count == 1)
                {
                    return selectedObjs[0];
                }
            }

            return null;
        }

        public RectangleF GetBoundRect()
        {
            RectangleF unionRect = RectangleF.Empty;
            foreach (ITeachObject teachObject in selectedObjs)
            {
                unionRect = RectangleF.Union(unionRect, teachObject.BaseRegion.ToRectangleF());
            }

            return unionRect;
        }

        public Probe GetFirstSelectedProbe()
        {
            foreach (ITeachObject teachObject in selectedObjs)
            {
                if (teachObject is Probe)
                {
                    return (Probe)teachObject;
                }
            }

            return null;
        }

        public int[] GetLightTypeIndex()
        {
            var lightTypeIndex = new SortedSet<int>();
            foreach (ITeachObject teachObject in selectedObjs)
            {
                if (teachObject.LightTypeIndexArr != null)
                {
                    foreach (int index in teachObject.LightTypeIndexArr)
                    {
                        lightTypeIndex.Add(index);
                    }
                }
            }

            if (lightTypeIndex.Count > 0)
            {
                return lightTypeIndex.ToArray();
            }
            else
            {
                return new int[] { 0 };
            }
        }


        public List<Probe> GetSelectedProbe()
        {
            var selectedPorbeList = new List<Probe>();
            foreach (ITeachObject teachObject in selectedObjs)
            {
                if (teachObject is Probe)
                {
                    selectedPorbeList.Add((Probe)teachObject);
                }
            }

            return selectedPorbeList;
        }

        public Probe GetSingleSelectedProbe()
        {
            Probe setSingleSelectedProbe = null;
            int probeCount = 0;
            foreach (ITeachObject teachObject in selectedObjs)
            {
                if (teachObject is Probe)
                {
                    if (setSingleSelectedProbe == null)
                    {
                        setSingleSelectedProbe = (Probe)teachObject;
                    }

                    probeCount++;
                }
            }

            if (probeCount == 1)
            {
                return setSingleSelectedProbe;
            }

            return null;
        }

        public List<Probe> GetProbeList()
        {
            var selectedPorbeList = new List<Probe>();
            foreach (ITeachObject teachObject in selectedObjs)
            {
                if (teachObject is Probe)
                {
                    selectedPorbeList.Add((Probe)teachObject);
                }
            }

            return selectedPorbeList;
        }

        public List<Target> GetTargetList()
        {
            var targetList = new List<Target>();
            foreach (ITeachObject teachObject in selectedObjs)
            {
                if (teachObject is Probe probe)
                {
                    if (targetList.IndexOf(probe.Target) == -1)
                    {
                        targetList.Add(probe.Target);
                    }
                }
                else if (teachObject is Target target)
                {
                    if (targetList.IndexOf(target) == -1)
                    {
                        targetList.Add(target);
                    }
                }
            }

            return targetList;
        }

        public List<Target> GetTargetList(int inspectOrder)
        {
            var targetList = new List<Target>();
            foreach (ITeachObject teachObject in selectedObjs)
            {
                if (teachObject is Probe probe)
                {
                    if (targetList.IndexOf(probe.Target) == -1)
                    {
                        if (probe.Target.InspectOrder == inspectOrder)
                        {
                            targetList.Add(probe.Target);
                        }
                    }
                }
                else if (teachObject is Target target)
                {
                    if (targetList.IndexOf(target) == -1)
                    {
                        if (target.InspectOrder == inspectOrder)
                        {
                            targetList.Add(target);
                        }
                    }
                }
            }

            return targetList;
        }


        public void AppendResultFigures(ProbeResultList probeResultList, FigureGroup backgroundFigures)
        {
            var redPen = new Pen(Color.Red);
            var yellowPen = new Pen(Color.Yellow);

            foreach (ITeachObject teachObject in selectedObjs)
            {
                if (teachObject is Probe probe)
                {
                    ProbeResult probeResult = probeResultList.GetProbeResult(probe);
                    if (probeResult != null)
                    {
                        probeResult.AppendResultFigures(backgroundFigures, ResultImageType.Camera);
                    }
                }
                else if (teachObject is Target target)
                {
                    foreach (Probe targetProbe in target.ProbeList)
                    {
                        ProbeResult probeResult = probeResultList.GetProbeResult(targetProbe);
                        if (probeResult != null)
                        {
                            probeResult.AppendResultFigures(backgroundFigures, ResultImageType.Camera);
                        }
                    }
                }
            }
        }

        public bool IsSelectable(Figure figure)
        {
            if (figure.Tag is ITeachObject teachObject)
            {
                if (teachObject is Target)
                {
                    return false;
                }
            }

            return true;
        }

        public void Select(Figure figure)
        {
            if (figure.Tag is ITeachObject teachObject)
            {
                if (teachObject is Target)
                {
                    return;
                }

                selectedObjs.Add(teachObject);
            }
        }

        public void Select(ITeachObject teachObject)
        {
            if (teachObject == null)
            {
                return;
            }

            selectedObjs.Add(teachObject);
        }

        public void Select(List<ITeachObject> teachObjectList)
        {
            foreach (ITeachObject teachObject in teachObjectList)
            {
                selectedObjs.Add(teachObject);
            }
        }

        public void ShowTracker(DrawBox drawBox)
        {
            foreach (ITeachObject teachObject in selectedObjs)
            {
                drawBox.SelectFigureByTag(teachObject);
            }
        }

        public void Unselect(List<Figure> figureList)
        {
            foreach (Figure figure in figureList)
            {
                if (figure.Tag is ITeachObject teachObject)
                {
                    if (teachObject is Target)
                    {
                        return;
                    }

                    selectedObjs.Remove(teachObject);
                }
            }
        }

        public void Copy(List<Figure> figureList)
        {
            selectedObjs.Clear();

            var newProbes = new List<Probe>();

            foreach (Figure figure in figureList)
            {
                if (figure.Tag is Probe probe)
                {
                    //Probe newProbe = (Probe)probe.Clone();

                    //RotatedRect figureRect = figure.GetRectangle();
                    //if (Rectangle.Intersect(boundary, figureRect.ToRectangle()) != figureRect.ToRectangle())
                    //{
                    //    figure.SetRectangle(target.Region);
                    //    continue;
                    //}

                    //newProbe.Region = figureRect;

                    //target.Add(newProbe);

                    //selectedProbes.Add(newProbe);
                    selectedObjs.Add(probe);
                }
            }
        }

        /// <summary>
        /// 새로 생성된 Figure를 받아서, 검사 객체를 복사한다.
        /// 이 클래스의 검사 객체는 Probe이고, 생성된 Probe는 원본 객체를 가지고 있는 Target에 추가된다.
        /// 사용상의 편의를 위해 선택되는 Figure는 하나의 Target으로 제한되거나,
        /// 일정한 규칙에 기반하여 선택되어야 한다.
        /// </summary>
        /// <param name="figureList"></param>
        public void Paste(List<Figure> figureList, FigureGroup workingFigures, FigureGroup backgroundFigures, SizeF pasteOffset)
        {
            selectedObjs.Clear();

            foreach (Figure figure in figureList)
            {
                if (figure.Tag is ITeachObject teachObject)
                {
                    var newTeachObject = (ITeachObject)teachObject.Clone();
                    newTeachObject.AddToParent();

                    newTeachObject.Offset(pasteOffset);

                    newTeachObject.AppendFigures(positionAligner, workingFigures, backgroundFigures, new CanvasPanel.Option());
                }
            }
        }

        public void Move(List<Figure> figureList)
        {
            foreach (Figure figure in figureList)
            {
                RotatedRect rectangle = figure.GetRectangle();
                var figureRect = rectangle.ToRectangle();

                if (figure.Tag is ITeachObject teachObject)
                {
                    if (boundary.Contains(figureRect) == false)
                    {
                        figure.SetRectangle(teachObject.BaseRegion);
                        continue;
                    }

                    teachObject.UpdateRegion(rectangle);
                }
            }
        }

        public void SetAddType(string probeTypeName, string algorithmTypeName)
        {
            ProbeTypeName = probeTypeName;
            AlgorithmTypeName = algorithmTypeName;
        }

        public bool IsSelected()
        {
            return selectedObjs.Count > 0;
        }

        public void DeleteObject()
        {
            List<Target> targetList = GetTargetList();
            if (targetList == null)
            {
                return;
            }

            if (targetList.Count == 0)
            {
                return;
            }

            if (targetList.Count > 1)
            {
                foreach (Target target in targetList)
                {
                    target.InspectStep.RemoveTarget(target);
                }
            }
            else if (targetList[0] != null)
            {
                Target target = targetList[0];
                foreach (ITeachObject teachObject in selectedObjs)
                {
                    if (teachObject is Probe probe)
                    {
                        target.RemoveProbe(probe);
                    }
                }

                if (target.ProbeList.Count == 0)
                {
                    target.InspectStep.RemoveTarget(target);
                }

                foreach (ITeachObject teachObject in selectedObjs)
                {
                    if (teachObject is Target selTarget)
                    {
                        selTarget.InspectStep.RemoveTarget(selTarget);
                    }
                }
            }

            selectedObjs.Clear();
        }

        public void Inspect(InspectStep inspectStep, ImageBuffer imageBuffer, bool saveDebugImage, int cameraIndex,
            Calibration calibration, ProbeResultList probeResultList, IInspectEventListener inspectEventHandler)
        {
            var cancellationTokenSource = new CancellationTokenSource();

            LogHelper.Debug(LoggerType.Operation, "ModellerPage - testTargetButton_Click");

            var inspectParam = new InspectParam(inspectStep.StepNo, positionAligner, calibration, imageBuffer,
                                                        probeResultList, true, cancellationTokenSource.Token, inspectEventHandler);

            //PointF stepPos = positionAligner.Align(inspectStep.Position.ToPointF());
            var stepPos = new PointF();

            List<Target> targetList = GetTargetList();
            targetList.AddRange(inspectStep.GetTargets(Target.TypeGlobalFiducial));
            targetList.AddRange(inspectStep.GetTargets(Target.TypeCalibration));
            GetInspectOrderRange(targetList, out int minOrder, out int maxOrder);

            for (int inspectOrder = minOrder; inspectOrder <= maxOrder; inspectOrder++)
            {
                inspectParam.LocalPositionAligner = inspectStep.GetPositionAligner(cameraIndex, probeResultList);
                inspectParam.LocalCameraCalibration = inspectStep.GetCalibration(cameraIndex, probeResultList);
                inspectParam.InspectStepAlignedPos = stepPos;

                List<Target> orderedTargetList = targetList.FindAll(x => x.InspectOrder == inspectOrder);

                foreach (Target target in orderedTargetList)
                {
                    target.Inspect(inspectParam);
                }
            }
        }

        public void GetInspectOrderRange(List<Target> targetList, out int min, out int max)
        {
            int minValue = int.MaxValue;
            int maxValue = 0;

            targetList.ForEach(x => { minValue = Math.Min(x.InspectOrder, minValue); maxValue = Math.Max(x.InspectOrder, maxValue); });

            min = minValue;
            max = maxValue;
        }

        public void Select(List<Figure> figureList)
        {
            selectedObjs.Clear();

            foreach (Figure figure in figureList)
            {
                if (figure.Tag is ITeachObject teachObject)
                {
                    selectedObjs.Add(teachObject);
                }
            }
        }
    }
}
