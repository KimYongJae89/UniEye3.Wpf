using DynMvp.Base;
using DynMvp.Data;
using DynMvp.Data.Forms;
using DynMvp.Data.UI;
using DynMvp.Vision;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UniEye.Base.UI.Main2018;

namespace UniEye.Base.UI
{
    public partial class TargetInfoPanel : UserControl, IObjectInfoPanel
    {
        private TeachHandler teachHandlerProbe;
        private bool onValueUpdate = false;

        public AlgorithmValueChangedDelegate ValueChanged = null;
        private ImageD targetGroupImage;

        public ParamValueChangedDelegate ParamValueChanged { get; set; }

        public TargetInfoPanel()
        {
            InitializeComponent();
        }

        public void ClearProbeData()
        {
            targetPictureBox.Image = Properties.Resources.Image; // 아무것도 등록이 안된 경우는 이 이미지를 넣어 준다

            txtTargetName.Text = "";
        }

        public void SelectProbe(Probe probe)
        {
            MessageBox.Show(this, GetType().FullName + " : SelectProbe 이 구현되지 않았습니다.");
            //throw new NotImplementedException();
        }

        public void SelectTarget(Target target)
        {
            onValueUpdate = true;

            txtTargetName.Enabled = false;
            txtTargetName.Text = target.Name;

            onValueUpdate = false;
        }

        public void SetTeachHandler(TeachHandler teachHandler)
        {
            teachHandlerProbe = teachHandler;
        }

        public void UpdateTargetImage(ImageD image)
        {
            targetGroupImage = image;

            if (targetPictureBox.Image != null)
            {
                targetPictureBox.Image.Dispose();
            }

            if (image != null)
            {
                targetPictureBox.Image = image.ToBitmap();
            }
            else
            {
                targetPictureBox.Image = Properties.Resources.Image; // 아무것도 등록이 안된 경우는 이 이미지를 넣어 준다
            }
        }

        public ParamPanel ParamPanel { get; set; }

        private void buttonRefresh_Click(object sender, EventArgs e)
        {
            ITeachObject teachObject = teachHandlerProbe.GetSingleSelected();
            if (teachObject != null)
            {
                List<Target> targetList = teachHandlerProbe.GetTargetList();

                ImageD targetImage = targetGroupImage.ClipImage(targetList[0].BaseRegion);

                teachObject.UpdateTargetImage((Image2D)targetImage);

                if (targetPictureBox.Image != null)
                {
                    targetPictureBox.Image.Dispose();
                }

                targetPictureBox.Image = targetImage.ToBitmap();
            }

            ParamPanel.TestSelectObject();
        }

        private void DefaultTargetInfoControl_Load(object sender, EventArgs e)
        {
            buttonRefresh.Text = StringManager.GetString(buttonRefresh.Text);
            labelTargetName.Text = StringManager.GetString(labelTargetName.Text);
        }

        private void txtTargetName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter)
            {
                return;
            }

            ChangeTargetName(txtTargetName.Text);
        }

        private void txtTargetName_TextChanged(object sender, EventArgs e)
        {
            ChangeTargetName(txtTargetName.Text);
        }

        private void ChangeTargetName(string text)
        {
            if (teachHandlerProbe == null)
            {
                return;
            }

            if (teachHandlerProbe.GetTargetList().Count != 1)
            {
                return;
            }

            if (string.IsNullOrEmpty(text))
            {
                return;
            }

            List<Target> targetList = teachHandlerProbe.GetTargetList();
            foreach (Target target in targetList)
            {
                target.Name = text;
            }

            ParamControl_ValueChanged(ValueChangedType.None, null, null, true);
        }

        private void ParamControl_ValueChanged(ValueChangedType valueChangedType, Algorithm algorithm, AlgorithmParam newParam, bool modified)
        {
            LogHelper.Debug(LoggerType.Operation, "DefaultTargetInfoPanel - ParamControl_ValueChanged");

            if (onValueUpdate == false)
            {
                ValueChanged?.Invoke(valueChangedType, algorithm, newParam, modified);
            }
        }
    }
}
