using DynMvp.Base;
using DynMvp.Data;
using DynMvp.InspectData;
using DynMvp.UI;
using System;
using System.Drawing;
using System.Windows.Forms;
using UniEye.Base.Data;

namespace UniEye.Base.UI
{
    public enum ReportMode
    {
        Review, Report
    }

    public interface IDefectReportPanel
    {
        void Initialize(ReportMode reportMode, Schema schema, ProductResult inspectionResult);
        void Terminate();
        void MovePrev();
        void MoveNext();
        void SetGood();
        void SetDefect();
    }

    public partial class DefectReviewForm : Form
    {
        private ProductResult inspectResult;

        //        private Machine machine;

        private IDefectReportPanel defectReportPanel;
        public IDefectReportPanel DefectReportPanel
        {
            set => defectReportPanel = value;
        }

        public DefectReviewForm()
        {
            InitializeComponent();

            defectReportPanel = UiManager.Instance().CreateDefectReportPanel();

            var userControl = (UserControl)defectReportPanel;
            userControl.Dock = System.Windows.Forms.DockStyle.Fill;

            panelReport.Controls.Add(userControl);

            btnAlarmOff.Text = StringManager.GetString(btnAlarmOff.Text);
        }

        public void Initialize(ProductResult inspectionResult)
        {
            StepModel model = ModelManager.Instance().CurrentStepModel;
            defectReportPanel.Initialize(ReportMode.Review, model.ModelSchema, inspectionResult);

            inspectResult = inspectionResult;

            UpdateResult();
        }

        private void ProbeDefectProcessForm_Load(object sender, EventArgs e)
        {

        }

        private void UpdateResult()
        {
            int defectCount = inspectResult.GetProbeDefectCount();
            numDefectTarget.Text = defectCount.ToString();
            if (defectCount == 0)
            {
                labelResult.BackColor = Color.LightGreen;
                labelResult.Text = StringManager.GetString("Good");
            }
            else
            {
                labelResult.BackColor = Color.Red;
                labelResult.Text = StringManager.GetString("NG");
            }
        }

        private void retryButton_Click(object sender, EventArgs e)
        {

        }

        private void probeNgButton_Click(object sender, EventArgs e)
        {
            defectReportPanel.SetDefect();
        }

        private void panelTop_MouseDown(object sender, MouseEventArgs e)
        {
            FormMoveHelper.MouseDown(this);
        }

        private void labelTitle_MouseDown(object sender, MouseEventArgs e)
        {
            FormMoveHelper.MouseDown(this);
        }

        private void prevButton_Click(object sender, EventArgs e)
        {
            defectReportPanel.MovePrev();
        }

        private void nextButton_Click(object sender, EventArgs e)
        {
            defectReportPanel.MoveNext();
        }

        private void probeGoodButton_Click(object sender, EventArgs e)
        {
            defectReportPanel.SetGood();
        }

        private void btnAlarmOff_Click(object sender, EventArgs e)
        {
            SystemState.Instance().Alarmed = false;
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            DialogResult result;
            if (inspectResult.IsGood())
            {
                result = MessageForm.Show(this, StringManager.GetString("Do you want to close this form?"), MessageFormType.YesNo);
            }
            else
            {
                result = MessageForm.Show(this, StringManager.GetString("Are you sure you want to process as all NG?"), MessageFormType.YesNo);
            }

            if (result == DialogResult.No)
            {
                return;
            }

            defectReportPanel.Terminate();
            Close();
        }
    }
}
