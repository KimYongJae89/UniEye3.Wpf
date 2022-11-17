using DynMvp.Base;
using DynMvp.Component.DepthSystem;
using DynMvp.Component.DepthSystem.UI;
using DynMvp.Data;
using DynMvp.Data.UI;
using DynMvp.Devices;
using DynMvp.Devices.Comm;
using DynMvp.Inspect;
using DynMvp.Vision;
using System;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;
using UniEye.Base.Config;
using UniEye.Base.Data;
using UniEye.Base.Inspect;
using UniEye.Base.MachineInterface;
using UniEye.Base.UI;

namespace UniEye.Base.Settings.UI
{
    public partial class ConfigForm : Form
    {
        private AlignDataInterfaceInfo alignDataInterfaceInfo;
        private ConfigDevicePanel devicePanel;
        private string[] systemTypes;
        private bool lowLevelUser = false;

        public ConfigForm(bool lowLevelUser = false)
        {
            InitializeComponent();

            devicePanel = new ConfigDevicePanel();
            devicePanel.Dock = DockStyle.Fill;

            tabPageDeviceNew.Controls.Add(devicePanel);

            devicePanel.Location = new System.Drawing.Point(3, 3);
            devicePanel.Name = "devicePanel";
            devicePanel.Size = new System.Drawing.Size(409, 523);
            devicePanel.Dock = DockStyle.Fill;
            devicePanel.Visible = true;

            labelTitle.Text = StringManager.GetString(labelTitle.Text);
            labelSystemType.Text = StringManager.GetString(labelSystemType.Text);
            labelLanguage.Text = StringManager.GetString(labelLanguage.Text);
            labelProductLogo.Text = StringManager.GetString(labelProductLogo.Text);
            labelCompanyLogo.Text = StringManager.GetString(labelCompanyLogo.Text);
            labelImagingLibrary.Text = StringManager.GetString(labelImagingLibrary.Text);
            buttonOk.Text = StringManager.GetString(buttonOk.Text);
            buttonCancel.Text = StringManager.GetString(buttonCancel.Text);

            this.lowLevelUser = lowLevelUser;
        }

        public void Init(string[] systemTypes)
        {
            this.systemTypes = systemTypes;
        }

        private void ConfigForm_Load(object sender, EventArgs e)
        {
            var operationConfig = OperationConfig.Instance();
            var deviceConfig = DeviceConfig.Instance();
            var inspectConfig = InspectConfig.Instance();
            var uiConfig = UiConfig.Instance();
            var pathSettings = PathConfig.Instance();
            var lightConfig = LightConfig.Instance();

            virtualMode.Checked = deviceConfig.VirtualMode;

            language.Text = uiConfig.Language.ToString();
            checkShowScore.Checked = uiConfig.ShowScore;
            checkShowNGImage.Checked = uiConfig.ShowNGImage;
            title.Text = uiConfig.Title;
            devicePanel.Title = title.Text;
            programTitle.Text = uiConfig.ProgramTitle;

            if (string.IsNullOrEmpty(operationConfig.SystemType) == true)
            {
                cmbSystemType.SelectedItem = "None";
            }
            else
            {
                cmbSystemType.SelectedItem = operationConfig.SystemType;
            }

            imagingLibrary.SelectedIndex = (int)operationConfig.ImagingLibrary;
            dataPathType.SelectedIndex = (int)operationConfig.DataPathType;
            checkUseLoginForm.Checked = operationConfig.UseUserManager;
            dataRestoringDaysNumeric.Value = operationConfig.ResultStoringDays;

            saveTargetImage.Checked = inspectConfig.SaveTargetImage;
            saveProbeImage.Checked = inspectConfig.SaveProbeImage;
            saveCameraImage.Checked = inspectConfig.SaveCameraImage;
            chkUseSaveResultFigure.Checked = inspectConfig.SaveResultFigure;
            cmbImageNameFormat.Text = inspectConfig.ImageNameFormat;

            numLightType.Value = lightConfig.NumLightType;

            companyLogo.Text = pathSettings.CompanyLogo;
            productLogo.Text = pathSettings.ProductLogo;

            EnableControls();

            if (lowLevelUser == true)
            {
                tabMain.TabPages.RemoveAt(4);
                tabMain.TabPages.RemoveAt(3);
                tabMain.TabPages.RemoveAt(1);
                tabMain.TabPages.RemoveAt(0);
            }
        }

