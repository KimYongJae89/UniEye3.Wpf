using DynMvp.Base;
using DynMvp.Component.DepthSystem.DepthViewer;
using DynMvp.Data;
using DynMvp.Data.UI;
using DynMvp.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using UniEye.Base.Config;
using UniEye.Base.Settings;
using UniEye.Base.UI;

namespace UniEye.Base.UI.Main
{
    public partial class ReportPage : UserControl, IMainTabPage, IModelEventListener
    {
        private IReportPanel reportPanel = null;
        private bool onUpdateList = false;

        public ReportPage()
        {
            InitializeComponent();

            reportPanel = UiManager.Instance().CreateReportPanel();
        }

        public void ChangeCaption()
        {
            labelTotal.Text = StringManager.GetString(labelTotal.Text);
            labelNG.Text = StringManager.GetString(labelNG.Text);
            labelGood.Text = StringManager.GetString(labelGood.Text);
            searchNg.Text = StringManager.GetString(searchNg.Text);
            searchGood.Text = StringManager.GetString(searchGood.Text);
            labelStart.Text = StringManager.GetString(labelStart.Text);
            labelEnd.Text = StringManager.GetString(labelEnd.Text);
            labelModel.Text = StringManager.GetString(labelModel.Text);
            labelModel.Text = StringManager.GetString(labelModel.Text);
            labelStart.Text = StringManager.GetString(labelStart.Text);
            labelEnd.Text = StringManager.GetString(labelEnd.Text);
            searchGood.Text = StringManager.GetString(searchGood.Text);
            searchNg.Text = StringManager.GetString(searchNg.Text);
            searchFalseReject.Text = StringManager.GetString(searchFalseReject.Text);
            btnSearch.Text = StringManager.GetString(btnSearch.Text);
        }

        public string TabName => "Report";

        public TabKey TabKey => TabKey.Report;

        public Bitmap TabIcon => Properties.Resources.chart;

        public Bitmap TabSelectedIcon => Properties.Resources.chart_sel;

        public Color TabSelectedColor => Color.FromArgb(102, 24, 136);

        public bool IsAdminPage => false;

        public Uri Uri => throw new NotImplementedException();

        public void Initialize()
        {
            stepNo.Text = "0";
        }

        private void ReportPage_Load(object sender, EventArgs e)
        {
            RefreshReportPage();
        }

        public void RefreshReportPage()
        {
            ModelManager.Instance().Refresh();

            UpdateModelCombo();

            startDate.CustomFormat = "yyyy-MM-dd";
            endDate.CustomFormat = "yyyy-MM-dd";

            startHour.SelectedIndex = 0;
            endHour.SelectedIndex = 23;
        }

