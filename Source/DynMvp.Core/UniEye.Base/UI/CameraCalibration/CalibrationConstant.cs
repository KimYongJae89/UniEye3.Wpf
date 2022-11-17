using DynMvp.Base;
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

namespace UniEye.Base.UI.CameraCalibration
{
    public partial class CalibrationConstant : UserControl, CameraCalibrationPanel, IMultiLanguageSupport
    {
        public CalibrationConstant()
        {
            InitializeComponent();
            StringManager.AddListener(this);
        }

        public void UpdateLanguage()
        {
            StringManager.UpdateString(this);
        }

        public CalibrationResult Calibrate(Calibration calibration, ImageD imageD)
        {
            CalibrationResult result = calibration.Calibrate(Convert.ToSingle(pelWidth.Text), Convert.ToSingle(pelHeight.Text));
            return result;
        }

        //public void ChangeLanguage()
        //{
        //    labelScaleX.Text = StringManager.GetString(labelScaleX.Text);
        //    labelScaleY.Text = StringManager.GetString(labelScaleY.Text);
        //}

        public void Initialize()
        {
            throw new NotImplementedException();
        }

        public void UpdateData(Calibration calibration)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new UpdateDataDelegate(UpdateData), calibration);
                return;
            }

            pelWidth.Text = calibration.PelSize.Width.ToString();
            pelHeight.Text = calibration.PelSize.Height.ToString();
        }



        public void UpdateResult(CalibrationResult result, int subResultId = -1)
        {

        }
    }
}
