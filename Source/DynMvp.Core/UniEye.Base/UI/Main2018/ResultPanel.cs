using DynMvp.Base;
using DynMvp.Data;
using DynMvp.Data.UI;
using DynMvp.InspectData;
using Infragistics.Win.UltraWinDock;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UniEye.Base.UI.Main2018
{
    public partial class ResultPanel : UserControl, IModellerPane
    {
        private TryInspectionResultView2 tryInspectionResultView;

        public ResultPanel()
        {
            InitializeComponent();

            tryInspectionResultView = new TryInspectionResultView2();
            tryInspectionResultView.Dock = DockStyle.Fill;

            Controls.Add(tryInspectionResultView);
            //this.tryInspectionResultView.TeachHandlerProbe = teachHandler;
            //this.tryInspectionResultView.TryInspectionResultCellClicked = UpdateImageFigure;
        }

        public string Title => "Result";

        public PaneType PaneType => PaneType.Result;

        public Control Control => this;

        public DockedLocation DockedLocation => DockedLocation.DockedRight;

        public void OnPreSelectedInspect()
        {
            tryInspectionResultView.ClearResult();
        }

        public void OnPostSelectedInspect(ProbeResultList probeResultList)
        {
            tryInspectionResultView.SetResult(probeResultList);
        }

        public void StepChanged(InspectStep inspectStep)
        {
        }

        public void Clear()
        {

        }

        public void PointSelected(PointF point, ref bool processingCancelled)
        {

        }

        public void UpdateImage(Image2D sourceImage2d, int lightTypeIndex)
        {

        }

        public void SelectObject(List<ITeachObject> teachObjectList)
        {

        }
    }
}