        private void EnableControls()
        {

        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            var operationConfig = OperationConfig.Instance();
            var deviceConfig = DeviceConfig.Instance();
            var inspectConfig = InspectConfig.Instance();
            var uiConfig = UiConfig.Instance();
            var pathSettings = PathConfig.Instance();
            var lightConfig = LightConfig.Instance();

            uiConfig.Language = language.Text;
            operationConfig.SystemType = cmbSystemType.SelectedItem.ToString();
            operationConfig.ImagingLibrary = (ImagingLibrary)imagingLibrary.SelectedIndex;
            operationConfig.DataPathType = (DataPathType)dataPathType.SelectedIndex;
            inspectConfig.SaveProbeImage = saveProbeImage.Checked;
            inspectConfig.SaveTargetImage = saveTargetImage.Checked;
            inspectConfig.SaveCameraImage = saveCameraImage.Checked;
            inspectConfig.SaveResultFigure = chkUseSaveResultFigure.Checked;
            uiConfig.ShowScore = checkShowScore.Checked;
            uiConfig.ShowNGImage = checkShowNGImage.Checked;
            operationConfig.UseUserManager = checkUseLoginForm.Checked;
            operationConfig.ResultStoringDays = (int)dataRestoringDaysNumeric.Value;
            inspectConfig.ImageNameFormat = cmbImageNameFormat.Text;

            pathSettings.CompanyLogo = companyLogo.Text;
            pathSettings.ProductLogo = productLogo.Text;

            deviceConfig.VirtualMode = virtualMode.Checked;
            lightConfig.NumLightType = (int)numLightType.Value;

            uiConfig.ProgramTitle = programTitle.Text;
            uiConfig.Title = title.Text;

            devicePanel.SaveSetting();

            operationConfig.Save();
            deviceConfig.Save();
            inspectConfig.Save();
            uiConfig.Save();
            pathSettings.Save();
            lightConfig.Save();
        }

        private void buttonSelectCompanyLogo_Click(object sender, EventArgs e)
        {
            var dialog = new OpenFileDialog();
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                companyLogo.Text = dialog.FileName;
            }
        }

        private void buttonSelectProductLogo_Click(object sender, EventArgs e)
        {
            var dialog = new OpenFileDialog();
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                productLogo.Text = dialog.FileName;
            }
        }

        private void buttonConfigLight_Click(object sender, EventArgs e)
        {
            //            LightControlListForm form = new LightControlListForm();
            //            form.ShowDialog();
        }

        private void buttonConfigAlignmentInterface_Click(object sender, EventArgs e)
        {
            var form = new AlignDataInterfaceInfoForm();
            form.AlignDataInterfaceInfo = alignDataInterfaceInfo;
            if (form.ShowDialog() == DialogResult.OK)
            {
                alignDataInterfaceInfo = form.AlignDataInterfaceInfo;
            }
        }

        private void configDepthScanner1_Click(object sender, EventArgs e)
        {
            ConfigDepthScanner(0);
        }

        private void ConfigDepthScanner(int index)
        {
            var depthScannerConfiguration = new DepthScannerConfiguration();
            string filePath = string.Format("{0}\\DepthScannerConfiguration.xml", BaseConfig.Instance().ConfigPath);
            if (File.Exists(filePath) == true)
            {
                depthScannerConfiguration.LoadConfiguration(filePath);
            }

            DepthScannerInfo depthScannerInfo = depthScannerConfiguration.GetDepthScannerInfo(0);
            if (depthScannerInfo == null)
            {
                depthScannerInfo = new ExymaScannerInfo();
                depthScannerConfiguration.AddDepthScannerInfo(depthScannerInfo);
            }

            var form = new ExymaSettingForm();
            form.ExymaScannerInfo = (ExymaScannerInfo)depthScannerInfo;
            if (form.ShowDialog() == DialogResult.OK)
            {
                depthScannerConfiguration.SaveConfiguration(filePath);
            }
        }

        private void configDepthScanner2_Click(object sender, EventArgs e)
        {
            ConfigDepthScanner(1);
        }
    }
}