        public void UpdateModelCombo()
        {
            modelCombo.Items.Clear();

            foreach (ModelDescription md in ModelManager.Instance())
            {
                modelCombo.Items.Add(md.Name);
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (modelCombo.SelectedItem == null)
            {
                MessageBox.Show("Model is not selected");
                return;
            }

            onUpdateList = true;

            int goodCnt = 0;
            int ngCnt = 0;

            serialDataGridView.Columns.Clear();

            serialDataGridView.ColumnCount = 2;
            serialDataGridView.ColumnHeadersVisible = true;
            serialDataGridView.Columns[0].Name = "Serial No.";
            serialDataGridView.Columns[1].Name = "Result";

            string modelName = modelCombo.SelectedItem.ToString();

            bool searchAll = (searchGood.Checked == searchNg.Checked);
            var startTime = new DateTime(startDate.Value.Year, startDate.Value.Month, startDate.Value.Day, Convert.ToInt32(startHour.Text), 0, 0);
            var endTime = new DateTime(endDate.Value.Year, endDate.Value.Month, endDate.Value.Day, Convert.ToInt32(endHour.Text), 59, 59);
            string defaultPath = PathConfig.Instance().Result + @"\" + modelName;

            var directoryInfo = new DirectoryInfo(defaultPath);
            var searchDirector = new List<DirectoryInfo>();

            if (directoryInfo.Exists == false)
            {
                return;
            }

            foreach (DirectoryInfo directory in directoryInfo.GetDirectories())
            {
                if (startTime.Date <= directory.CreationTime.Date && directory.CreationTime.Date <= endTime.Date)
                {
                    searchDirector.Add(directory);
                }
            }
            if (searchDirector.Count == 0)
            {
                MessageForm.Show(ParentForm, "Not found result data.");
                return;
            }

            var resultFiles = new List<FileInfo>();
            var dailyResultFiles = new List<string>();

            foreach (DirectoryInfo directory in searchDirector)
            {
                foreach (DirectoryInfo di in directory.GetDirectories())
                {
                    foreach (FileInfo file in di.GetFiles())
                    {
                        if (file == null)
                        {
                            continue;
                        }

                        if (file.Name == "result.csv")
                        {
                            resultFiles.Add(file);
                            dailyResultFiles.Add(file.FullName);
                        }
                    }
                }
            }
            //모든 파일을 불러 온다
            foreach (string dailyResultFile in dailyResultFiles)
            {
                using (StreamReader sr = File.OpenText(dailyResultFile))
                {
                    string lineStr;
                    var tempList = new List<string>();
                    while ((lineStr = sr.ReadLine()) != null)
                    {
                        tempList.Add(lineStr);
                    }
                    string[] words = tempList[1].Split(new char[] { ',' });
                    string serialNo = words[1].Trim();
                    string tempInspectionTime = words[2].Trim();
                    var inspectionTime = DateTime.Parse(tempInspectionTime);
                    string resultStr = words[3].Trim();

                    if (resultStr == "Reject")
                    {
                        resultStr = "NG";
                    }

                    if (resultStr == "Accept")
                    {
                        resultStr = "OK";
                    }

                    if (resultStr == "FalseReject")
                    {
                        resultStr = "OverKill";
                    }

                    if (searchAll == false)
                    {
                        if (searchNg.Checked && resultStr != "NG")
                        {
                            continue;
                        }

                        if (searchGood.Checked && resultStr != "OK")
                        {
                            continue;
                        }

                        if (searchFalseReject.Checked && resultStr != "OverKill")
                        {
                            continue;
                        }
                    }
                    if (startTime <= inspectionTime && inspectionTime <= endTime)
                    {
                        int rowIndex = serialDataGridView.Rows.Add(serialNo, resultStr);

                        serialDataGridView.Rows[rowIndex].Tag = inspectionTime;

                        if (resultStr == "NG")
                        {
                            serialDataGridView.Rows[rowIndex].Cells[1].Style.BackColor = Color.Red;
                            ngCnt++;
                        }
                        else
                        {
                            serialDataGridView.Rows[rowIndex].Cells[1].Style.BackColor = Color.LightGreen;
                            goodCnt++;
                        }
                    }
                }
            }

            productionTotal.Text = (ngCnt + goodCnt).ToString();
            productionNg.Text = ngCnt.ToString();
            productionGood.Text = goodCnt.ToString();

            serialDataGridView.Sort(serialDataGridView.Columns[0], ListSortDirection.Descending);

            onUpdateList = false;
        }

        public void ModelAutoSelector()
        {
            ModelBase curModel = ModelManager.Instance().CurrentModel;
            if (curModel.IsEmpty() == true)
            {
                return;
            }

            if (modelCombo.Text != curModel.Name)
            {
                modelCombo.SelectedItem = curModel.Name;
            }
        }

        private void serialDataGridView_DoubleClick(object sender, EventArgs e)
        {
            if (serialDataGridView.SelectedRows.Count == 0)
            {
                return;
            }

            int rowIndex = serialDataGridView.SelectedRows[0].Index;
            string resultPath = GetResultPath(rowIndex);

            if (string.IsNullOrEmpty(resultPath) == false)
            {
                UiManager.Instance().MainForm.ModifyTeaching(resultPath);
            }
        }

        private string GetResultPath(int rowIndex)
        {
            string modelName = modelCombo.SelectedItem?.ToString();
            if (string.IsNullOrEmpty(modelName))
            {
                return "NULL";
            }

            string serialNo = serialDataGridView.Rows[rowIndex].Cells[0].Value.ToString();
            var inspectionTime = (DateTime)serialDataGridView.Rows[rowIndex].Tag;

            string date = inspectionTime.ToString("yyyyMMdd");
            string time = serialNo.Substring(8, 2);
            return string.Format("{0}\\{1}\\{2}\\{3}", PathConfig.Instance().Result, modelName, date, serialNo);
        }

        private void serialDataGridView_SelectionChanged(object sender, EventArgs e)
        {
            SerialDataGridViewCellEvent();
        }

        private void modelCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            serialDataGridView.Rows.Clear(); //모델이 바뀌면 데이터 리스트 초기화

            reportPanel.ClearPanel();


            if (ModelManager.Instance().CurrentModel is DynMvp.Data.StepModel model)
            {
                int count = model.GetInspectStepList().Count();
                if (count > 0)
                {
                    for (int i = 0; i < count; i++)
                    {
                        stepNo.Items.Add(i);
                    }

                    stepNo.Text = "0";
                }
            }
        }

        private void serialDataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            SerialDataGridViewCellEvent();
        }

        private void SerialDataGridViewCellEvent()
        {
            if (serialDataGridView.SelectedRows.Count == 0 || onUpdateList)
            {
                return;
            }

            int rowIndex = serialDataGridView.SelectedRows[0].Index;

            string resultPath = GetResultPath(rowIndex);

            if (resultPath == "NULL")
            {
                LogHelper.Debug(LoggerType.Error, "Model is not selected.");
                if (MessageBox.Show(StringManager.GetString("Model is not selected. Please select model.")) == DialogResult.OK)
                {
                    return;
                }
            }

            reportPanel.ShowResult(resultPath, Convert.ToInt32(stepNo.Text));
        }

        private void stepNo_SelectedIndexChanged(object sender, EventArgs e)
        {
            SerialDataGridViewCellEvent();
        }

        private void ReportPanel_VisibleChanged(object sender, EventArgs e)
        {
            if (Visible)
            {
                RefreshReportPage();
            }
        }

        public void OnIdle()
        {

        }

        public void TabPageVisibleChanged(bool visibleFlag)
        {
            if (visibleFlag == true)
            {
                RefreshReportPage();
            }
        }

        public void ModelOpen(ModelBase model)
        {
            modelCombo.Text = model.Name;
        }

        public void ModelClosed(ModelBase model)
        {
            modelCombo.Text = "";
        }

        public void ModelListChanged()
        {
            UpdateModelCombo();
        }

        public void ProcessKeyDown(KeyEventArgs e) { }
    }
}
